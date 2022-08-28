namespace UrbanVegetation
{
	public static class ArrayUtils
	{
		public static bool IsIndexOutOfBounds(int index, int arraysize)
		{
			return index < 0 || index > arraysize - 1;
		}
	}
}