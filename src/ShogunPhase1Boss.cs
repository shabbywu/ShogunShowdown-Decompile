using System.Collections;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class ShogunPhase1Boss : Boss
{
	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	private int enemySpawnFrequency = 9;

	private int nBeforeSpawn;

	public override string TechnicalName { get; } = "Shogun";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.shogun_phase1_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 40;


	protected override int HigerInitialHP { get; } = 50;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[2]
	{
		EnemyTraitsEnum.quickWitted,
		EnemyTraitsEnum.unfreezable
	};


	public override bool Freezable { get; }

	public override IEnumerator ProcessTurn()
	{
		if (!base.IsAlive)
		{
			EffectsManager.Instance.HugeScreenShake();
			EffectsManager.Instance.CreateInGameEffect("ShogunTransformationEffect", ((Component)bossRoom).transform);
			EffectsManager.Instance.GamepadRumble(2.5f, 0.3f);
			yield return (object)new WaitForSeconds(2.5f);
			InstantiateShogunPhase2();
			CombatManager.Instance.Enemies.Remove(this);
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		yield return null;
	}

	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		bossRoom.bossHealthBar.UpdateHealth(base.AgentStats.maxHP, base.AgentStats.HP);
		if (base.AgentStats.HP > 0)
		{
			return;
		}
		Die();
		Enemy[] array = CombatManager.Instance.Enemies.FindAll((Enemy e) => e != null && !(e is Boss)).ToArray();
		foreach (Enemy enemy in array)
		{
			if (enemy.IsAlive)
			{
				enemy.CommitSeppuku();
			}
		}
	}

	private void InstantiateShogunPhase2()
	{
		Boss boss = (Boss)AgentsFactory.Instance.InstantiateEnemy(EnemyEnum.shogun_phase2_boss, ((Component)base.Cell).transform);
		boss.Cell = base.Cell;
		boss.FacingDir = base.FacingDir;
		bossRoom.SetBoss(boss);
		bossRoom.bossHealthBar.UpdateHealth(boss.AgentStats.maxHP, boss.AgentStats.HP);
		CombatManager.Instance.Enemies.Add(boss);
		((ShogunBossRoom)bossRoom).SwitchToPhase2();
	}

	private void InstantiateAllies(Room room)
	{
		Cell cell = room.Grid.Cells[0];
		Cell cell2 = room.Grid.Cells[room.Grid.Cells.Length - 1];
		Enemy enemy = AgentsFactory.Instance.InstantiateEnemy(EnemyEnum.shogunSummoner, ((Component)cell).transform);
		Enemy enemy2 = AgentsFactory.Instance.InstantiateEnemy(EnemyEnum.shogunSummoner, ((Component)cell2).transform);
		enemy.Cell = cell;
		enemy2.Cell = cell2;
		enemy.FacingDir = Dir.right;
		enemy2.FacingDir = Dir.left;
		CombatManager.Instance.Enemies.Add(enemy);
		CombatManager.Instance.Enemies.Add(enemy2);
	}

	public override void Start()
	{
		base.Start();
		nBeforeSpawn = enemySpawnFrequency;
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[6]
		{
			(AttackEnum.sword, 1f),
			(AttackEnum.spear, 1f),
			(AttackEnum.swirl, 1f),
			(AttackEnum.dashForward, 1f),
			(AttackEnum.dragonPunch, 1f),
			(AttackEnum.mirror, 1f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities);
	}

	public override void FirstTimeBossFightInitializations(Room room)
	{
		InstantiateAllies(room);
	}

	protected override ActionEnum AIPickAction()
	{
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (previousAction == ActionEnum.attack)
		{
			return MoveAwayFromHero();
		}
		if (base.AttackQueue.NTiles == 0)
		{
			AttackEnum next;
			do
			{
				next = attacksGen.GetNext();
			}
			while (next == AttackEnum.mirror && (Object)(object)base.Cell == (Object)(object)bossRoom.Grid.CentralCell());
			return PlayTile(next);
		}
		if (base.AttackQueue.NTiles == 1 && HasInAttackStack(AttackEnum.dashForward))
		{
			return PlayTile(AttackEnum.spear);
		}
		if (base.AttackQueue.HasOffensiveAttack)
		{
			if (IsThreatheningHero())
			{
				return ActionEnum.attack;
			}
			if (IsPathToHeroFree())
			{
				return MoveTowardsStrikingPosition();
			}
		}
		else if (base.AttackQueue.NTiles > 0)
		{
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
