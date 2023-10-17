using UnityEngine;

public class FinalTutorialPanel : TutorialPanel
{
	[SerializeField]
	private Enemy[] enemies;

	[SerializeField]
	private MyButton trainMoreButton;

	public override bool CanGoToNextPanel => false;

	public override void EnablePanel()
	{
		base.EnablePanel();
		trainMoreButton.Interactable = CombatManager.Instance.Enemies.Count == 0;
	}

	public override void ProcessTurn()
	{
		base.ProcessTurn();
		trainMoreButton.Interactable = CombatManager.Instance.Enemies.Count == 0;
	}

	public void SpawnEnemies()
	{
		new Wave(enemies, 1).Spawn(base.TutorialRoom);
		trainMoreButton.Interactable = false;
	}
}
