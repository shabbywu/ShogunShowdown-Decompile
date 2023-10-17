using System.Collections.Generic;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class ShielderEnemy : Enemy
{
	public override string TechnicalName { get; } = "Shielder";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.shielder;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; }

	protected override int DefaultInitialHP { get; } = 4;


	protected override int HigerInitialHP { get; } = 5;


	protected override ActionEnum AIPickAction()
	{
		if (firstTurn)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.attack)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (CombatManager.Instance.Enemies.Count == 1)
		{
			return CompletelyAlonePattern();
		}
		List<Enemy> enemiesWithoutShield = GetEnemiesWithoutShield();
		if (enemiesWithoutShield.Count == 0)
		{
			return NoAvailableUnshieldedAllyPattern();
		}
		return AlliesWithoutShieldPattern(enemiesWithoutShield);
	}

	private ActionEnum AlliesWithoutShieldPattern(List<Enemy> alliesWithoutShield)
	{
		Enemy targetEnemy = GetTargetEnemy(alliesWithoutShield);
		if (base.FacingDir != base.Cell.DirectionToOtherCell(targetEnemy.Cell))
		{
			return TurnAroundActionEnum();
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.shieldAllied);
		}
		return ActionEnum.attack;
	}

	private ActionEnum NoAvailableUnshieldedAllyPattern()
	{
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (base.CanMoveAwayFromHero)
		{
			return MoveAwayFromHero();
		}
		if (HasInAttackStack(AttackEnum.shield))
		{
			return ActionEnum.attack;
		}
		if (!base.HasShield)
		{
			if (HasInAttackStack(AttackEnum.shieldAllied))
			{
				((EnemyAttackQueue)base.AttackQueue).Clear();
			}
			return PlayTile(AttackEnum.shield);
		}
		return ActionEnum.wait;
	}

	private ActionEnum CompletelyAlonePattern()
	{
		if (base.CanMoveAwayFromHero)
		{
			if (!IsFacingHero())
			{
				return FaceHero();
			}
			return MoveAwayFromHero();
		}
		if (HasInAttackStack(AttackEnum.shield))
		{
			return ActionEnum.attack;
		}
		if (!base.HasShield)
		{
			if (HasInAttackStack(AttackEnum.shieldAllied))
			{
				((EnemyAttackQueue)base.AttackQueue).Clear();
			}
			return PlayTile(AttackEnum.shield);
		}
		return TurnAroundActionEnum();
	}

	private Enemy GetTargetEnemy(List<Enemy> enemies)
	{
		foreach (Cell item in base.Cell.AllCellsInDirection(base.FacingDir))
		{
			if (item.Agent is Enemy && enemies.Contains((Enemy)item.Agent))
			{
				return (Enemy)item.Agent;
			}
		}
		foreach (Cell item2 in base.Cell.AllCellsInDirection(DirUtils.Opposite(base.FacingDir)))
		{
			if (item2.Agent is Enemy && enemies.Contains((Enemy)item2.Agent))
			{
				return (Enemy)item2.Agent;
			}
		}
		Debug.LogError((object)"Shielder: GetTargetEnemy: should not get Here!!!");
		return null;
	}

	private List<Enemy> GetEnemiesWithoutShield()
	{
		List<Enemy> list = new List<Enemy>();
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			if (!enemy.HasShield && !((Object)(object)enemy == (Object)(object)this))
			{
				list.Add(enemy);
			}
		}
		return list;
	}
}
