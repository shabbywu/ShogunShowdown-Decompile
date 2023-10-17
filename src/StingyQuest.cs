using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewStingyQuest", menuName = "SO/Quests/StingyQuest", order = 1)]
public class StingyQuest : Quest
{
	[SerializeField]
	private int nCoins;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, nCoins);
	}

	public override void Initialize()
	{
		((UnityEvent<int>)EventsManager.Instance.CoinsUpdate).AddListener((UnityAction<int>)CoinsUpdate);
	}

	public override void FinalizeQuest()
	{
		((UnityEvent<int>)EventsManager.Instance.CoinsUpdate).RemoveListener((UnityAction<int>)CoinsUpdate);
	}

	private void CoinsUpdate(int value)
	{
		if (value >= nCoins)
		{
			QuestCompleted();
		}
	}
}
