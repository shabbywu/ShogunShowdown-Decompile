using System.Collections;
using System.Collections.Generic;
using Parameters;
using UnityEngine;

public class WaveRoom : CombatRoom
{
	public WaveProgressBar waveProgressBar;

	private int nTurnsBeforeNextWave;

	private int iWave;

	public List<Wave> Waves { get; set; }

	public int NWaves => Waves.Count;

	public int WaveNumber => iWave;

	public override string BannerTextBegin => base.Name;

	private bool DebugLog => false;

	private int MaxEnemiesInRoom => (base.Grid.NCells + 1) / 2;

	private bool IsLastWave => iWave >= NWaves - 1;

	public override void Begin()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		base.Begin();
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.combat;
		CombatManager.Instance.BeginCombat();
		Waves = WavesFactory.Instance.GetWaves(base.Id);
		waveProgressBar.Initialize(iWave, NWaves);
		((Component)waveProgressBar).transform.SetParent(CombatSceneManager.Instance.temporaryUI.transform);
		((Component)waveProgressBar).transform.localPosition = 3.225f * Vector3.up;
		if (iWave != -1)
		{
			return;
		}
		SpawnNextWave();
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			enemy.DecideNextAction();
		}
	}

	public override void End()
	{
		Object.Destroy((Object)(object)((Component)waveProgressBar).gameObject);
	}

	public override IEnumerator ProcessTurn()
	{
		if (CombatManager.Instance.Enemies.Count == 1)
		{
			nTurnsBeforeNextWave = Mathf.Min(nTurnsBeforeNextWave, GameParams.maxTurnsBeforeNextWaveWhenOneEnemyLeft);
		}
		bool flag = NextWaveWouldBeTooCrowded();
		nTurnsBeforeNextWave--;
		if (!IsLastWave && !flag && nTurnsBeforeNextWave <= 1)
		{
			waveProgressBar.WaveAboutToSpawn();
		}
		if (IsLastWave)
		{
			if (base.WaveCleared)
			{
				EventsManager.Instance.EndOfCombat.Invoke();
			}
		}
		else if (base.WaveCleared || (nTurnsBeforeNextWave <= 0 && !flag))
		{
			yield return (object)new WaitForSeconds(0.1f);
			SpawnNextWave();
			yield return (object)new WaitForSeconds(0.25f);
		}
	}

	public override void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		base.Initialize(name, id, loadRoomStateFromSaveData);
		nTurnsBeforeNextWave = 0;
		iWave = -1;
	}

	public void SpawnNextWave()
	{
		iWave++;
		Wave wave = Waves[iWave];
		wave.Spawn(this);
		nTurnsBeforeNextWave = wave.Duration;
		waveProgressBar.WaveBegins(iWave);
	}

	private bool NextWaveWouldBeTooCrowded()
	{
		if (IsLastWave)
		{
			return false;
		}
		int num = CombatManager.Instance.Enemies.Count + Waves[iWave + 1].NEnemies;
		if (DebugLog && num > MaxEnemiesInRoom)
		{
			Debug.Log((object)$"Delay spawning because too crowded: nEnemies: {num}. Max allowed: {MaxEnemiesInRoom}");
		}
		return num > MaxEnemiesInRoom;
	}

	public override void PopulateSaveData(RunSaveData runSaveData)
	{
		base.PopulateSaveData(runSaveData);
		runSaveData.combatRoom.iWave = iWave;
		runSaveData.combatRoom.nTurnsBeforeNextWave = nTurnsBeforeNextWave;
	}

	public override void LoadFromSaveData(RunSaveData runSaveData)
	{
		base.LoadFromSaveData(runSaveData);
		iWave = runSaveData.combatRoom.iWave;
		nTurnsBeforeNextWave = runSaveData.combatRoom.nTurnsBeforeNextWave;
	}
}
