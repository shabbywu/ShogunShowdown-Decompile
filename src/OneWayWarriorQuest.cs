using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewOneWayWarriorQuest", menuName = "SO/Quests/OneWayWarriorQuest", order = 1)]
public class OneWayWarriorQuest : Quest
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
		if (win && MetricsManager.Instance.runMetrics.runStats.nTurnArounds == 0)
		{
			QuestCompleted();
		}
	}
}
