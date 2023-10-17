using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BringTileAtMaxLevelQuest", menuName = "SO/Quests/BringTileAtMaxLevelQuest", order = 1)]
public class BringTileAtMaxLevelQuest : Quest
{
	protected override string ProcessDescription(string description)
	{
		return string.Format(description, Attack.maxMaxLevel);
	}

	public override void Initialize()
	{
		EventsManager.Instance.TileUpgraded.AddListener((UnityAction<Tile>)UponTileUpgraded);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.TileUpgraded.RemoveListener((UnityAction<Tile>)UponTileUpgraded);
	}

	private void UponTileUpgraded(Tile tile)
	{
		if (tile.Attack.Level == Attack.maxMaxLevel)
		{
			QuestCompleted();
		}
	}
}
