using System.Collections.Generic;
using UrbanVegetation.UrbanZones;

namespace UrbanVegetation.Plants
{
	public interface IPlantPrototype
	{
		PlantClass PlantClass { get; }
		GroundConditionTolerance GroundConditionTolerance { get; }
		List<UrbanZoneClass> AllowedUrbanZones { get; }
		PlantGrowable PlantGrowable { get; }
		PlantDrawable PlantDrawable { get; }
	}
}