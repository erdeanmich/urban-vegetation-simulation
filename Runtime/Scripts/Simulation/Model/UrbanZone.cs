using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UrbanVegetation.Plants;
using UrbanVegetation.UrbanZones;
using Random = System.Random;

namespace UrbanVegetation.Simulation.Model
{
	public class UrbanZone : IUrbanZone
	{
		private readonly UrbanZonePrototype urbanZonePrototype;
		private readonly List<Vector2> area;
		private Func<Vector2, float> getGroundConditionForPosition;
		private Vector2 light;
		private List<IPlant> vegetation;
		private readonly Dictionary<UrbanZonePlacementStrategy, Func<List<Vector2>>> handleOfPlacementStrategy;
		private Random rng = new Random();

		public UrbanZone(UrbanZonePrototype urbanZonePrototype, List<Vector2> area)
		{
			this.urbanZonePrototype = urbanZonePrototype;
			this.area = area;
			
			handleOfPlacementStrategy = new Dictionary<UrbanZonePlacementStrategy, Func<List<Vector2>>>
			{
				{ UrbanZonePlacementStrategy.Random, CalcRandomPositions },
				{ UrbanZonePlacementStrategy.Cluster, CalcClusteredPositions },
				{ UrbanZonePlacementStrategy.Edge, CalcEdgePositions },
				{ UrbanZonePlacementStrategy.Regular, CalcRegularPositions }
			};
		}

		public UrbanZoneClass ZoneClass => urbanZonePrototype.UrbanZoneClass;

		public void SetInitialVegetation(List<IPlantPrototype> plantPrototypes, Func<Vector2, float> getGroundConditionForPosition, Vector2 light)
		{
			this.getGroundConditionForPosition = getGroundConditionForPosition;
			this.light = light;

			vegetation = new List<IPlant>();
			if (plantPrototypes.Count == 0)
			{
				return;
			}
			
			var plantPositions = GetPlantPositions();

			plantPositions.ForEach(position =>
			{
				var plantPrototype = rng.Next(plantPrototypes.Count);
				vegetation.Add(new Plant(plantPrototypes[plantPrototype], position));
			});
		}

		public void UpdateVegetation()
		{
			if (vegetation.Count == 0)
			{
				return;
			}

			vegetation.ForEach(plant =>
			{
				var shouldPrune = rng.NextDouble() < urbanZonePrototype.PruneFactor;
				if (shouldPrune == false)
				{
					plant.Age();
				}
				else
				{
					plant.Prune();
				}
			});
			
			var plantFitness = DeterminePlantFitness();
			var tenPercent = Mathf.CeilToInt(plantFitness.Count * 0.1f);
			var plantsOrderedDescending = (from entry in plantFitness orderby entry.Value descending select entry.Key);
			
			
			var looserPlants = (from entry in plantFitness where entry.Value == 0 select entry.Key);
			foreach (IPlant looserPlant in looserPlants)
			{
				vegetation.Remove(looserPlant);
			}
			
			foreach (IPlant plant in plantsOrderedDescending.Reverse().Take(tenPercent))
			{
				vegetation.Remove(plant);
			}
			
			List<Vector2> occupiedPositions = vegetation.ConvertAll(p => p.Position).SelectMany(p => p).ToList();
			foreach (IPlant plant in plantsOrderedDescending.Take(tenPercent))
			{
				ReproducePlant(plant, ref occupiedPositions);
			}
			
		}

		public List<IPlant> GetVegetation()
		{
			return vegetation;
		}

		private void ReproducePlant(IPlant plant, ref List<Vector2> occupiedPositions)
		{
			int minX = (int) plant.Position[0].x;
			int maxX = (int) plant.Position[0].x;
			int minY = (int) plant.Position[0].y;
			int maxY = (int) plant.Position[0].y;
			
			plant.Position.ForEach(position =>
			{
				int xPos = (int) position.x;
				int yPos = (int) position.y;
				
				if (xPos < minX)
				{
					minX = xPos;
				}

				if (xPos > maxX)
				{
					maxX = xPos;
				}

				if (yPos < minY)
				{
					minY = yPos;
				}

				if (yPos > maxY)
				{
					maxY = yPos;
				}
			});

			Vector2 newPosition;
			for (int x = minX-1; x <= maxX+1; x++)
			{
				for (int y = minY-1; y <= maxY+1; y++)
				{
					int calcX = (int) light.x + x;
					int calcY = (int)light.y + y;
					newPosition = new Vector2(calcX , calcY);
					bool isInUrbanZone = area.Contains(newPosition);
					bool isFree = occupiedPositions.Contains(newPosition) == false;
					if (isInUrbanZone && isFree)
					{
						var newPlant = new Plant(plant.PlantPrototype, newPosition);
						occupiedPositions.Add(newPosition);
						vegetation.Add(newPlant);
						return;
					}
				}
			}
			
		}

