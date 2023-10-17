using UnityEngine;

namespace Utils;

internal static class MyMath
{
	public static Vector2 Rotate(Vector2 v, float angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle), v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle));
	}

	public static int ModularizeIndex(int i, int lenght)
	{
		if (i >= 0 && i < lenght)
		{
			return i;
		}
		if (i >= lenght)
		{
			return i % lenght;
		}
		if (i < 0)
		{
			return lenght + i % lenght;
		}
		Debug.LogError((object)"I should not get here...");
		return 0;
	}
}
