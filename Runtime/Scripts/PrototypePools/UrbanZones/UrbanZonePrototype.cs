using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.Simulation.Model;
using UrbanVegetation.Util;

namespace UrbanVegetation.UrbanZones
{
	public class UrbanZonePrototype : MonoBehaviour, IUrbanZonePrototype
	{
		[SerializeField]
		private UrbanZoneClass urbanZoneClass;
		
		[SerializeField]
		[Range(0,1)]
		private double pruneFactor;
		
		[SerializeField]
		[Range(0,1)]
		private double initialDensity;
		
		[SerializeField]
		private UrbanZonePlacementStrategy placementStrategy;

		[SerializeField]
		private UrbanZoneIdentifiable urbanZoneIdentifiable;

		public UrbanZoneClass UrbanZoneClass => urbanZoneClass;
		public double PruneFactor => pruneFactor;
		public double InitialDensity => initialDensity;
		public UrbanZonePlacementStrategy PlacementStrategy => placementStrategy;
		public List<IUrbanZone> Identify(int[,] cityTopology, List<Area> areasInCityTopology)
		{
			return urbanZoneIdentifiable.Identify(cityTopology, this, areasInCityTopology);
		}
	}
}