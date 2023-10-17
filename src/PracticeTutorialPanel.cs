using UnityEngine;

public class PracticeTutorialPanel : TutorialPanel
{
	[SerializeField]
	private Enemy[] enemies;

	private bool enemiesAlreadySpawned;

	private bool HasActiveEnemies => CombatManager.Instance.Enemies.Count > 0;

	public override bool IsVolatilePanel { get; } = true;


	public override bool CanGoToNextPanel => !HasActiveEnemies;

	public override void EnablePanel()
	{
		base.EnablePanel();
		if (enemiesAlreadySpawned && !HasActiveEnemies)
		{
			base.TutorialRoom.NextPanel();
		}
		else if (!enemiesAlreadySpawned)
		{
			SpawnEnemies();
		}
	}

	public override void ProcessTurn()
	{
		base.ProcessTurn();
		if (!HasActiveEnemies)
		{
			base.TutorialRoom.NextPanel();
		}
	}

	private void SpawnEnemies()
	{
		Cell cell = base.TutorialRoom.Grid.LeftmostFreeCell();
		Cell cell2 = base.TutorialRoom.Grid.RightmostFreeCell();
		if (enemies.Length == 1)
		{
			Spawner.SpawnEnemy((Globals.Hero.Cell.Distance(cell) > Globals.Hero.Cell.Distance(cell2)) ? cell : cell2, ((Component)enemies[0]).gameObject).Summoned = true;
		}
		else if (enemies.Length == 2)
		{
			Spawner.SpawnEnemy(cell, ((Component)enemies[0]).gameObject).Summoned = true;
			Spawner.SpawnEnemy(cell2, ((Component)enemies[1]).gameObject).Summoned = true;
		}
		else
		{
			Debug.LogError((object)"Tutorial room spawning: more than 2 enemies not implemented!");
		}
		EffectsManager.Instance.GenericSpawningEffect();
		enemiesAlreadySpawned = true;
	}
}
