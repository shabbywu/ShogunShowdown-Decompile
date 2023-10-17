using UnityEngine;

namespace Utils;

public static class DirUtils
{
	public static Vector3 ToVec(Dir dir)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		switch (dir)
		{
		case Dir.left:
			return Vector3.left;
		case Dir.right:
			return Vector3.right;
		case Dir.up:
			return Vector3.up;
		case Dir.down:
			return Vector3.down;
		default:
			Debug.Log((object)"DirUtils.DirToVec: should not get here...");
			return Vector3.zero;
		}
	}

	public static Dir Opposite(Dir dir)
	{
		switch (dir)
		{
		case Dir.left:
			return Dir.right;
		case Dir.right:
			return Dir.left;
		case Dir.up:
			return Dir.down;
		case Dir.down:
			return Dir.up;
		default:
			Debug.Log((object)"DirUtils.Opposite: should not get here...");
			return Dir.left;
		}
	}
}
