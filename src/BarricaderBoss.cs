using System.Collections;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class BarricaderBoss : Boss
{
	private enum PatternEnum
	{
		none,
		meleeAttack,
		buildBarricade,
		barricaded
	}

	[SerializeField]
	private Enemy[] enemiesToSummon;

	private PatternEnum _currentPattern;

	private GameObject cellWarning;

	private AttackEnum prevBarricatedAttack = AttackEnum.volley;

	public override string TechnicalName { get; } = "BarricaderBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.barricader_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 20;


	protected override int HigerInitialHP { get; } = 25;


	protected override int CoinReward { get; } = 10;


	public override Enemy GetNextEnemyToSummon => MyRandom.NextRandomUniform(enemiesToSummon);

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

	public override void ExecuteAttacksInQueue()
	{
		base.ExecuteAttacksInQueue();
		((MonoBehaviour)this).StartCoroutine(DestroyCellWarning());
	}

	public override void Die()
	{
		base.Die();
		if ((Object)(object)cellWarning != (Object)null)
		{
			Object.Destroy((Object)(object)cellWarning);
		}
	}

	public override void Freeze(int duration)
	{
		base.Freeze(duration);
		if ((Object)(object)cellWarning != (Object)null && Freezable)
		{
			Object.Destroy((Object)(object)cellWarning);
		}
	}

	protected override ActionEnum AIPickAction()
	{
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		SelectPattern();
		return CurrentPattern switch
		{
			PatternEnum.meleeAttack => OffensivePattern(), 
			PatternEnum.buildBarricade => BuildBarricadePattern(), 
			PatternEnum.barricaded => BarricadedPattern(), 
			_ => ActionEnum.wait, 
		};
	}

	private ActionEnum BarricadedPattern()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.attack)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(prevBarricatedAttack = ((prevBarricatedAttack == AttackEnum.summon || CombatManager.Instance.Agents.Count >= 5) ? AttackEnum.volley : AttackEnum.summon));
		}
		if (base.AttackQueue.NTiles == 1)
		{
			if (HasInAttackStack(AttackEnum.volley))
			{
				cellWarning = EffectsManager.Instance.CreateInGameEffect("CellWarningEffect", ((Component)Globals.Hero).transform.position);
			}
			CurrentPattern = PatternEnum.none;
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}

	private ActionEnum OffensivePattern()
	{
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.swirl);
		}
		if (base.AttackQueue.HasOffensiveAttack)
		{
			if (IsThreatheningHero())
			{
				CurrentPattern = PatternEnum.none;
				return ActionEnum.attack;
			}
			if (IsPathToHeroFree())
			{
				return MoveTowardsStrikingPosition();
			}
		}
		return ActionEnum.wait;
	}

	private ActionEnum BuildBarricadePattern()
	{
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(((Object)(object)base.CellBehind == (Object)null) ? AttackEnum.dragonPunch : AttackEnum.dashBackward);
		}
		if (base.AttackQueue.NTiles == 1)
		{
			return PlayTile(AttackEnum.barricade);
		}
		if (base.AttackQueue.NTiles == 2)
		{
			CurrentPattern = PatternEnum.none;
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}

	private void SelectPattern()
	{
		if (CurrentPattern == PatternEnum.none)
		{
			if (PreviousPattern == PatternEnum.meleeAttack)
			{
				CurrentPattern = PatternEnum.buildBarricade;
			}
			else if (IsPathToHeroFree())
			{
				CurrentPattern = PatternEnum.meleeAttack;
			}
			else
			{
				CurrentPattern = PatternEnum.barricaded;
			}
		}
		else if (CurrentPattern == PatternEnum.meleeAttack && !IsPathToHeroFree())
		{
			((EnemyAttackQueue)base.AttackQueue).Clear();
			CurrentPattern = PatternEnum.barricaded;
		}
	}

	private IEnumerator DestroyCellWarning()
	{
		yield return (object)new WaitForSeconds(VolleyAttack.TimeBeforeHit);
		if ((Object)(object)cellWarning != (Object)null)
		{
			Object.Destroy((Object)(object)cellWarning);
		}
	}
}
