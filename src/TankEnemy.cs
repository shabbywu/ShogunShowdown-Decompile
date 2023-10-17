using AgentEnums;
using CombatEnums;
using TileEnums;

public class TankEnemy : Enemy
{
	public override string TechnicalName { get; } = "Swordsman";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.tank;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 3;


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
		if (previousAction == ActionEnum.attack)
		{
			return MoveAwayFromHero();
		}
		if (previousAction == ActionEnum.playTile && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.sword, base.AttackEffect);
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
		return ActionEnum.wait;
	}
}
