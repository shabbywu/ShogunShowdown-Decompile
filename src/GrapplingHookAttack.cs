using System.Collections;
using TileEnums;
using UnityEngine;

public class GrapplingHookAttack : Attack
{
	private readonly float fastSpeed = 16f;

	private readonly float mediumSpeed = 13f;

	private readonly float slowSpeed = 10f;

	private readonly float agentThrowAnimationTime = 0.25f;

	private readonly float impactPauseTime = 0.15f;

	private readonly Vector3 relativeGrapplingHookPosition = new Vector3(0.2f, 0f, 0f);

	private TetheredWeaponEffect grapplingHook;

	public override AttackEnum AttackEnum => AttackEnum.grapplingHook;

	public override string LocalizationTableKey { get; } = "GrapplingHook";


	public override int InitialValue => 1;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "GrapplingHookAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[3]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		if ((Object)(object)AgentInRange(attacker) == (Object)null)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		Agent target = AgentInRange(attacker);
		yield return (object)new WaitForSeconds(agentThrowAnimationTime);
		grapplingHook = EffectsManager.Instance.CreateInGameEffect("GrapplingHoockEffect", ((Component)attacker.AgentGraphics).transform).GetComponent<TetheredWeaponEffect>();
		((Component)grapplingHook).transform.localPosition = relativeGrapplingHookPosition;
		SoundEffectsManager.Instance.Play("MissHit");
		float throwDistance = Mathf.Abs(((Component)grapplingHook).transform.position.x - ((Component)target).transform.position.x);
		yield return grapplingHook.PerformWeaponMove(0f, throwDistance, fastSpeed);
		HitTarget(target);
		yield return (object)new WaitForSeconds(impactPauseTime);
		bool num = target.IsAlive && target.Movable;
		float speed = (num ? slowSpeed : mediumSpeed);
		if (num)
		{
			Cell targetCell = attacker.Cell.Neighbours[attacker.FacingDir];
			target.ImposedMovement(targetCell, speed, 0f, createDustEffect: true);
		}
		yield return grapplingHook.PerformWeaponMove(throwDistance, 0f, speed);
		grapplingHook.DisappearAndDestroy();
		attacker.AttackInProgress = false;
	}

	private void OnDisable()
	{
		if ((Object)(object)grapplingHook != (Object)null)
		{
			grapplingHook.DisappearAndDestroy();
		}
	}
}
