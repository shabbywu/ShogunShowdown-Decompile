using System.Linq;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class HideyoshiBoss : Boss
{
	private enum PatternEnum
	{
		none,
		centralCellDamage,
		summon,
		backdashMirrorSpear,
		bombAndGoAway,
		swirlMirrorSwirl
	}

	private PseudoRandomWithMemory<PatternEnum> patternGen;

	private PatternEnum currentPattern;

	private PatternEnum previousPattern;

	[SerializeField]
	private Enemy[] enemiesToSummon;

	[SerializeField]
	private Enemy[] extraEnemiesToSummonPostShogunDefeated;

	private int iSummon = -1;

	public override string TechnicalName { get; } = "DaimyoWhite";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.hideyoshi_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 45;


	protected override int HigerInitialHP { get; } = 55;


	protected override int CoinReward { get; } = 10;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[2]
	{
		EnemyTraitsEnum.quickWitted,
		EnemyTraitsEnum.unfreezable
	};


	public override bool Freezable { get; }

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
		(PatternEnum, float)[] choicesAndBaseProbabilities = new(PatternEnum, float)[5]
		{
			(PatternEnum.centralCellDamage, 1f),
			(PatternEnum.summon, 1.5f),
			(PatternEnum.backdashMirrorSpear, 1f),
			(PatternEnum.bombAndGoAway, 1f),
			(PatternEnum.swirlMirrorSwirl, 1f)
		};
		patternGen = new PseudoRandomWithMemory<PatternEnum>(choicesAndBaseProbabilities);
		if (UnlocksManager.Instance.ShogunDefeated)
		{
			enemiesToSummon = enemiesToSummon.Concat(extraEnemiesToSummonPostShogunDefeated).ToArray();
		}
	}

	protected override ActionEnum AIPickAction()
	{
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (currentPattern == PatternEnum.none)
		{
			PickNextPattern();
		}
		return currentPattern switch
		{
			PatternEnum.centralCellDamage => CentralDamageCellPattern(), 
			PatternEnum.summon => Summon(), 
			PatternEnum.backdashMirrorSpear => BackdashMirrorSpear(), 
			PatternEnum.bombAndGoAway => BombAndGoAway(), 
			PatternEnum.swirlMirrorSwirl => SwirlMirrorSwirl(), 
			_ => ActionEnum.wait, 
		};
	}

	private void PickNextPattern()
	{
		do
		{
			currentPattern = patternGen.GetNext();
		}
		while ((currentPattern == PatternEnum.summon && CombatManager.Instance.Enemies.Count > 2) || currentPattern == previousPattern);
		previousPattern = currentPattern;
	}

	private ActionEnum CentralDamageCellPattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.origin);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.swirl);
		}
		if (base.AttackQueue.NTiles == 2)
		{
			return PlayTile(AttackEnum.earthImpale);
		}
		currentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum Summon()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.summon, AttackEffectEnum.replay);
		}
		currentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum SwirlMirrorSwirl()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.swirl);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.mirror);
		}
		if (base.AttackQueue.NTiles == 2)
		{
			return PlayTile(AttackEnum.swirl);
		}
		currentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum BackdashMirrorSpear()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.dashBackward);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.mirror);
		}
		if (base.AttackQueue.NTiles == 2)
		{
			return PlayTile(AttackEnum.spear);
		}
		currentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum BombAndGoAway()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.phantomLeap);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.bomb);
		}
		if (base.AttackQueue.NTiles == 2)
		{
			return PlayTile(AttackEnum.dashBackward);
		}
		if (!IsPathToHeroFree())
		{
			return ActionEnum.wait;
		}
		currentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}
}
