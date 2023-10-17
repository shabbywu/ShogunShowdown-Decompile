using System;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using UnityEngine;
using Utils;

namespace ShopStuff;

[Serializable]
public class ShopItemPool
{
	public enum SelectionMode
	{
		random,
		sequential
	}

	public ShopItemTypeEnum type;

	public SelectionMode selectionMode;

	public int nSlotsInitial;

	public int nSlotsMax;

	public ShopItemData[] items;

	public ShopItemData[] GetNextNItems(int n, ShopItemData[] itemsToAvoid = null, ShopItemData[] itemsToPrefereblyAvoid = null)
	{
		ShopItemData[] array = items.Where((ShopItemData item) => item.CanBeSold).ToArray();
		if (itemsToAvoid != null)
		{
			array = array.Where((ShopItemData item) => !itemsToAvoid.Contains(item)).ToArray();
		}
		if (selectionMode == SelectionMode.random)
		{
			float[] array2 = array.Select((ShopItemData item) => item.Probability).ToArray();
			if (itemsToPrefereblyAvoid != null)
			{
				foreach (ShopItemData value in itemsToPrefereblyAvoid)
				{
					int num = Array.IndexOf(array, value);
					if (num >= 0)
					{
						array2[num] /= 100f;
					}
				}
			}
			return MyRandom.NextNFromArrayNoRepetition(array, Mathf.Min(n, array.Length), array2);
		}
		List<ShopItemData> list = new List<ShopItemData>();
		for (int j = 0; j < Math.Min(n, array.Length); j++)
		{
			list.Add(array[j]);
		}
		return list.ToArray();
	}

	public void Initialize()
	{
		if (type == ShopItemTypeEnum.unlock)
		{
			for (int i = 0; i < items.Length; i++)
			{
				items[i].price = GameParams.UnlocksMetacurrencyPrice(i);
			}
		}
	}
}
