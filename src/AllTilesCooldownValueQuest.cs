using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewAllTilesCooldownValueQuest", menuName = "SO/Quests/AllTilesCooldownValueQuest", order = 1)]
public class AllTilesCooldownValueQuest : Quest
{
	[SerializeField]
	private int[] cooldownValues;

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
		if (!win)
		{
			return;
		}
		foreach (Tile item in TilesManager.Instance.Deck)
		{
			if (!cooldownValues.Contains(item.Attack.Cooldown))
			{
				return;
			}
		}
		QuestCompleted();
	}
}
