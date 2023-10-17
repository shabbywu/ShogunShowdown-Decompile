using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TileEnums;
using UnityEngine;

public class ScarStrikeAttack : Attack
{
	private List<Agent> targets;

	public override AttackEnum AttackEnum => AttackEnum.scarStrike;

	public override string LocalizationTableKey { get; } = "ScarStrike";


	public override int InitialValue => 1;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override bool IsDirectional { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		targets = CombatManager.Instance.Agents.Where((Agent agent) => agent.IsOpponent(attackingAgent) && !agent.IsAtFullHealth).ToList();
		if (targets.Count == 0)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		SoundEffectsManager.Instance.Play("ScarStrikeAttackBegin");
		foreach (Agent target in targets)
		{
			EffectsManager.Instance.CreateInGameEffect("ScarStrikeEffect", ((Component)target).transform.position);
		}
		yield return (object)new WaitForSeconds(0.4f);
		foreach (Agent target2 in targets)
		{
			HitTarget(target2, "ScarStrikeAttack");
		}
		yield return (object)new WaitForSeconds(0.4f);
		attacker.AttackInProgress = false;
	}
}
