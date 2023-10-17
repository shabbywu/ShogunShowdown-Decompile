using System;
using ShopStuff;
using UnityEngine;

[Serializable]
public class ShopSaveData : NamedSaveData
{
	public ShopItemTypeEnum[] shopItemTypes;

	public int[] nSlotsPerType;

	public int numberOfTimesUpgraded;

	public ShopSaveData(string name, ShopItemTypeEnum[] shopItemTypes, int[] slotsPerType)
	{
		base.name = name;
		this.shopItemTypes = shopItemTypes;
		nSlotsPerType = slotsPerType;
		numberOfTimesUpgraded = 0;
	}

	public void Log()
	{
		for (int i = 0; i < shopItemTypes.Length; i++)
		{
			Debug.Log((object)$"   {shopItemTypes[i]}: {nSlotsPerType[i]}");
		}
	}

	public int NSlotsPerType(ShopItemTypeEnum type)
	{
		for (int i = 0; i < shopItemTypes.Length; i++)
		{
			if (shopItemTypes[i] == type)
			{
				return nSlotsPerType[i];
			}
		}
		return 0;
	}
}
