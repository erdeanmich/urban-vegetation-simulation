using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UrbanVegetation.Plants;
using UrbanVegetation.Simulation.Model;
using UrbanVegetation.UrbanZones;
using UrbanVegetation.Util;
using UrbanVegetation.ScriptableObjects;

namespace UrbanVegetation.Simulation
{
	public class EcosystemSimulation : MonoBehaviour
	{
		[Header("Prototype Pools")]
		[SerializeField]
		private PlantPrototypePool plantPrototypePool;

		[SerializeField]
		private UrbanZonePrototypePool urbanZonePrototypePool;

		[SerializeField]
		private int simulationSteps;

		[SerializeField]
		private Vector2 light;

		private int[,] cityTopology;
		private List<Area> areas;
		private float[,] groundCondition;
		private List<IUrbanZone> urbanZones;

		public void SetCityTopology(int[,] cityTopology)
		{
			this.cityTopology = cityTopology;
			groundCondition = new float[cityTopology.GetLength(0), cityTopology.GetLength(1)];
			areas = new FloodFiller(cityTopology).DetermineAllAreas();

			int lengthX = groundCondition.GetLength(0);
			int lengthY = groundCondition.GetLength(1);
			for (int x = 0; x < lengthX; x++)
			{
				for (int y = 0; y < lengthY; y++)
				{
					groundCondition[x,y] = Mathf.PerlinNoise((float) x / lengthX, (float) y / lengthY);
				}
			}
		}
		
		public void InitSimulation()
		{
			List<IUrbanZonePrototype> urbanZonePrototypes = urbanZonePrototypePool.GetAllUrbanZonePrototypes();
			urbanZones = new List<IUrbanZone>();
			var plantPrototypesForZones = new Dictionary<UrbanZoneClass, List<IPlantPrototype>>();
			urbanZonePrototypes.ForEach(prototype =>
			{
				List<IUrbanZone> identifiedUrbanZones = prototype.Identify(cityTopology, areas);
				urbanZones.AddRange(identifiedUrbanZones);
				UrbanZoneClass urbanZoneClass = prototype.UrbanZoneClass;
				if (plantPrototypesForZones.ContainsKey(urbanZoneClass) == false)
				{
					plantPrototypesForZones[urbanZoneClass] = plantPrototypePool.GetPlantPrototypesForZone(urbanZoneClass);
				}
			});
			
			urbanZones.ForEach(urbanZone =>
			{
				List<IPlantPrototype> plantPrototypesForZone = plantPrototypesForZones[urbanZone.ZoneClass];
				urbanZone.SetInitialVegetation(plantPrototypesForZone, GetGroundConditionForPosition, light);
			});
		}

		public List<IPlant> GetVegetation()
		{
			List<IPlant> plants = urbanZones.ConvertAll(zone => zone.GetVegetation()).SelectMany(x => x).ToList();
			return plants;
		}

		public void Simulate()
		{
			for (int i = 0; i < simulationSteps; i++)
			{
				SimulateStep();
			}
		}

		private void SimulateStep()
		{
			foreach (IUrbanZone urbanZone in urbanZones)
			{
				urbanZone.UpdateVegetation();
			}
		}

		private float GetGroundConditionForPosition(Vector2 position)
		{
			if (position.x < groundCondition.GetLength(0) && position.y < groundCondition.GetLength(1))
			{
				return groundCondition[(int)position.x, (int)position.y];
			}

			return 0f;
		}
	}
	
	
}