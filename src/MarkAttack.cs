using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class MarkAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.mark;

	public override string LocalizationTableKey { get; } = "Curse";


	public override int InitialValue => -1;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "ProjectileThrow";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void Initialize(int maxLevel)
	{
		base.Initialize(maxLevel);
		base.TileEffect = TileEffectEnum.freePlay;
	}

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
		ProjectileEffect projectile = EffectsManager.Instance.CreateInGameEffect("MarkProjectileEffect", ((Component)attacker).transform.position).GetComponent<ProjectileEffect>();
		Vector3 targetPosition = (((Object)(object)target != (Object)null) ? ((Component)target).transform.position : (((Component)attacker).transform.position + 20f * DirUtils.ToVec(attacker.FacingDir)));
		projectile.Throw(((Component)attacker).transform.position, targetPosition);
		if ((Object)(object)target != (Object)null)
		{
			while (projectile.MovingTowardsTargetPosition)
			{
				yield return null;
			}
			SoundEffectsManager.Instance.Play("CombatHit");
			if (!target.AgentStats.mark)
			{
				target.GetMarked();
				yield return (object)new WaitForSeconds(0.2f);
			}
		}
		else
		{
			yield return (object)new WaitForSeconds(0.2f);
		}
		yield return (object)new WaitForSeconds(0.1f);
		attacker.AttackInProgress = false;
	}
}
