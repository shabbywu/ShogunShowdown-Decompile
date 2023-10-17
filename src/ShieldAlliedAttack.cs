using System.Collections;
using TileEnums;
using UnityEngine;

public class ShieldAlliedAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.shieldAllied;

	public override string LocalizationTableKey => "ShieldAllied";

	public override int InitialValue => -1;

	public override int InitialCooldown => 0;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "ShieldAlliedAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	protected override bool ClosestTargetOnly { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		Agent[] array = AgentsInRange(attackingAgent);
		foreach (Agent agent in array)
		{
			if (!agent.IsOpponent(attackingAgent) && !agent.HasShield)
			{
				((MonoBehaviour)this).StartCoroutine(PerformAttack(agent));
				return true;
			}
		}
		return false;
	}

	private IEnumerator PerformAttack(Agent target)
	{
		attacker.AttackInProgress = true;
		ShieldAlliedEffect component = EffectsManager.Instance.CreateInGameEffect("ShieldAlliedEffect", ((Component)attacker).transform.position).GetComponent<ShieldAlliedEffect>();
		yield return ((MonoBehaviour)this).StartCoroutine(component.Perform(attacker, target));
		yield return (object)new WaitForSeconds(0.1f);
		attacker.AttackInProgress = false;
	}
}
