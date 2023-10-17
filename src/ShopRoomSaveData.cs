using System;

[Serializable]
public class ShopRoomSaveData
{
	public RewardSaveData reward;

	public ShopInRunSaveData shopInRun;

	public string leftShopComponentName;

	public string rightShopComponentName;

	public ShopRoomSaveData()
	{
		shopInRun = new ShopInRunSaveData();
		reward = new RewardSaveData();
	}
}
