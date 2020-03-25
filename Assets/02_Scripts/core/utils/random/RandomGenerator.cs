using UnityEngine;

public class RandomGenerator
{
	private static System.Random randomGenerator = null;

	public static float GetRandom(float min, float max)
	{
		return Random.Range(min, max);
	}

	public static int GetRandom(int min, int max)
	{
		return Random.Range(min, max);
	}

	public static System.Random GetRandomGenerator()
	{
		if (randomGenerator == null)
		{
			randomGenerator = new System.Random();
		}

		return randomGenerator;
	}

	public static int NextProbability()
	{
		return Next(0, 100);
	}

	public static int Next(int minValue, int maxValue)
	{
		return GetRandomGenerator().Next(minValue, maxValue);
	}
}