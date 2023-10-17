using System;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;

namespace TilesUtils;

internal static class TileSorting
{
	public static Tile[] SortByCombat(Tile[] unsorted)
	{
		List<Tile> list = new List<Tile>();
		foreach (AttackEnum value in Enum.GetValues(typeof(AttackEnum)))
		{
			foreach (Tile tile in unsorted)
			{
				if (tile.Attack.AttackEnum == value)
				{
					list.Add(tile);
				}
			}
		}
		return list.ToArray();
	}

	public static Tile[] SortByTurnsBeforeCharged(Tile[] unsorted)
	{
		List<Tile> list = new List<Tile>();
		int num = 0;
		Tile[] array = unsorted;
		foreach (Tile tile in array)
		{
			num = Mathf.Max(num, tile.TurnsBeforeCharged);
		}
		for (int j = 0; j <= num; j++)
		{
			array = unsorted;
			foreach (Tile tile2 in array)
			{
				if (j == tile2.TurnsBeforeCharged)
				{
					list.Add(tile2);
				}
			}
		}
		return list.ToArray();
	}
}
