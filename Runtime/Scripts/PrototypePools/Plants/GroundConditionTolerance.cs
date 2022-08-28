using System;
using UnityEngine;

namespace UrbanVegetation.Plants
{
	[Serializable]
	public struct GroundConditionTolerance : IGroundConditionTolerance
	{
		[SerializeField]
		[Range(0,1)]
		private double ideal;
		
		[SerializeField]
		[Range(0,1)]
		private double lower;
		
		[SerializeField]
		[Range(0,1)]
		private double upper;

		public double Ideal => ideal;
		public double Upper => upper;
		public double Lower => lower;
	}
}