using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewSniperQuest", menuName = "SO/Quests/SniperQuest", order = 1)]
public class SniperQuest : Quest
{
	private static int distanceForSniperKill = 8;

	public override void Initialize()
	{
		EventsManager.Instance.EnemyDied.AddListener((UnityAction<Enemy>)EnemyDied);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.EnemyDied.RemoveListener((UnityAction<Enemy>)EnemyDied);
	}

	private void EnemyDied(Enemy enemy)
	{
		if ((Object)(object)enemy.LastAttacker == (Object)(object)Globals.Hero && Cell.Distance(enemy.Cell, Globals.Hero.Cell) == distanceForSniperKill)
		{
			QuestCompleted();
		}
	}
}
