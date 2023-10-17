using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewTrustyWeaponsQuest", menuName = "SO/Quests/TrustyWeaponsQuest", order = 1)]
public class TrustyWeaponsQuest : Quest
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
		if (win && MetricsManager.Instance.runMetrics.runStats.nNewTilesPicked == 0)
		{
			QuestCompleted();
		}
	}
}
