using AgentEnums;
using CombatEnums;
using TileEnums;

public class DasherEnemy : Enemy
{
	public override string TechnicalName { get; } = "Dasher";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.dasher;


	public override bool IsPurelyRangedEnemy { get; } = true;


	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 1;


	protected override int HigerInitialHP { get; } = 2;


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
			return PlayTile(AttackEnum.dashForward, base.AttackEffect);
		}
		if (base.AttackQueue.HasOffensiveAttack && IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
