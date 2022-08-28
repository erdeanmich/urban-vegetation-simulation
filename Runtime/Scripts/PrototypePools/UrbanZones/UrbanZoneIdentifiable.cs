using System.Collections.Generic;
using UnityEngine;
using UrbanVegetation.Simulation.Model;
using UrbanVegetation.Util;

namespace UrbanVegetation.UrbanZones
{
	public abstract class UrbanZoneIdentifiable : MonoBehaviour
	{
		public abstract List<IUrbanZone> Identify(int[,] cityTopology, UrbanZonePrototype urbanZonePrototype, List<Area> areas);
	}
}