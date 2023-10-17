using ProgressionEnums;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IslandClearedQuest", menuName = "SO/Quests/IslandClearedQuest", order = 1)]
public class IslandClearedQuest : Quest
{
	[SerializeField]
	private IslandEnum island;

	[Tooltip("The day on which the island must be cleared. 0 means 'any day'")]
	[SerializeField]
	private int day;

	public override void Initialize()
	{
		EventsManager.Instance.IslandCleared.AddListener((UnityAction<(IslandEnum, int)>)IslandCleared);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.IslandCleared.RemoveListener((UnityAction<(IslandEnum, int)>)IslandCleared);
	}

	private void IslandCleared((IslandEnum island, int day) cleared)
	{
		if (cleared.island == island && (day == 0 || day == cleared.day))
		{
			QuestCompleted();
		}
	}
}
