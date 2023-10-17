using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewFrugalFighterQuest", menuName = "SO/Quests/FrugalFighterQuest", order = 1)]
public class FrugalFighterQuest : Quest
{
	public override void Initialize()
	{
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.GameOver.RemoveListener((UnityAction<bool>)GameOver);
	}

	private void GameOver(bool win)
	{
		if (win && MetricsManager.Instance.runMetrics.runStats.nConsumablesUsed == 0)
		{
			QuestCompleted();
		}
	}
}
