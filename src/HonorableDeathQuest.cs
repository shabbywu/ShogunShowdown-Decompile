using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewHonorableDeathQuest", menuName = "SO/Quests/HonorableDeathQuest", order = 1)]
public class HonorableDeathQuest : Quest
{
	public override void Initialize()
	{
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
		EventsManager.Instance.BossDied.AddListener((UnityAction<Boss>)BossDied);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.GameOver.RemoveListener((UnityAction<bool>)GameOver);
		EventsManager.Instance.BossDied.RemoveListener((UnityAction<Boss>)BossDied);
	}

	private void GameOver(bool win)
	{
		if (!win && CombatSceneManager.Instance.Room is BossRoom bossRoom && !bossRoom.Boss.IsAlive)
		{
			QuestCompleted();
		}
	}

	private void BossDied(Boss boss)
	{
		if (!Globals.Hero.IsAlive)
		{
			QuestCompleted();
		}
	}
}
