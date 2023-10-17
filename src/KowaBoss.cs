using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class KowaBoss : Boss
{
	private int maxEnemies = 3;

	[SerializeField]
	private Enemy[] enemiesToSummon;

	[SerializeField]
	private Enemy[] extraEnemiesToSummonPostShogunDefeated;

	private int iSummon = -1;

	private static int nTunrsBetweenSpawning = 8;

	private int spawningCountdown;

	public override string TechnicalName { get; } = "KowaBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.kowa_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 30;


	protected override int HigerInitialHP { get; } = 40;


	public override Enemy GetNextEnemyToSummon
	{
		get
		{
			iSummon = MyMath.ModularizeIndex(iSummon + 1, enemiesToSummon.Length);
			return enemiesToSummon[iSummon];
		}
	}

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
		if (previousAction == ActionEnum.attack)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.CanMoveAwayFromHero)
		{
			return MoveAwayFromHero();
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		attackEffectForTileToPlay = AttackEffectEnum.none;
		if (base.AttackQueue.NTiles == 0)
		{
			if (!base.HasShield && base.LastTilePlayed != AttackEnum.shield)
			{
				return PlayTile(AttackEnum.shield);
			}
			return PlayTile(AttackEnum.smokeBomb);
		}
		if (base.AttackQueue.NTiles >= 1)
		{
			if (!base.AttackQueue.HasOffensiveAttack)
			{
				return ActionEnum.attack;
			}
			if (IsThreatheningHero())
			{
				return ActionEnum.attack;
			}
		}
		return ActionEnum.wait;
	}

	public override IEnumerator ProcessTurn()
	{
		if (!base.IsAlive)
		{
			yield break;
		}
		bool flag = CombatManager.Instance.Enemies.Count < maxEnemies;
		if (flag)
		{
			spawningCountdown--;
		}
		if (spawningCountdown <= 0 && flag)
		{
			yield return (object)new WaitForSeconds(0.1f);
			SoundEffectsManager.Instance.Play("KowaBell");
			yield return (object)new WaitForSeconds(0.3f);
			int num = maxEnemies - CombatManager.Instance.Enemies.Count;
			List<Enemy> list = new List<Enemy>();
			for (int i = 0; i < num; i++)
			{
				list.Add(GetNextEnemyToSummon);
			}
			new Wave(list.ToArray(), 1, 1f, summoned: true).Spawn((CombatRoom)CombatSceneManager.Instance.Room);
			yield return (object)new WaitForSeconds(0.3f);
			spawningCountdown = nTunrsBetweenSpawning;
		}
	}
}
