using System.Collections;
using System.Linq;
using Parameters;
using TileEnums;
using UnityEngine;

public class MeteorHammerAttack : Attack
{
	private readonly float slowSpeed = 15f;

	private readonly float fastSpeed = 18f;

	private readonly float accellerationTime = 0.2f;

	private readonly float impactPauseTime = 0.2f;

	private readonly float agentThrowAnimationTime = 0.25f;

	private readonly Vector3 relativeMeteorHammerPosition = new Vector3(0.1f, 0f, 0f);

	public override AttackEnum AttackEnum => AttackEnum.meteorHammer;

	public override string LocalizationTableKey { get; } = "MeteorHammer";


	public override int InitialValue => 2;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[3] { 1, 2, 3 };


	public override string AnimationTrigger { get; protected set; } = "MeteorHammerAttack";


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

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		yield return (object)new WaitForSeconds(agentThrowAnimationTime);
		TetheredWeaponEffect meteorHammer = EffectsManager.Instance.CreateInGameEffect("MeteorHammerEffect", ((Component)attacker.AgentGraphics).transform).GetComponent<TetheredWeaponEffect>();
		((Component)meteorHammer).transform.localPosition = relativeMeteorHammerPosition;
		SoundEffectsManager.Instance.Play("Chain");
		Agent targetInFront = AgentInRange(attacker);
		float throwDistance = (((Object)(object)targetInFront == (Object)null) ? ((float)Range.Max() * TechParams.effectiveGridCellSize) : Mathf.Abs(((Component)meteorHammer).transform.position.x - ((Component)targetInFront).transform.position.x));
		yield return meteorHammer.PerformWeaponMove(0f, throwDistance, slowSpeed, accellerationTime);
		if ((Object)(object)targetInFront != (Object)null)
		{
			HitTarget(targetInFront);
		}
		yield return (object)new WaitForSeconds(impactPauseTime);
		SoundEffectsManager.Instance.Play("Chain");
		if ((Object)(object)targetInFront != (Object)null)
		{
			Agent targetBehind = (((Object)(object)attacker.CellBehind != (Object)null) ? attacker.CellBehind.Agent : null);
			float ricochetDistance = (((Object)(object)targetBehind == (Object)null) ? (-1.1f * TechParams.effectiveGridCellSize) : (0f - Mathf.Abs(((Component)meteorHammer).transform.position.x - ((Component)targetBehind).transform.position.x)));
			((MonoBehaviour)this).StartCoroutine(PirouetteAnimationAfterDelay(Mathf.Max(0f, throwDistance / fastSpeed - 0.1f)));
			yield return meteorHammer.PerformWeaponMove(throwDistance, ricochetDistance, fastSpeed, accellerationTime);
			if ((Object)(object)targetBehind != (Object)null)
			{
				HitTarget(targetBehind);
			}
			yield return (object)new WaitForSeconds(impactPauseTime);
			yield return meteorHammer.PerformWeaponMove(ricochetDistance, 0f, fastSpeed, accellerationTime);
		}
		else
		{
			yield return meteorHammer.PerformWeaponMove(throwDistance, 0f, fastSpeed, accellerationTime);
		}
		meteorHammer.DisappearAndDestroy();
		attacker.AttackInProgress = false;
	}

	private IEnumerator PirouetteAnimationAfterDelay(float delay)
	{
		yield return (object)new WaitForSeconds(delay);
		attacker.Animator.SetTrigger("SwapTossAttack");
	}
}
