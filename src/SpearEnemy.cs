using AgentEnums;
using CombatEnums;
using TileEnums;

public class SpearEnemy : Enemy
{
	public override string TechnicalName { get; } = "Spear";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.spear;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 4;


	protected override int HigerInitialHP { get; } = 5;


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
			return PlayTile(AttackEnum.spear, base.AttackEffect);
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
