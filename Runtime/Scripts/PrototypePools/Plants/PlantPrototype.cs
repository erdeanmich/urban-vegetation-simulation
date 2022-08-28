using System;
using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.UrbanZones;

namespace UrbanVegetation.Plants
{
	[Serializable]
	public class PlantPrototype : MonoBehaviour, IPlantPrototype
	{
		[SerializeField]
		private PlantClass plantClass;

		[SerializeField]
		private UrbanZoneClass[] urbanZones;
		
		[SerializeField]
		private GroundConditionTolerance groundConditionTolerance;

		[SerializeField]
		private PlantGrowable plantGrowable;

		[SerializeField]
		private PlantDrawable plantDrawable;

		public PlantClass PlantClass => plantClass;
		public GroundConditionTolerance GroundConditionTolerance => groundConditionTolerance;
		public List<UrbanZoneClass> AllowedUrbanZones => new List<UrbanZoneClass>(urbanZones);
		public PlantGrowable PlantGrowable => plantGrowable;
		public PlantDrawable PlantDrawable => plantDrawable;
	}
}