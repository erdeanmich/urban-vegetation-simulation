using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UrbanVegetation.Plants;

namespace UrbanVegetation.Simulation.Model
{
	class Plant : IPlant
	{
		public IPlantPrototype PlantPrototype { get; }

		public List<Vector2> Position { get; }

		private int age; 

		public Plant(IPlantPrototype plantPrototype, Vector2 position)
		{
			Position = new List<Vector2> { position };
			PlantPrototype = plantPrototype;
			age = 0;
		}

		public void Age()
		{
			age++;
		}
		
		public void Prune()
		{
			age--;
		}

		public void Render(Transform parent)
		{
			PlantPrototype.PlantDrawable.Draw(age, Position.First(), parent);
		}

		public int GetAge()
		{
			return age;
		}
	}
}