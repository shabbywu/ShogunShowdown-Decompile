using System;
using System.Collections.Generic;

[Serializable]
public class ShopInRunSaveData
{
	public List<string> shopItemDataNames;

	public List<bool> onSale;

	public bool alreadyUpgraded;

	public bool freeConsumableAlreadyGiven;

	public bool shouldGiveFreeConsumable;

	public ShopInRunSaveData()
	{
		shopItemDataNames = new List<string>();
		onSale = new List<bool>();
	}
}
