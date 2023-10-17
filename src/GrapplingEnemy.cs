using AgentEnums;
using CombatEnums;
using TileEnums;

public class GrapplingEnemy : Enemy
{
	public override string TechnicalName { get; } = "Grappler";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.grappler;


	public override bool IsPurelyRangedEnemy { get; } = true;


	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 5;


	protected override int HigerInitialHP { get; } = 6;


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
		if (previousAction == ActionEnum.playTile && base.AttackQueue.NTiles == 2 && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.grapplingHook);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.sword, base.AttackEffect);
		}
		if (base.AttackQueue.NTiles == 2)
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
