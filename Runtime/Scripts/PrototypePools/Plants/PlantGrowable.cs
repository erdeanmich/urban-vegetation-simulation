using UnityEngine;

namespace UrbanVegetation.Plants
{
	public abstract class PlantGrowable : MonoBehaviour
	{
		public abstract void Grow(int age);
	}
}