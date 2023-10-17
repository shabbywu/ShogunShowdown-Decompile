using UnityEngine;

public abstract class Reward : MonoBehaviour
{
	protected RewardSaveData rewardSaveData;

	public bool InProgress { get; set; }

	public bool Exausted { get; set; }

	public abstract bool Rerollable { get; }

	public bool Rerolled { get; set; }

	public bool IsPayedReward { get; set; }

	public abstract void StartEvent(RewardSaveData rewardSaveData);

	public abstract void EndEvent();

	public abstract void Initialize();

	public virtual void FinalizeMe()
	{
		((Component)this).gameObject.SetActive(false);
	}

	public virtual void Reroll()
	{
	}

	public virtual void Skipped()
	{
		Exausted = true;
	}

	public abstract void PopulateSaveData(RewardSaveData rewardSaveData);
}
