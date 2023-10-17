using System;
using System.Collections.Generic;
using AgentEnums;

[Serializable]
public class SaveData
{
	public string version;

	public List<CharacterSaveData> characterSaveData;

	public List<ShopSaveData> shopSaveData;

	public UnlocksSaveData unlocksSaveData;

	public HeroEnum lastHeroUsed;

	public int metaCurrency;

	public int nRuns;

	public Options legacyOptions;

	public SaveData()
	{
		version = "";
		characterSaveData = new List<CharacterSaveData>();
		shopSaveData = new List<ShopSaveData>();
		unlocksSaveData = new UnlocksSaveData();
		lastHeroUsed = HeroEnum.wanderer;
		metaCurrency = 0;
		nRuns = 0;
		legacyOptions = new Options();
	}

	public static NamedSaveData GetNamedSaveData(IEnumerable<NamedSaveData> items, string name)
	{
		foreach (NamedSaveData item in items)
		{
			if (item.name == name)
			{
				return item;
			}
		}
		return null;
	}

	public void SetCharacterSaveData(CharacterSaveData value)
	{
		for (int i = 0; i < characterSaveData.Count; i++)
		{
			if (characterSaveData[i].name == value.name)
			{
				characterSaveData[i] = value;
				return;
			}
		}
		characterSaveData.Add(value);
	}

	public void SetShopSaveData(ShopSaveData value)
	{
		for (int i = 0; i < shopSaveData.Count; i++)
		{
			if (shopSaveData[i].name == value.name)
			{
				shopSaveData[i] = value;
				return;
			}
		}
		shopSaveData.Add(value);
	}
}
