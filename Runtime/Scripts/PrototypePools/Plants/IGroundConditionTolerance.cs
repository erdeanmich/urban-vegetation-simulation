namespace UrbanVegetation.Plants
{
	public interface IGroundConditionTolerance
	{
		double Ideal { get; }
		double Upper { get; }
		double Lower { get; }
	}
}