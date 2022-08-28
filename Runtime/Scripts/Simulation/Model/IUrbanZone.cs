using System;
using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.Plants;
using UrbanVegetation.UrbanZones;

namespace UrbanVegetation.Simulation.Model
{
	public interface IUrbanZone
	{
		UrbanZoneClass ZoneClass { get; }
		void SetInitialVegetation(List<IPlantPrototype> plantPrototypes, Func<Vector2, float> getGroundConditionForPosition, Vector2 light);
		void UpdateVegetation();
		List<IPlant> GetVegetation();
	}
}