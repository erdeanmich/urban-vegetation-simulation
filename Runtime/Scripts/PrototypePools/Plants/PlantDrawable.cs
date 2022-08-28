using UnityEngine;

namespace UrbanVegetation.Plants
{
	public abstract class PlantDrawable : MonoBehaviour
	{
		public abstract void Draw(int age, Vector2 position, Transform parent);
	}
}