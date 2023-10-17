using System;

[Serializable]
public class RewardRoomSaveData
{
	public int rerollPrice;

	public RewardSaveData reward;

	public RewardRoomSaveData()
	{
		rerollPrice = -1;
		reward = new RewardSaveData();
	}
}
