using System.Collections;
using TileEnums;
using UnityEngine;

public class KaitenryukenAttack : Attack
{
	private static readonly int hitInFrontFrame = 9;

	private static readonly int hitBehindFrame = 33;

	private static readonly int endOfAttackFrame = 45;

	private Cell cellInFront;

	private Cell cellBehind;

	public override AttackEnum AttackEnum => AttackEnum.kaitenryuken;

	public override string LocalizationTableKey => "Kaitenryuken";

	public override int InitialValue => 2;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = new int[2] { -1, 1 };


	public override string AnimationTrigger { get; protected set; } = "KaitenryukenAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	public override void ApplyEffect()
	{
		attacker.Flip();
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		cellInFront = attacker.CellInFront;
		cellBehind = attacker.CellBehind;
		EffectsManager.Instance.CreateInGameEffect("LandWindWirlEffect", ((Component)attacker).transform.position);
		yield return (object)new WaitForSeconds((float)hitInFrontFrame / 60f);
		HitInFront();
		yield return (object)new WaitForSeconds((float)(hitBehindFrame - hitInFrontFrame) / 60f);
		HitBehind();
		yield return (object)new WaitForSeconds((float)(endOfAttackFrame - hitBehindFrame) / 60f);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.AttackInProgress = false;
	}

	private void HitInFront()
	{
		SoundEffectsManager.Instance.Play("KaitenryukenUp");
		if ((Object)(object)cellInFront != (Object)null && (Object)(object)cellInFront.Agent != (Object)null)
		{
			HitTarget(cellInFront.Agent);
		}
	}

	private void HitBehind()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("KaitenryukenDown");
		SoundEffectsManager.Instance.Play("KaitenryukenGroundHit");
		if ((Object)(object)cellBehind != (Object)null && (Object)(object)cellBehind.Agent != (Object)null)
		{
			HitTarget(cellBehind.Agent);
		}
		else
		{
			EffectsManager.Instance.ScreenShake();
		}
		EffectsManager.Instance.CreateInGameEffect("LandWindWirlEffect", ((Component)attacker).transform.position);
	}
}
