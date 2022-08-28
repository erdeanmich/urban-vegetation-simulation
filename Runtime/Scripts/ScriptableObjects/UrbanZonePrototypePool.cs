using System;
using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.UrbanZones;

namespace UrbanVegetation.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Tools", menuName = "ScriptableObjects/UrbanZonePrototypePool", order = 1)]
	public class UrbanZonePrototypePool : ScriptableObject
	{
		[SerializeField]
		private GameObject[] urbanZonePrototypesPrefabs;

		public List<IUrbanZonePrototype> GetAllUrbanZonePrototypes()
		{
			var list = new List<IUrbanZonePrototype>();
			foreach (var prefab in urbanZonePrototypesPrefabs)
			{
				var urbanZonePrototype = prefab.GetComponent<UrbanZonePrototype>();
				if (urbanZonePrototype == null)
				{
					throw new Exception($"Prefab {prefab.name} has no UrbanZonePrototype component on it");
				}
				list.Add(urbanZonePrototype);
			}

			return list;
		}
	}
}