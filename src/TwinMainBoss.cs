using System;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class TwinMainBoss : Boss
{
	private PseudoRandomWithMemory<AttackEnum> flankAttackGen;

	private PseudoRandomWithMemory<AttackEnum> frontalAttackGen;

	protected Boss otherTwin;

	public override string TechnicalName { get; } = "TwinsBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.twin_main_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 35;


	protected override int HigerInitialHP { get; } = 45;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.unrelenting };


	protected override int MetaCurrencyReward => (int)Math.Ceiling((float)base.MetaCurrencyReward / 2f);

	protected override int CoinReward { get; } = 3;


	public override void Start()
	{
		base.Start();
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[2]
		{
			(AttackEnum.shadowDash, 1f),
			(AttackEnum.phantomLeap, 1f)
		};
		(AttackEnum, float)[] choicesAndBaseProbabilities2 = new(AttackEnum, float)[3]
		{
			(AttackEnum.spear, 1f),
			(AttackEnum.swirl, 1f),
			(AttackEnum.tetsubo, 1f)
		};
		flankAttackGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities);
		frontalAttackGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities2);
		AssignTwinBossesToEachOther();
	}

	public override void FirstTimeBossFightInitializations(Room room)
	{
		InstantiateTwinSideBoss(room);
	}

	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		if (base.AgentStats.HP <= 0 && base.IsAlive)
		{
			otherTwin.Die();
		}
		base.PostHealthUpdateEvents(actualDeltaHealth);
	}

	private void AssignTwinBossesToEachOther()
	{
		EnemyEnum otherTwinEnum = ((EnemyEnum == EnemyEnum.twin_main_boss) ? EnemyEnum.twin_side_boss : EnemyEnum.twin_main_boss);
		otherTwin = (Boss)CombatManager.Instance.Enemies.Find((Enemy e) => e.EnemyEnum == otherTwinEnum);
	}

	private void InstantiateTwinSideBoss(Room room)
	{
		Enemy enemy = AgentsFactory.Instance.InstantiateEnemy(EnemyEnum.twin_side_boss, ((Component)room).transform);
		enemy.Cell = room.Grid.LeftMostCell();
		enemy.SetPositionToCellPosition();
		enemy.FacingDir = Dir.right;
		CombatManager.Instance.Enemies.Add(enemy);
	}

	protected override ActionEnum AIPickAction()
	{
		if (firstTurn)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.attack)
		{
			if (IsPathToHeroFree())
			{
				return MoveTowardsHero();
			}
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile && (base.AttackQueue.NTiles != 1 || !HasInAttackStack(AttackEnum.phantomLeap)))
		{
			return ActionEnum.wait;
		}
		if (!IsPathToHeroFree())
		{
			return NoFreePathPattern();
		}
		if (!IsPathToHeroFreeForAllEnemies())
		{
			return FlankPattern();
		}
		return FrontalAttackPattern();
	}

	private ActionEnum NoFreePathPattern()
	{
		if (base.CanMoveAwayFromHero)
		{
			return MoveAwayFromHero();
		}
		if (!HasInAttackStack(AttackEnum.spear))
		{
			((EnemyAttackQueue)base.AttackQueue).Clear();
			return PlayTile(AttackEnum.spear);
		}
		return ActionEnum.wait;
	}

	private ActionEnum FrontalAttackPattern()
	{
		if (IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (!base.AttackQueue.HasOffensiveAttack)
		{
			if (base.AttackQueue.NTiles > 0)
			{
				((EnemyAttackQueue)base.AttackQueue).Clear();
			}
			return PlayTile(frontalAttackGen.GetNext());
		}
		return MoveTowardsStrikingPosition();
	}

	private ActionEnum FlankPattern()
	{
		if (IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (base.AttackQueue.NTiles == 0)
		{
			if (CouldUseShadowDash())
			{
				return PlayTile(flankAttackGen.GetNext());
			}
			return PlayTile(AttackEnum.phantomLeap);
		}
		if (base.AttackQueue.NTiles == 1 && HasInAttackStack(AttackEnum.phantomLeap))
		{
			return PlayTile(AttackEnum.swapToss);
		}
		if (HasInAttackStack(AttackEnum.phantomLeap) && HasInAttackStack(AttackEnum.swapToss))
		{
			return ActionEnum.attack;
		}
		if (base.AttackQueue.HasOffensiveAttack)
		{
			return MoveTowardsStrikingPosition();
		}
		return ActionEnum.wait;
	}

	private bool CouldUseShadowDash()
	{
		Cell cell = Globals.Hero.Cell.Neighbour(HeroDir(), 1);
		if ((Object)(object)cell != (Object)null)
		{
			return (Object)(object)cell.Agent == (Object)null;
		}
		return false;
	}

	private bool IsPathToHeroFreeForAllEnemies()
	{
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			if (!((Object)(object)enemy == (Object)(object)this) && !enemy.IsPathToHeroFree())
			{
				return false;
			}
		}
		return true;
	}
}
