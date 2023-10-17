using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class BambooGroveBoss : Boss
{
	[SerializeField]
	private Enemy enemyToSummon;

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	public override string TechnicalName { get; } = "DasherBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.bamboo_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 15;


	protected override int HigerInitialHP { get; } = 20;


	protected override int CoinReward { get; } = 10;


	public override Enemy GetNextEnemyToSummon => enemyToSummon;

	public override void Start()
	{
		base.Start();
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[4]
		{
			(AttackEnum.summon, 1f),
			(AttackEnum.sword, 0.75f),
			(AttackEnum.swirl, 0.75f),
			(AttackEnum.dashForward, 1f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities, 2f, allowSameConsecutiveResults: false);
	}

	protected override ActionEnum AIPickAction()
	{
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (previousAction == ActionEnum.attack)
		{
			return MoveAwayFromHero();
		}
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(attacksGen.GetNext());
		}
		if (base.AttackQueue.NTiles == 1 && HasInAttackStack(AttackEnum.dashForward))
		{
			return PlayTile(AttackEnum.sword);
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
		if (HasInAttackStack(AttackEnum.summon))
		{
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
