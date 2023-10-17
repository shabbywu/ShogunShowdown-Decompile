using System.Linq;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;

public class SummonerEnemy : Enemy
{
	[SerializeField]
	private Enemy[] enemiesToSummon;

	[SerializeField]
	private Enemy[] extraEnemiesToSummonPostShogunDefeated;

	private Enemy nextEnemyToSummon;

	private int iSummon;

	private int waitBeforeSummoning;

	public override string TechnicalName { get; } = "Summoner";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.summoner;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; }

	public override bool Movable => false;

	protected override int DefaultInitialHP { get; } = 15;


	protected override int HigerInitialHP { get; } = 15;


	public override Enemy GetNextEnemyToSummon => nextEnemyToSummon;

	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.heavy };


	public override void Start()
	{
		base.Start();
		if (UnlocksManager.Instance.ShogunDefeated)
		{
			enemiesToSummon = enemiesToSummon.Concat(extraEnemiesToSummonPostShogunDefeated).ToArray();
		}
	}

	protected override ActionEnum AIPickAction()
	{
		if (waitBeforeSummoning > 0)
		{
			waitBeforeSummoning--;
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.attack)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.summon);
		}
		if (HasInAttackStack(AttackEnum.summon))
		{
			waitBeforeSummoning = 6;
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
