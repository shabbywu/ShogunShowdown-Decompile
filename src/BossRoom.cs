using System.Collections;
using UnityEngine;
using Utils;

public class BossRoom : CombatRoom
{
	public Boss bossPrefab;

	public BossHealthBar bossHealthBar;

	[SerializeField]
	private Cell bossInitialCell;

	private Boss boss;

	private int bossInitialDistance = 2;

	public Boss Boss => boss;

	public override string BannerTextBegin => Globals.Hero.Name + "\nv.s.\n" + boss.Name;

	public override void Begin()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.Begin();
		if ((Object)(object)boss != (Object)null)
		{
			((Component)bossHealthBar).transform.SetParent(CombatSceneManager.Instance.temporaryUI.transform);
			((Component)bossHealthBar).transform.localPosition = Vector3.zero;
			bossHealthBar.Name = boss.Name;
			((Component)bossHealthBar).gameObject.SetActive(true);
		}
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.combat;
		CombatManager.Instance.BeginCombat();
		EventsManager.Instance.BeginBossFight.Invoke();
	}

	public override void End()
	{
		Object.Destroy((Object)(object)((Component)bossHealthBar).gameObject);
		EventsManager.Instance.EndBossFight.Invoke();
		SaveDataManager.Instance.StoreSaveData();
	}

	public void SetBoss(Boss boss)
	{
		this.boss = boss;
	}

	public override IEnumerator ProcessTurn()
	{
		if ((Object)(object)boss != (Object)null)
		{
			yield return boss.ProcessTurn();
		}
		if (((Object)(object)boss == (Object)null || !boss.IsAlive) && Globals.Hero.IsAlive)
		{
			yield return (object)new WaitForSeconds(1f);
			EventsManager.Instance.EndOfCombat.Invoke();
		}
		else
		{
			yield return null;
		}
	}

	public void BossDied()
	{
		while (CombatManager.Instance.Enemies.Count > 0)
		{
			CombatManager.Instance.Enemies[0].CommitSeppuku();
		}
		MusicManager.Instance.Stop();
	}

	public override void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		base.Initialize(name, id, loadRoomStateFromSaveData);
		if (!loadRoomStateFromSaveData)
		{
			InstantiateBoss();
		}
	}

	private void InstantiateBoss()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)bossInitialCell == (Object)null)
		{
			bossInitialCell = base.Grid.initialPlayerCell.Neighbour(Dir.right, bossInitialDistance);
		}
		GameObject val = Object.Instantiate<GameObject>(((Component)bossPrefab).gameObject, ((Component)this).transform);
		boss = val.GetComponent<Boss>();
		((Component)boss).transform.position = ((Component)bossInitialCell).transform.position;
		boss.Cell = bossInitialCell;
		boss.FacingDir = Dir.left;
		CombatManager.Instance.Enemies.Add(boss);
		boss.FirstTimeBossFightInitializations(this);
	}

	public override void LoadFromSaveData(RunSaveData runSaveData)
	{
		base.LoadFromSaveData(runSaveData);
		boss = (Boss)CombatManager.Instance.Enemies.Find((Enemy enemy) => enemy is Boss);
	}
}
