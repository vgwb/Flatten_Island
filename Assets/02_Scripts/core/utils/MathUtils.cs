using UnityEngine;

public class MathUtils
{
	public static float EPSILON = 0.001f;

	public static float Map01(float value, float min, float max)
	{
		float clampedValue = Mathf.Clamp(value, min, max);

		if (max == min)
		{
			return 1.0f;
		}

		return (clampedValue - min) / (max - min);
	}

	public static float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
	{
		float fromAbs = from - fromMin;
		float fromMaxAbs = fromMax - fromMin;

		float normal = fromAbs / fromMaxAbs;

		float toMaxAbs = toMax - toMin;
		float toAbs = toMaxAbs * normal;

		float to = toAbs + toMin;

		return to;
	}

	public static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
	{
		bool equal = true;

		if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
		if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
		if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

		return equal;
	}

	public static bool AlmostEqual(float f1, float f2, float precision)
	{
		if (Mathf.Abs(f1 - f2) > precision)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public static Vector3 GetPerpendicularNormal(Vector3 normal)
	{
		if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
		{
			return new Vector3(Mathf.Sign(normal.x), 0.0f, 0.0f);
		}
		else
		{
			return new Vector3(0.0f, Mathf.Sign(normal.y), 0.0f);
		}
	}

	public static bool IsInRange(float number, float bottom, float top)
	{
		return (number >= bottom && number <= top);
	}

	public static bool IsInRange(float number, Vector2 range)
	{
		return (number >= range.x && number <= range.y);
	}
}