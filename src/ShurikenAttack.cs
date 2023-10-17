using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class ShurikenAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.shuriken;

	public override string LocalizationTableKey => "Shuriken";

	public override int InitialValue => 1;

	public override int InitialCooldown => 3;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "ProjectileThrow";


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
		Agent target = AgentInRange(attacker);
		yield return (object)new WaitForSeconds(0.1f);
		ProjectileEffect projectile = EffectsManager.Instance.CreateInGameEffect("ShurikenProjectileEffect", ((Component)attacker).transform.position).GetComponent<ProjectileEffect>();
		Vector3 targetPosition = (((Object)(object)target != (Object)null) ? ((Component)target).transform.position : (((Component)attacker).transform.position + 20f * DirUtils.ToVec(attacker.FacingDir)));
		projectile.Throw(((Component)attacker).transform.position, targetPosition);
		if ((Object)(object)target != (Object)null)
		{
			while (projectile.MovingTowardsTargetPosition)
			{
				yield return null;
			}
			HitTarget(target);
		}
		else
		{
			yield return (object)new WaitForSeconds(0.2f);
		}
		yield return (object)new WaitForSeconds(0.1f);
		attacker.AttackInProgress = false;
	}
}
