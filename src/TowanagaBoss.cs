using System.Collections;
using System.Collections.Generic;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class TowanagaBoss : Boss
{
	private List<AttackEnum> availableAttacks = new List<AttackEnum> { AttackEnum.summon };

	private List<AttackEnum> recentlyAddedAttacks = new List<AttackEnum>();

	private PseudoRandomWithMemory<AttackEnum> meleeAttacksGen;

	private PseudoRandomWithMemory<AttackEnum> rangedAttacksGen;

	private PseudoRandomWithMemory<AttackEffectEnum> effectsGen;

	[SerializeField]
	private Enemy[] enemiesToSummon;

	public override string TechnicalName { get; } = "DaimyoDarkGreen";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.tokugawa_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 40;


	protected override int HigerInitialHP { get; } = 50;


	protected override int CoinReward { get; } = 10;


	public override Enemy GetNextEnemyToSummon => MyRandom.NextRandomUniform(enemiesToSummon);

	public override void Start()
	{
		base.Start();
	}

	protected override ActionEnum AIPickAction()
	{
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
		if (base.AttackQueue.HasOffensiveAttack && IsPathToHeroFree())
		{
			ActionEnum actionEnum = MoveTowardsStrikingPosition();
			if (actionEnum != 0)
			{
				return actionEnum;
			}
			return ActionEnum.attack;
		}
		if (HasInAttackStack(AttackEnum.summon))
		{
			return ActionEnum.attack;
		}
		if (base.AttackQueue.NTiles < 3)
		{
			if (recentlyAddedAttacks.Count > 0)
			{
				tileToPlay = recentlyAddedAttacks[0];
				recentlyAddedAttacks.RemoveAt(0);
				return ActionEnum.playTile;
			}
			return PlayTile(MyRandom.NextRandomUniform(availableAttacks));
		}
		return ActionEnum.attack;
	}

	public override IEnumerator ProcessTurn()
	{
		Tile[] tiles = Globals.Hero.AttackQueue.TCC.Tiles;
		foreach (Tile tile in tiles)
		{
			if (!availableAttacks.Contains(tile.Attack.AttackEnum))
			{
				availableAttacks.Add(tile.Attack.AttackEnum);
				recentlyAddedAttacks.Add(tile.Attack.AttackEnum);
			}
		}
		yield return null;
	}
}
