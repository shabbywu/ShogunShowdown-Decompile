using Parameters;
using UnityEngine;
using Utils;

public abstract class CombatRoom : Room
{
	protected bool wasLoadedFromSaveData;

	public override string BannerTextEnd
	{
		get
		{
			string text = "";
			text = ((MetricsManager.Instance.currentRoomMetrics.damageTaken == 0) ? "Obliterated" : ((MetricsManager.Instance.currentRoomMetrics.damageTaken > GameParams.roomClearedThreshold) ? "Survived" : "Cleared"));
			return LocalizationUtils.LocalizedString("Terms", text);
		}
	}

	protected bool WaveCleared => CombatManager.Instance.Enemies.Count == 0;

	public override void Begin()
	{
		CombatManager.Instance.AllowTileInteraction = true;
	}

	public override void PopulateSaveData(RunSaveData runSaveData)
	{
		base.PopulateSaveData(runSaveData);
		runSaveData.combatRoom = new CombatRoomSaveData();
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			runSaveData.combatRoom.enemies.Add(enemy.GetEnemyCombatSaveData());
		}
	}

	public override void LoadFromSaveData(RunSaveData runSaveData)
	{
		base.LoadFromSaveData(runSaveData);
		wasLoadedFromSaveData = true;
		foreach (EnemyCombatSaveData enemy2 in runSaveData.combatRoom.enemies)
		{
			Enemy enemy = AgentsFactory.Instance.InstantiateEnemy(enemy2.enemy, ((Component)this).transform);
			AddEnemy(enemy, base.Grid.Cells[enemy2.iCell]);
			enemy.LoadFromSaveData(enemy2);
		}
	}
}
