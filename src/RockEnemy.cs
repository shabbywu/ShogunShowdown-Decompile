using AgentEnums;
using CombatEnums;
using TileEnums;

public class RockEnemy : Enemy
{
	public override string TechnicalName { get; } = "Guardian";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.rock;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 5;


	protected override int HigerInitialHP { get; } = 7;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.unrelenting };


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
		if (previousAction == ActionEnum.attack && IsPathToHeroFree())
		{
			return MoveTowardsHero();
		}
		if (previousAction == ActionEnum.playTile)
		{
			if (base.EliteType == EliteTypeEnum.quickWitted && IsThreatheningHero())
			{
				return ActionEnum.attack;
			}
			if (IsPathToHeroFree())
			{
				return MoveTowardsHero();
			}
			return ActionEnum.wait;
		}
		if (IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.sword, base.AttackEffect);
		}
		if (base.AttackQueue.HasOffensiveAttack && IsPathToHeroFree())
		{
			return MoveTowardsStrikingPosition();
		}
		return ActionEnum.wait;
	}
}
