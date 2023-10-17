using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class NobunagaBoss : Boss
{
	[SerializeField]
	private Enemy[] enemiesToSummonEasy;

	[SerializeField]
	private Enemy[] enemiesToSummonHard;

	[SerializeField]
	private Enemy[] extraEnemiesToSummonPostShogunDefeated;

	[SerializeField]
	private NobunagaArmor armor;

	private List<NobunagaVulnerableEffect> vulnerableEffects = new List<NobunagaVulnerableEffect>();

	private List<Cell> vulnerableCells = new List<Cell>();

	private int hpLastTurn;

	private int iSummon = -1;

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	private static int maxNumberOfEnemiesOnGrid = 3;

	public override string TechnicalName { get; } = "DaimyoGray";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.nobunaga_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 40;


	protected override int HigerInitialHP { get; } = 50;


	protected override int CoinReward { get; } = 10;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1];


	public override Enemy GetNextEnemyToSummon
	{
		get
		{
			Enemy[] array = ((base.AgentStats.HP > base.AgentStats.maxHP / 2) ? enemiesToSummonEasy : enemiesToSummonHard);
			iSummon = MyMath.ModularizeIndex(iSummon + 1, array.Length);
			return array[iSummon];
		}
	}

	private int NumberOfVulnerableCells
	{
		get
		{
			if ((double)base.AgentStats.HP > 0.666 * (double)base.AgentStats.maxHP)
			{
				return 3;
			}
			if ((double)base.AgentStats.HP > 0.333 * (double)base.AgentStats.maxHP)
			{
				return 2;
			}
			return 1;
		}
	}

	public override void Start()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		base.Start();
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[5]
		{
			(AttackEnum.spear, 1f),
			(AttackEnum.tetsubo, 1f),
			(AttackEnum.swirl, 1f),
			(AttackEnum.summon, 0.5f),
			(AttackEnum.shadowDash, 0.01f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities, 2f, allowSameConsecutiveResults: false);
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(OnBeginningOfCombat));
		if (UnlocksManager.Instance.ShogunDefeated)
		{
			enemiesToSummonEasy = enemiesToSummonEasy.Concat(extraEnemiesToSummonPostShogunDefeated).ToArray();
		}
	}

	protected override ActionEnum AIPickAction()
	{
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
		if (base.AttackQueue.NTiles < 1)
		{
			SelectNextAttack();
			return ActionEnum.playTile;
		}
		if (base.AttackQueue.HasOffensiveAttack && IsPathToHeroFree())
		{
			return MoveTowardsStrikingPosition();
		}
		if (HasInAttackStack(AttackEnum.summon))
		{
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}

	private void SelectNextAttack()
	{
		while (true)
		{
			tileToPlay = attacksGen.GetNext();
			attackEffectForTileToPlay = AttackEffectEnum.none;
			if (tileToPlay != AttackEnum.summon)
			{
				break;
			}
			if (CombatManager.Instance.Enemies.Count < maxNumberOfEnemiesOnGrid)
			{
				if (CombatManager.Instance.Enemies.Count == 1)
				{
					attackEffectForTileToPlay = AttackEffectEnum.replay;
				}
				break;
			}
		}
	}

	public override void ReceiveAttack(Hit hit, Agent attacker)
	{
		if (vulnerableCells.Any((Cell cell) => (Object)(object)cell == (Object)(object)base.Cell))
		{
			base.ReceiveAttack(hit, attacker);
			return;
		}
		armor.Hit();
		SoundEffectsManager.Instance.Play("ShieldDisappear");
	}

	public override void Die()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		base.Die();
		RemoveVulnerableEffects();
		EventsManager.Instance.BeginningOfCombat.RemoveListener(new UnityAction(OnBeginningOfCombat));
	}

	private void OnBeginningOfCombat()
	{
		AddVulnerableEffects();
	}

	protected override void OnCellChanged(Cell oldCell, Cell newCell)
	{
		armor.ArmorActive = !vulnerableCells.Contains(newCell);
	}

	public override IEnumerator ProcessTurn()
	{
		bool num = base.AgentStats.HP < hpLastTurn;
		hpLastTurn = base.AgentStats.HP;
		if (num && base.IsAlive)
		{
			RemoveVulnerableEffects();
			AddVulnerableEffects();
			armor.ArmorActive = !vulnerableCells.Contains(base.Cell);
		}
		yield return null;
	}

	private void RemoveVulnerableEffects()
	{
		vulnerableEffects.ForEach(delegate(NobunagaVulnerableEffect effect)
		{
			effect.Disappear();
		});
		vulnerableEffects.Clear();
	}

	private void AddVulnerableEffects()
	{
		Cell[] input = bossRoom.Grid.Cells.Where((Cell cell) => !vulnerableCells.Contains(cell) && (Object)(object)cell != (Object)(object)base.Cell).ToArray();
		vulnerableCells.Clear();
		vulnerableCells = MyRandom.NextNFromArrayNoRepetition(input, NumberOfVulnerableCells).ToList();
		SoundEffectsManager.Instance.Play("NobunagaLightAppears");
		foreach (Cell vulnerableCell in vulnerableCells)
		{
			vulnerableEffects.Add(EffectsManager.Instance.CreateInGameEffect("NobunagaVulnerableEffect", ((Component)vulnerableCell).transform).GetComponent<NobunagaVulnerableEffect>());
		}
	}
}
