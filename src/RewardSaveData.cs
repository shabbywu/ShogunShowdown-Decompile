using System;
using System.Collections.Generic;
using TileEnums;

[Serializable]
public class RewardSaveData
{
	public bool inProgress;

	public bool rewardExausted;

	public List<TileSaveData> tilesRewards;

	public TileUpgradeEnum tileUpgradeEnum;

	public int price;

	public RewardSaveData()
	{
		inProgress = false;
		rewardExausted = false;
		tilesRewards = new List<TileSaveData>();
	}
}
