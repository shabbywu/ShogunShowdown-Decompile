using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombaUtilsNameSpace;
using CombatEnums;
using Parameters;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class CombatManager : MonoBehaviour
{
	private bool heroPlayedTileInThisUpdate;

	private Room room;

	public static CombatManager Instance { get; private set; }

	public bool CombatInProgress { get; set; }

	public bool AllowHeroAction { get; set; } = true;


	public bool AllowTileInteraction { get; set; } = true;


	public int KillStreak { get; private set; }

	public bool TurnInProgress { get; private set; }

	public List<ICombatTask> EndOfTurnTasks { get; private set; } = new List<ICombatTask>();


	public List<Enemy> Enemies { get; set; } = new List<Enemy>();


	public List<Agent> Agents
	{
		get
		{
			List<Agent> list = new List<Agent>();
			list.Add(Globals.Hero);
			list.AddRange(Enemies);
			return list;
		}
	}

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(OnEndOfCombat));
		EventsManager.Instance.EnemyDied.AddListener((UnityAction<Enemy>)OnEnemyDied);
		EventsManager.Instance.PreciseKill.AddListener((UnityAction<Agent>)OnPreciseKill);
	}

	public void HeroPlayedTile(Tile tile)
	{
		if (tile.Attack.TileEffect != TileEffectEnum.freePlay)
		{
			heroPlayedTileInThisUpdate = true;
		}
	}

	private void Update()
	{
		if (CombatInProgress && !TurnInProgress)
		{
			bool flag = false;
			ActionEnum heroAction = ActionEnum.wait;
			if (Globals.Hero.AgentStats.ice > 0)
			{
				flag = true;
				heroAction = ActionEnum.wait;
			}
			else if (heroPlayedTileInThisUpdate)
			{
				flag = true;
				heroAction = ActionEnum.playTile;
			}
			if (flag && HeroActionAllowed(heroAction))
			{
				TriggerTurn(heroAction);
			}
		}
		heroPlayedTileInThisUpdate = false;
	}

	public void TriggerTurn(ActionEnum heroAction)
	{
		Globals.Hero.Action = heroAction;
		((MonoBehaviour)this).StartCoroutine(ProcessTurn());
	}

	private IEnumerator ProcessTurn()
	{
		EventsManager.Instance.BeginningOfCombatTurn.Invoke();
		KillStreak = 0;
		TurnInProgress = true;
		foreach (Tile item in TilesManager.Instance.Deck)
		{
			item.OnBeginningOfTurn();
		}
		foreach (Agent agent in Agents)
		{
			agent.OnBeginningOfTurn();
		}
		if (Globals.Hero.AgentStats.ice > 0)
		{
			yield return (object)new WaitForSeconds(0.75f);
		}
		if (Globals.Hero.Action == ActionEnum.wait)
		{
			Globals.Hero.Wait();
			yield return (object)new WaitForSeconds(0.05f);
		}
		if (Globals.Hero.Action == ActionEnum.attack)
		{
			Globals.Hero.ExecuteAttacksInQueue();
			while (AgentsActionInProgress())
			{
				yield return null;
			}
		}
		if (Globals.Hero.ActionIsMove)
		{
			yield return Globals.Hero.PerformMoveAction(Globals.Hero.MoveDir);
		}
		if (Globals.Hero.Action == ActionEnum.flipLeft)
		{
			Globals.Hero.FlipLeft();
		}
		if (Globals.Hero.Action == ActionEnum.flipRight)
		{
			Globals.Hero.FlipRight();
		}
		if (Globals.Hero.Action == ActionEnum.playTile)
		{
			yield return (object)new WaitForSeconds(0.05f);
		}
		foreach (Enemy enemy in Enemies)
		{
			if (enemy.Action == ActionEnum.flipLeft)
			{
				enemy.FlipLeft();
			}
			if (enemy.Action == ActionEnum.flipRight)
			{
				enemy.FlipRight();
			}
			if (enemy.ActionIsMove)
			{
				enemy.Move(enemy.MoveDir);
			}
		}
		while (AgentsActionInProgress())
		{
			yield return null;
		}
		List<Enemy> list = new List<Enemy>(Enemies);
		foreach (Enemy item2 in list)
		{
			if (Enemies.Contains(item2) && item2.Action == ActionEnum.attack)
			{
				item2.ExecuteAttacksInQueue();
				while (AgentsActionInProgress())
				{
					yield return null;
				}
			}
		}
		foreach (Enemy enemy2 in Enemies)
		{
			if (enemy2.Action == ActionEnum.playTile)
			{
				enemy2.PlayTileAction();
			}
		}
		while (AgentsActionInProgress())
		{
			yield return null;
		}
		foreach (Agent agent2 in Agents)
		{
			agent2.EndOfTurn();
		}
		while (AgentsActionInProgress())
		{
			yield return null;
		}
		if (!CombatSceneManager.Instance.HeroIsAlive)
		{
			yield break;
		}
		yield return ProcessEndOfTurnTasks();
		yield return ((MonoBehaviour)this).StartCoroutine(room.ProcessTurn());
		TurnInProgress = false;
		if (!CombatSceneManager.Instance.HeroIsAlive)
		{
			yield break;
		}
		Enemies = Enemies.OrderBy((Enemy enemy) => enemy.Cell.IndexInGrid).ToList();
		foreach (Enemy enemy3 in Enemies)
		{
			enemy3.DecideNextAction();
		}
		SetEnemiesAttackOrderUI();
		if (AllowTileInteraction && CombatInProgress)
		{
			foreach (Tile item3 in TilesManager.Instance.Deck)
			{
				item3.OnEndOfTurn();
			}
			TilesManager.Instance.CanInteractWithTiles = true;
		}
		EventsManager.Instance.EndOfCombatTurn.Invoke();
		if (CombatInProgress)
		{
			EventsManager.Instance.SaveRunProgress.Invoke();
		}
	}

	public void BeginCombat(bool campMode = false)
	{
		CombatInProgress = true;
		room = CombatSceneManager.Instance.Room;
		if (!campMode)
		{
			Globals.Hero.EnterCombatMode();
			TilesManager.Instance.CanInteractWithTiles = true;
			EventsManager.Instance.BeginningOfCombat.Invoke();
			EndOfTurnTasks.Clear();
		}
	}

	private void PreCombatHacks()
	{
	}

	private IEnumerator ProcessEndOfTurnTasks()
	{
		foreach (ICombatTask endOfTurnTask in EndOfTurnTasks)
		{
			yield return endOfTurnTask.Execute();
		}
		foreach (ICombatTask item in EndOfTurnTasks.Where((ICombatTask task) => task.IsFinished).ToList())
		{
			EndOfTurnTasks.Remove(item);
			item.FinalizeTask();
		}
	}

	public void OnEndOfCombat()
	{
		CombatInProgress = false;
		Enemies = new List<Enemy>();
		room.End();
		Globals.Hero.ExitCombatMode();
		EndOfTurnTasks.Clear();
	}

	public void OnEnemyDied(Enemy enemy)
	{
		if (!enemy.CommittedSeppuku)
		{
			KillStreak++;
			if (KillStreak > 1)
			{
				((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).Invoke(enemy);
				EffectsManager.Instance.CreateInGameEffect("KillStreakEffect", ((Component)enemy).transform);
				SoundEffectsManager.Instance.Play("SpecialHit");
			}
		}
		Enemies.Remove(enemy);
	}

	private void OnPreciseKill(Agent agent)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Globals.Hero.tileInProgress != (Object)null && Globals.Hero.tileInProgress.Attack.AttackEffect == AttackEffectEnum.perfectStrike)
		{
			Globals.Hero.tileInProgress.PostExecutionCooldownRecharge = Globals.Hero.tileInProgress.Attack.Cooldown;
			Globals.Hero.tileInProgress.OriginOfPostExecutionCooldownRechargeEffect = ((Component)agent).transform.position;
		}
	}

	private bool HeroActionAllowed(ActionEnum heroAction)
	{
		if (!AllowHeroAction)
		{
			return false;
		}
		if (Globals.Hero.AgentStats.ice > 0 && heroAction != 0)
		{
			return false;
		}
		if (heroAction == ActionEnum.attack && (Globals.Hero.AttackQueue.NTiles == 0 || Globals.Hero.AttackQueue.TCC.IsFragmented))
		{
			return false;
		}
		switch (heroAction)
		{
		case ActionEnum.moveLeft:
			return Globals.Hero.MoveActionAllowed(Dir.left);
		case ActionEnum.moveRight:
			return Globals.Hero.MoveActionAllowed(Dir.right);
		case ActionEnum.flipLeft:
			if (Globals.Hero.FacingDir == Dir.left)
			{
				return false;
			}
			break;
		}
		if (heroAction == ActionEnum.flipRight && Globals.Hero.FacingDir == Dir.right)
		{
			return false;
		}
		return true;
	}

	private bool AgentsActionInProgress()
	{
		foreach (Agent agent in Agents)
		{
			if (agent.ActionInProgress)
			{
				return true;
			}
		}
		return false;
	}

	private void SetEnemiesAttackOrderUI()
	{
		foreach (Enemy enemy in Enemies)
		{
			enemy.EnemyActionPreview.HideAttackOrder();
		}
		if (Enemies.Where((Enemy e) => e.Action == ActionEnum.attack).Count() <= 1)
		{
			return;
		}
		int num = 0;
		foreach (Enemy enemy2 in Enemies)
		{
			if (enemy2.Action == ActionEnum.attack)
			{
				enemy2.EnemyActionPreview.ShowAttackOrder(num);
				num++;
			}
		}
	}

	public void PlayerInputsCombatAction(ActionEnum action, bool stickyInput = true)
	{
		if (!CombatInProgress)
		{
			return;
		}
		if (TurnInProgress)
		{
			if (stickyInput)
			{
				((MonoBehaviour)this).StartCoroutine(StickyPlayerInputsCombatAction(action));
			}
		}
		else if (HeroActionAllowed(action))
		{
			TriggerTurn(action);
		}
		else
		{
			EffectsManager.Instance.CannotPerformActionEffect();
		}
	}

	private IEnumerator StickyPlayerInputsCombatAction(ActionEnum action)
	{
		float t = GameParams.stickyInputTime;
		while (t > 0f)
		{
			if (TurnInProgress)
			{
				t -= Time.deltaTime;
				yield return null;
				continue;
			}
			PlayerInputsCombatAction(action, stickyInput: false);
			break;
		}
	}

	public void KillEnemies()
	{
		while (Enemies.Count > 0)
		{
			Enemies[0].CommitSeppuku();
		}
	}
}
