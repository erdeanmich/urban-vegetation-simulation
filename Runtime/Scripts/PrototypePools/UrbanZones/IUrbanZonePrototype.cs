using System.Collections.Generic;
using UrbanVegetation.Simulation.Model;
using UrbanVegetation.Util;

namespace UrbanVegetation.UrbanZones
{
	public interface IUrbanZonePrototype
	{
		UrbanZoneClass UrbanZoneClass { get; }
		double PruneFactor { get; }
		double InitialDensity { get; }
		UrbanZonePlacementStrategy PlacementStrategy { get; }
		List<IUrbanZone> Identify(int[,] cityTopology, List<Area> areasInCityTopology);
	}
}