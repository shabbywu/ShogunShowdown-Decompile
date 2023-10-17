using System.Collections;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;

public class VolleyEnemy : Enemy
{
	private GameObject cellWarning;

	public override string TechnicalName { get; } = "Volley";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.volley;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; } = true;


	protected override int DefaultInitialHP { get; } = 3;


	protected override int HigerInitialHP { get; } = 4;


	public override void ExecuteAttacksInQueue()
	{
		base.ExecuteAttacksInQueue();
		((MonoBehaviour)this).StartCoroutine(DestroyCellWarning());
	}

	public override void Freeze(int duration)
	{
		base.Freeze(duration);
		if ((Object)(object)cellWarning != (Object)null && Freezable)
		{
			Object.Destroy((Object)(object)cellWarning);
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

	public override void Die()
	{
		base.Die();
		if ((Object)(object)cellWarning != (Object)null)
		{
			Object.Destroy((Object)(object)cellWarning);
		}
	}

	protected override ActionEnum AIPickAction()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (firstTurn)
		{
			return ActionEnum.wait;
		}
		if (!IsFacingHero())
		{
			return FaceHero();
		}
		if (previousAction == ActionEnum.attack)
		{
			return MoveAwayFromHero();
		}
		if (previousAction == ActionEnum.playTile && base.EliteType != EliteTypeEnum.quickWitted)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 0)
		{
			return PlayTile(AttackEnum.volley, base.AttackEffect);
		}
		if (base.AttackQueue.HasOffensiveAttack)
		{
			cellWarning = EffectsManager.Instance.CreateInGameEffect("CellWarningEffect", ((Component)Globals.Hero).transform.position);
			return ActionEnum.attack;
		}
		return ActionEnum.wait;
	}
}
