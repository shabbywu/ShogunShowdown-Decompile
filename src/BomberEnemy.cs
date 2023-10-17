using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;

public class BomberEnemy : Enemy
{
	private int escapeForTurns;

	public override string TechnicalName { get; } = "Bomber";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.bomber;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 4;


	protected override int HigerInitialHP { get; } = 4;


	protected override ActionEnum AIPickAction()
	{
		if (firstTurn)
		{
			return ActionEnum.wait;
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (escapeForTurns > 0)
		{
			escapeForTurns--;
			return MoveAwayFromHero();
		}
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.bomb);
		}
		if (base.AttackQueue.NTiles >= 1)
		{
			if ((Object)(object)base.CellInFront != (Object)null && (Object)(object)base.CellInFront.Agent != (Object)null && IsOpponent(base.CellInFront.Agent))
			{
				escapeForTurns = 3;
				return ActionEnum.attack;
			}
			if (IsPathToHeroFree())
			{
				return MoveTowardsHero();
			}
		}
		return ActionEnum.wait;
	}
}
