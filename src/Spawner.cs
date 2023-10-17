using UnityEngine;
using Utils;

public class Spawner : MonoBehaviour
{
	private Enemy enemy;

	public static Enemy SpawnEnemy(Cell cell, GameObject enemyPrefab)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Spawner component = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Combat/Spawner"), ((Component)cell).transform.position, Quaternion.identity).GetComponent<Spawner>();
		component.Spawn(cell, enemyPrefab);
		return component.enemy;
	}

	public void Spawn(Cell cell, GameObject enemyPrefab)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		enemy = Object.Instantiate<GameObject>(enemyPrefab, ((Component)cell).transform.position, Quaternion.identity).GetComponent<Enemy>();
		CombatSceneManager.Instance.Room.AddEnemy(enemy, cell);
		enemy.FacingDir = MyRandom.NextFromArray(new Dir[2]
		{
			Dir.left,
			Dir.right
		});
		enemy.SetVisible(value: false);
	}

	public void EnemyAppears()
	{
		enemy.SetVisible(value: true);
		enemy.ShowCombatUI();
		enemy.EnterCombatMode();
	}
}
