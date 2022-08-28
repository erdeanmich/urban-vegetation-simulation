using System;
using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.Plants;
using UrbanVegetation.UrbanZones;

namespace UrbanVegetation.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Tools", menuName = "ScriptableObjects/PlantPrototypePool", order = 1)]
	[Serializable]
	public class PlantPrototypePool : ScriptableObject
	{
		[SerializeField]
		private GameObject[] plantPrototypePrefabs;

		public List<IPlantPrototype> GetPlantPrototypesForZone(UrbanZoneClass urbanZoneClass)
		{
			List<IPlantPrototype> plantPrototypes = new List<IPlantPrototype>();
			foreach (GameObject plantPrototypePrefab in plantPrototypePrefabs)
			{
				IPlantPrototype plantPrototype = plantPrototypePrefab.GetComponent<PlantPrototype>();
				if (plantPrototype == null)
				{
					throw new Exception($"No PlantPrototype component found on {plantPrototypePrefab.name} prefab");
				}

				if (plantPrototype.AllowedUrbanZones.Contains(urbanZoneClass))
				{
					plantPrototypes.Add(plantPrototype);
				}
			}

			return plantPrototypes;
		}
	}
}