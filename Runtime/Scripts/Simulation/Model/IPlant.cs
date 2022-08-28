using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.Plants;

namespace UrbanVegetation.Simulation.Model
{
	public interface IPlant
	{
		IPlantPrototype PlantPrototype { get; }
		void Age();
		void Prune();
		List<Vector2> Position { get; }
		void Render(Transform parent);
		int GetAge();
	}
}