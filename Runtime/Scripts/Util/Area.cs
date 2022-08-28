using System;
using System.Collections.Generic;
using UnityEngine;

namespace UrbanVegetation.Util
{
	public class Area : IComparable
	{
		private int type;
		private List<Vector2> cells = new List<Vector2>();

		public Area(int type)
		{
			this.type = type;
		}
        
		public void AddCell(int x, int y)
		{
			cells.Add(new Vector2(x, y));
		}

		public int GetSize()
		{
			return cells.Count;
		}

		public int GetAreaType()
		{
			return type;
		}

		public List<Vector2> GetCells()
		{
			return cells;
		}

		public int CompareTo(object obj)
		{
			return GetSize().CompareTo(((Area) obj).GetSize());
		}
	}
}