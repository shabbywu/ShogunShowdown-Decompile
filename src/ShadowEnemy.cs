using AgentEnums;
using CombatEnums;
using TileEnums;

public class ShadowEnemy : Enemy
{
	public override string TechnicalName { get; } = "ShadowDasher";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.shadow;


	public override bool IsPurelyRangedEnemy { get; } = true;


	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 5;


	protected override int HigerInitialHP { get; } = 7;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1];


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
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.dashForward);
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
