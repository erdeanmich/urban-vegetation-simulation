using System.Collections.Generic;
using System.Linq;

namespace UrbanVegetation.Util
{
	public class FloodFiller
	{
		private readonly int[,] cityMap;

		public FloodFiller(int[,] cityMap)
		{
			this.cityMap = cityMap;
		}
		
		public List<Area> DetermineAllAreas()
        {
            // have a floodarray
            int[,] floodedCells = new int[cityMap.GetLength(0), cityMap.GetLength(1)];
            int floodColor = 1;
            var areasByFloodColor = new Dictionary<int, Area>();

            // loop through all cells 
            for (int x = 0; x < cityMap.GetLength(0); x++)
            {
                for (int y = 0; y < cityMap.GetLength(1); y++)
                {
					FloodCell(floodedCells, x, y, floodColor, areasByFloodColor, cityMap[x, y]);
					floodColor++;
				}
            }

            return areasByFloodColor.Values.ToList();
        }

        private void FloodCell(int[,] floodedCells, int x, int y, int floodColor, Dictionary<int, Area> areasByFloodColor, int cellType)
        {
            int cellLengthX = floodedCells.GetLength(0);
            int cellLengthY = floodedCells.GetLength(1);
            if (ArrayUtils.IsIndexOutOfBounds(x, cellLengthX) || ArrayUtils.IsIndexOutOfBounds(y, cellLengthY))
            {
                // no valid cell index
                return;
            }
            
            if (floodedCells[x, y] != 0)
            {
                // cell was already visited
                return;
            }

            if (cityMap[x, y] != cellType)
            {
                // stop flooding, this is not the cellType we are currently flooding
                return;
            }

            floodedCells[x, y] = floodColor;
            if (!areasByFloodColor.ContainsKey(floodColor))
            {
                areasByFloodColor[floodColor] = new Area(cellType);
            }
            
            areasByFloodColor[floodColor].AddCell(x,y);
            
            FloodCell(floodedCells, x + 1, y, floodColor, areasByFloodColor, cellType);
            FloodCell(floodedCells, x - 1, y, floodColor, areasByFloodColor, cellType);
            FloodCell(floodedCells, x, y + 1, floodColor, areasByFloodColor, cellType);
            FloodCell(floodedCells, x, y - 1, floodColor, areasByFloodColor, cellType);
        }
	}
}