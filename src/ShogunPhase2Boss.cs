using System.Collections;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class ShogunPhase2Boss : Boss
{
	private enum PatternEnum
	{
		none,
		dashAttack,
		dashBackAndMirror,
		blazingBarrage,
		shuriken,
		origin
	}

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	private PatternEnum _currentPattern;

	private PseudoRandomWithMemory<PatternEnum> patternGen;

	public override string TechnicalName { get; } = "Shogun";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.shogun_phase2_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 40;


	protected override int HigerInitialHP { get; } = 50;


	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[2]
	{
		EnemyTraitsEnum.quickWitted,
		EnemyTraitsEnum.unfreezable
	};


	public override bool Freezable { get; }

	private PatternEnum CurrentPattern
	{
		get
		{
			return _currentPattern;
		}
		set
		{
			PreviousPattern = _currentPattern;
			_currentPattern = value;
		}
	}

	private PatternEnum PreviousPattern { get; set; }

	public override void Start()
	{
		base.Start();
		(PatternEnum, float)[] choicesAndBaseProbabilities = new(PatternEnum, float)[3]
		{
			(PatternEnum.dashAttack, 1f),
			(PatternEnum.dashBackAndMirror, 1f),
			(PatternEnum.blazingBarrage, 1f)
		};
		patternGen = new PseudoRandomWithMemory<PatternEnum>(choicesAndBaseProbabilities);
		CurrentPattern = PatternEnum.blazingBarrage;
	}

	public override void Die()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.Die();
		EffectsManager.Instance.CreateInGameEffect("LargeHitEffect", ((Component)this).transform.position);
	}

	public override IEnumerator ProcessTurn()
	{
		if (!base.IsAlive)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(PostDeathSequence());
		}
	}

	private IEnumerator PostDeathSequence()
	{
		MusicManager.Instance.Play("Victory", 0f);
		EffectsManager.Instance.HugeScreenShake();
		EffectsManager.Instance.CreateInGameEffect("ShogunDeathEffect", ((Component)this).transform.position);
		EffectsManager.Instance.GamepadRumble(3f, 0.3f);
		yield return (object)new WaitForSeconds(2.64f);
		((Component)base.AgentGraphics).gameObject.SetActive(false);
		((Component)bossRoom.bossHealthBar).gameObject.SetActive(false);
		((ShogunBossRoom)bossRoom).SwitchToPhase3();
		yield return (object)new WaitForSeconds(0.5f);
	}

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
		if (CurrentPattern == PatternEnum.none)
		{
			PickNextPattern();
		}
		return CurrentPattern switch
		{
			PatternEnum.dashAttack => DashAttackPattern(), 
			PatternEnum.dashBackAndMirror => DashBackAndMirrorPattern(), 
			PatternEnum.blazingBarrage => BlazingBarragePattern(), 
			PatternEnum.shuriken => ShurikenPattern(), 
			PatternEnum.origin => OriginPattern(), 
			_ => ActionEnum.wait, 
		};
	}

	private void PickNextPattern()
	{
		if (PreviousPattern == PatternEnum.shuriken)
		{
			CurrentPattern = PatternEnum.origin;
			return;
		}
		PatternEnum next;
		do
		{
			next = patternGen.GetNext();
		}
		while (next == PreviousPattern);
		CurrentPattern = next;
	}

	private ActionEnum DashAttackPattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.dashForward);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.swirl);
		}
		CurrentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum BlazingBarragePattern()
	{
		if (base.AttackQueue.NTiles == 0 && (Object)(object)base.Cell != (Object)(object)bossRoom.Grid.CentralCell())
		{
			return PlayTile(AttackEnum.origin);
		}
		if (base.AttackQueue.NTiles == 0 && (Object)(object)base.Cell == (Object)(object)bossRoom.Grid.CentralCell())
		{
			return PlayTile(AttackEnum.blazingBarrage);
		}
		if (base.AttackQueue.NTiles == 1 && HasInAttackStack(AttackEnum.origin))
		{
			return PlayTile(AttackEnum.blazingBarrage);
		}
		HasInAttackStack(AttackEnum.blazingBarrage);
		CurrentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum OriginPattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.origin);
		}
		CurrentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}

	private ActionEnum DashBackAndMirrorPattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.dashBackward);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.mirror);
		}
		CurrentPattern = PatternEnum.shuriken;
		return ActionEnum.attack;
	}

	private ActionEnum ShurikenPattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.shuriken);
		}
		CurrentPattern = PatternEnum.none;
		return ActionEnum.attack;
	}
}
