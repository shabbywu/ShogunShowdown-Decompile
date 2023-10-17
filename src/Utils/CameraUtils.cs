using UnityEngine;

namespace Utils;

internal static class CameraUtils
{
	public static Vector3 CenterOfScreen()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.Scale(((Component)Camera.main).transform.position, new Vector3(1f, 1f, 0f));
	}
}
