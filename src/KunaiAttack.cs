using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class KunaiAttack : Attack
{
	private static readonly int kunaiRandomDeltaYMagnitudeInPixels = 3;

	private static readonly float deltaTBetweenKunais = 0.25f;

	public override AttackEnum AttackEnum => AttackEnum.kunai;

	public override string LocalizationTableKey { get; } = "Kunai";


	public override int InitialValue => 2;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "";


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
		int nKunai = base.Value;
		for (int i = 0; i < nKunai; i++)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(ThrowKunai());
		}
		yield return (object)new WaitForSeconds(0.1f);
		attacker.AttackInProgress = false;
	}

	private IEnumerator ThrowKunai()
	{
		Agent target = AgentInRange(attacker);
		attacker.Animator.SetTrigger("ProjectileThrow");
		yield return (object)new WaitForSeconds(0.1f);
		ProjectileEffect projectile = EffectsManager.Instance.CreateInGameEffect("KunaiProjectileEffect", ((Component)attacker).transform.position).GetComponent<ProjectileEffect>();
		Vector3 targetPosition = (((Object)(object)target != (Object)null) ? ((Component)target).transform.position : (((Component)attacker).transform.position + 20f * DirUtils.ToVec(attacker.FacingDir)));
		projectile.Throw(((Component)attacker).transform.position, targetPosition, kunaiRandomDeltaYMagnitudeInPixels);
		if ((Object)(object)target != (Object)null)
		{
			float deltaT2 = Time.time;
			while (projectile.MovingTowardsTargetPosition)
			{
				yield return null;
			}
			deltaT2 = Time.time - deltaT2;
			int value = base.Value;
			base.Value = 1;
			HitTarget(target);
			base.Value = value;
			yield return (object)new WaitForSeconds(Mathf.Max(0f, deltaTBetweenKunais - deltaT2));
		}
		else
		{
			yield return (object)new WaitForSeconds(deltaTBetweenKunais);
		}
	}
}
