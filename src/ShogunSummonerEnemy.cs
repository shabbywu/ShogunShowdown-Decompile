using System.Linq;
using AgentEnums;
using CombatEnums;
using PickupEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class ShogunSummonerEnemy : Enemy
{
	private static int nTurnsBetweenAttacksEasy = 7;

	private static int nTurnsBetweenAttacksHard = 4;

	[SerializeField]
	private Enemy[] enemiesToSummonEasy;

	[SerializeField]
	private Enemy[] enemiesToSummonHard;

	[SerializeField]
	private Enemy[] extraEnemiesToSummonPostShogunDefeated;

	private Enemy nextEnemyToSummon;

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	private int waitCountdown;

	public override string TechnicalName { get; } = "Summoner";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.shogunSummoner;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; }

	public override bool Movable => false;

	protected override int DefaultInitialHP { get; } = 30;


	protected override int HigerInitialHP { get; } = 30;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.heavy };


	public override Enemy GetNextEnemyToSummon => nextEnemyToSummon;

	private int NShogunSummoners => CombatManager.Instance.Enemies.Count((Enemy e) => e.EnemyEnum == EnemyEnum.shogunSummoner);

	private int NTurnsBetweenAttacks
	{
		get
		{
			if (NShogunSummoners != 2)
			{
				return nTurnsBetweenAttacksHard;
			}
			return nTurnsBetweenAttacksEasy;
		}
	}

	public override void Start()
	{
		base.Start();
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[2]
		{
			(AttackEnum.summon, 1f),
			(AttackEnum.shieldAllied, 1f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities);
		((Component)base.AgentCombatInfo.healthBar).gameObject.SetActive(false);
		if (UnlocksManager.Instance.ShogunDefeated)
		{
			enemiesToSummonEasy = enemiesToSummonEasy.Concat(extraEnemiesToSummonPostShogunDefeated).ToArray();
		}
	}

	protected override ActionEnum AIPickAction()
	{
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (HasInAttackStack(AttackEnum.summon))
		{
			SetNextEnemyToSummon();
			waitCountdown = NTurnsBetweenAttacks;
			return ActionEnum.attack;
		}
		if (HasInAttackStack(AttackEnum.shieldAllied))
		{
			waitCountdown = NTurnsBetweenAttacks;
			return ActionEnum.attack;
		}
		if (waitCountdown > 0)
		{
			waitCountdown--;
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(attacksGen.GetNext());
		}
		return ActionEnum.wait;
	}

	public override void Die()
	{
		PickupFactory.Instance.InstantiatePickup(PickupEnum.shield, base.Cell);
		base.Die();
	}

	private void SetNextEnemyToSummon()
	{
		Enemy[] input = ((NShogunSummoners == 2) ? enemiesToSummonEasy : enemiesToSummonHard);
		nextEnemyToSummon = MyRandom.NextFromArray(input);
	}
}
