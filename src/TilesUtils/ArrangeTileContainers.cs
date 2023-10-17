using UnityEngine;

namespace TilesUtils;

internal static class ArrangeTileContainers
{
	public static void SideBySide(TileContainer[] ccs, Vector3 position, float deltaX, Vector3 up)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ccs.Length; i++)
		{
			((Component)ccs[i]).transform.position = position + (float)(ccs.Length - 1 - i) * deltaX * up;
		}
	}

	public static void OnlyFullSideBySideCentered(TileContainer[] ccs, Vector3 position, float deltaX, Vector3 up)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < ccs.Length; i++)
		{
			if (ccs[i].HasTile)
			{
				num++;
			}
		}
		for (int j = 0; j < num; j++)
		{
			((Component)ccs[j]).transform.position = position + (float)(num - 1 - j) * deltaX * up;
			Transform transform = ((Component)ccs[j]).transform;
			transform.position -= (float)(num - 1) * deltaX * up / 2f;
		}
	}

	public static void SideBySideMultiline(TileContainer[] cc, Vector3 position, int nColumns, float deltaX, float deltaY)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		int num = (cc.Length + nColumns - 1) / nColumns;
		int num2 = 0;
		Vector3 val = (float)(nColumns - 1) * deltaX * Vector3.left / 2f + (float)(num - 1) * deltaY * Vector3.up / 2f;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < nColumns; j++)
			{
				if (num2 > cc.Length - 1)
				{
					return;
				}
				((Component)cc[num2]).transform.localPosition = position + (float)j * deltaX * Vector3.right + (float)i * deltaY * Vector3.down + val;
				num2++;
			}
		}
	}
}
