using AgentEnums;
using CombatEnums;
using TileEnums;

public class ArcherEnemy : Enemy
{
	public override string TechnicalName { get; } = "Archer";


	public override EnemyEnum EnemyEnum { get; }

	public override bool IsPurelyRangedEnemy { get; } = true;


	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 2;


	protected override int HigerInitialHP { get; } = 3;


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
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.arrow, base.AttackEffect);
		}
		if (base.AttackQueue.HasOffensiveAttack && IsThreatheningHero())
		{
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
