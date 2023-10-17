using AgentEnums;
using CombatEnums;
using TileEnums;

public class NinjaEnemy : Enemy
{
	private AttackEnum prevAttack = AttackEnum.smokeBomb;

	public override string TechnicalName { get; } = "Ninja";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.ninja;


	public override bool IsPurelyRangedEnemy { get; } = true;


	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 4;


	protected override int HigerInitialHP { get; } = 5;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1];


	protected override EliteTypeEnum[] IncompatibleEliteTypes { get; } = new EliteTypeEnum[1] { EliteTypeEnum.quickWitted };


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
		if (base.AttackQueue.NTiles < 1)
		{
			if (prevAttack == AttackEnum.shuriken)
			{
				tileToPlay = AttackEnum.smokeBomb;
			}
			else
			{
				tileToPlay = AttackEnum.shuriken;
			}
			prevAttack = tileToPlay;
			return PlayTile(tileToPlay, base.AttackEffect);
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
