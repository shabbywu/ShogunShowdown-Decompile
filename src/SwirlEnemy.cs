using AgentEnums;
using CombatEnums;
using TileEnums;

public class SwirlEnemy : Enemy
{
	public override string TechnicalName { get; } = "Swirler";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.swirler;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 2;


	protected override int HigerInitialHP { get; } = 3;


	protected override ActionEnum AIPickAction()
	{
		if (firstTurn)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.HasOffensiveAttack && IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		if (previousAction == ActionEnum.attack)
		{
			return MoveAwayFromHero();
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.swirl, base.AttackEffect);
		}
		if (base.AttackQueue.HasOffensiveAttack && IsPathToHeroFree())
		{
			return MoveTowardsStrikingPosition();
		}
		return ActionEnum.wait;
	}
}
