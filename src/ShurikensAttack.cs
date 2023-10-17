using System.Collections;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;
using Utils;

public class ShurikensAttack : Attack
{
	private int attacksCounter;

	public override AttackEnum AttackEnum => AttackEnum.shurikens;

	public override string LocalizationTableKey { get; } = "NotImplemented";


	public override int InitialValue => 1;

	public override int InitialCooldown => 4;

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

	public override Agent[] AgentsInRange(Agent attackingAgent)
	{
		List<Agent> list = new List<Agent>();
		for (int i = 1; i < Globals.MaxRoomSize; i++)
		{
			Cell cell = attackingAgent.Cell.Neighbour(attackingAgent.FacingDir, i);
			if ((Object)(object)cell != (Object)null && (Object)(object)cell.Agent != (Object)null)
			{
				list.Add(cell.Agent);
				break;
			}
		}
		for (int j = 1; j < Globals.MaxRoomSize; j++)
		{
			Cell cell2 = attackingAgent.Cell.Neighbour(DirUtils.Opposite(attackingAgent.FacingDir), j);
			if ((Object)(object)cell2 != (Object)null && (Object)(object)cell2.Agent != (Object)null)
			{
				list.Add(cell2.Agent);
				break;
			}
		}
		return list.ToArray();
	}

	private IEnumerator PerformSingleAttack(Dir direction, Agent target)
	{
		yield return (object)new WaitForSeconds(0.1f);
		ProjectileEffect projectile = EffectsManager.Instance.CreateInGameEffect("ShurikenProjectileEffect", ((Component)attacker).transform.position).GetComponent<ProjectileEffect>();
		Vector3 targetPosition = (((Object)(object)target != (Object)null) ? ((Component)target).transform.position : (((Component)attacker).transform.position + 20f * DirUtils.ToVec(direction)));
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
		attacksCounter--;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		Agent[] targets = AgentsInRange(attacker);
		attacksCounter = 2;
		Agent target = TargetInDirection(attacker.FacingDir, attacker, targets);
		Agent target2 = TargetInDirection(DirUtils.Opposite(attacker.FacingDir), attacker, targets);
		((MonoBehaviour)this).StartCoroutine(PerformSingleAttack(attacker.FacingDir, target));
		((MonoBehaviour)this).StartCoroutine(PerformSingleAttack(DirUtils.Opposite(attacker.FacingDir), target2));
		while (attacksCounter > 0)
		{
			yield return null;
		}
		yield return (object)new WaitForSeconds(0.2f);
		attacker.AttackInProgress = false;
	}

	private Agent TargetInDirection(Dir dir, Agent attaker, Agent[] targets)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		foreach (Agent agent in targets)
		{
			if (Vector3.Dot(((Component)agent).transform.position - ((Component)attaker).transform.position, DirUtils.ToVec(dir)) > 0f)
			{
				return agent;
			}
		}
		return null;
	}
}