		private Dictionary<IPlant, double> DeterminePlantFitness()
		{
			var fitnessForPlant = new Dictionary<IPlant, double>();

			foreach (IPlant plant in vegetation)
			{
				double groundConditionFactor = 0;
				int positionCount = plant.Position.Count;
				float groundCondition = plant.Position.ConvertAll(position => getGroundConditionForPosition.Invoke(position)).Take(positionCount).Sum() / positionCount;
				
				GroundConditionTolerance groundConditionTolerance = plant.PlantPrototype.GroundConditionTolerance;
				if (groundCondition < groundConditionTolerance.Lower || groundCondition > groundConditionTolerance.Upper)
				{
					groundConditionFactor = 0; 
				}
				else
				{
					groundConditionFactor = 1 - Math.Abs(groundConditionTolerance.Ideal - groundCondition);
				}

				if (groundConditionFactor == 0)
				{
					fitnessForPlant.Add(plant, 0f);
					continue;
				}
				
				double fitness = groundConditionFactor * plant.GetAge();
				fitnessForPlant.Add(plant, fitness);
			}
			
			return fitnessForPlant;
		}

		private List<Vector2> GetPlantPositions()
		{
			return handleOfPlacementStrategy[urbanZonePrototype.PlacementStrategy].Invoke();
		}

		private List<Vector2> CalcRegularPositions()
		{
			int stepSize = CalculateRegularStepSize();

			Vector2 firstPosition = area[0];
			CalculateRegularBoundingBox(out int minX, out int maxX, out int minY, out int maxY, firstPosition);
			return CalcRegularPositionsFromBoundingBox(minX, maxX, minY, maxY, stepSize);
		}

		private void CalculateRegularBoundingBox(out int minX, out int maxX, out int minY, out int maxY, Vector2 initializer)
		{
			minX = (int) initializer.x;
			maxX = (int) initializer.x;
			minY = (int) initializer.y;
			maxY = (int) initializer.y; 
			
			foreach (Vector2 position in area)
			{
				if (position.x < minX)
				{
					minX = (int) position.x;
				}
				
				if(position.x > maxX)
				{
					maxX = (int) position.x;
				}

				if (position.y < minY)
				{
					minY = (int) position.y;
				} 
				
				if (position.y > maxY)
				{
					maxY = (int) position.y;
				} 
			}
		}

		private int CalculateRegularStepSize()
		{
			int totalPositions = area.Count;
			double occupiedPositions = totalPositions * urbanZonePrototype.InitialDensity;
			int stepSize = Mathf.CeilToInt(totalPositions / (float)occupiedPositions);
			return stepSize;
		}

		private List<Vector2> CalcRegularPositionsFromBoundingBox(int minX, int maxX, int minY, int maxY, int stepSize)
		{
			var regularPositions = new List<Vector2>();
			int counter = 0;
			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					counter++;
					if (counter % stepSize == 0)
					{
						var pos = new Vector2(x, y);
						if (area.Contains(pos))
						{
							regularPositions.Add(pos);
						}
					}
				}
			}

			return regularPositions;
		}

		private List<Vector2> CalcClusteredPositions()
		{
			var randomClusterMiddles = new List<Vector2>();
			for (int i = 0; i < 5; i++)
			{
				Vector2 position;
				do
				{
					int randomIndex = rng.Next(area.Count);
					position = area[randomIndex];
				} while (randomClusterMiddles.Contains(position));

				randomClusterMiddles.Add(position);
			}

			var clusteredPositions = new List<Vector2>();
			foreach (Vector2 pos in randomClusterMiddles)
			{
				clusteredPositions.Add(pos);
				var neighbours = new List<Vector2>();

				for (int x = -3; x < 3; x++)
				{
					for (int y = -3; y < 3; y++)
					{
						if (x == 0 && y == 0)
						{
							continue;
						}

						var vector2 = new Vector2(pos.x + x, pos.y + y);
						bool canBePlaced = area.Contains(vector2) &&
										   clusteredPositions.Contains(vector2) == false &&
										   rng.NextDouble() < urbanZonePrototype.InitialDensity;

						if (canBePlaced)
						{
							neighbours.Add(vector2);
						}
					}
				}

				clusteredPositions.AddRange(neighbours);
			}

			return clusteredPositions;
		}

		private List<Vector2> CalcRandomPositions()
		{
			return area.FindAll(position =>
			{
				bool canBePlaced = rng.NextDouble() < urbanZonePrototype.InitialDensity;
				return canBePlaced;
			});
		}

		private List<Vector2> CalcEdgePositions()
		{
			var edgePositions = new List<Vector2>();
			foreach (Vector2 position in area)
			{
				int neighbourCount = 0;
				for (int x = -1 ; x <= 1; x++)
				{
					for (int y = -1 ; y <= 1; y++)
					{
						if (x == 0 && y == 0)
						{
							continue;
						}
						var potentialNeighbour = new Vector2(position.x + x, position.y + y);
						if (area.Contains(potentialNeighbour))
						{
							neighbourCount++;
						}
					}
				}

				if (neighbourCount < 8)
				{
					bool canBePlaced = rng.NextDouble() < urbanZonePrototype.InitialDensity;
					if (canBePlaced)
					{
						edgePositions.Add(position);
					}
				}
			}

			return edgePositions;
		}
	}
}