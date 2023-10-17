using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class WaterfallCavesBoss : Boss
{
	[SerializeField]
	private Enemy[] enemiesToSummon;

	private Enemy nextEnemyToSummon;

	private int iSummon;

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	public override string TechnicalName { get; } = "ImpalerBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.waterfall_caves_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 22;


	protected override int HigerInitialHP { get; } = 28;


	protected override int CoinReward { get; } = 10;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.unfreezable };


	public override bool Freezable { get; }

	public override Enemy GetNextEnemyToSummon => nextEnemyToSummon;

	public override void Start()
	{
		base.Start();
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[3]
		{
			(AttackEnum.spear, 1f),
			(AttackEnum.earthImpale, 1f),
			(AttackEnum.summon, 0.25f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities, 2f, allowSameConsecutiveResults: false);
	}

	protected override ActionEnum AIPickAction()
	{
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.HasOffensiveAttack && IsThreatheningHero())
		{
			return ActionEnum.attack;
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
			return PlayTile(attacksGen.GetNext());
		}
		if (base.AttackQueue.NTiles == 1 && HasInAttackStack(AttackEnum.earthImpale) && DistanceFromHero() == 1)
		{
			return PlayTile(AttackEnum.swirl);
		}
		if (base.AttackQueue.HasOffensiveAttack && IsPathToHeroFree())
		{
			return MoveTowardsStrikingPosition();
		}
		if (HasInAttackStack(AttackEnum.summon))
		{
			nextEnemyToSummon = enemiesToSummon[iSummon];
			iSummon++;
			if (iSummon >= enemiesToSummon.Length)
			{
				iSummon = 0;
			}
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
