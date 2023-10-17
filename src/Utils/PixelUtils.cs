using Parameters;
using UnityEngine;

namespace Utils;

public static class PixelUtils
{
	public static Vector3 PixelPerfectClamp(Vector3 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3((float)(int)(vec.x * (float)TechParams.pixelsPerUnit), (float)(int)(vec.y * (float)TechParams.pixelsPerUnit), (float)(int)(vec.z * (float)TechParams.pixelsPerUnit)) * TechParams.pixelSize;
	}

	public static float PixelPerfectClamp(float x)
	{
		return TechParams.pixelSize * (float)(int)(x * (float)TechParams.pixelsPerUnit);
	}
}
