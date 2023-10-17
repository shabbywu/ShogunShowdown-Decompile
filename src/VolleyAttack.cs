using System.Collections;
using TileEnums;
using UnityEngine;

public class VolleyAttack : Attack
{
	private Cell targetCell;

	private static readonly float volleyEffectDelay = 0.6f;

	private static readonly float damageDelay = 0.1f;

	public override AttackEnum AttackEnum => AttackEnum.volley;

	public override string LocalizationTableKey => "Volley";

	public override int InitialValue => 2;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "VolleyAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[1] { AttackEffectEnum.replay };


	protected override bool IsDirectional { get; set; }

	public static float TimeBeforeHit => volleyEffectDelay + damageDelay;

	public override void AttackDeclared(Agent agent)
	{
		targetCell = Globals.Hero.Cell;
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
		SoundEffectsManager.Instance.PlayAfterDeltaT("VolleyAttackUp", 0.1f);
		yield return (object)new WaitForSeconds(volleyEffectDelay);
		EffectsManager.Instance.CreateInGameEffect("VolleyAttackEffect", ((Component)targetCell).transform.position);
		SoundEffectsManager.Instance.Play("VolleyAttackDown");
		yield return (object)new WaitForSeconds(damageDelay);
		Agent agent = targetCell.Agent;
		if ((Object)(object)agent != (Object)null)
		{
			HitTarget(agent);
		}
		yield return (object)new WaitForSeconds(0.3f);
		attacker.AttackInProgress = false;
	}
}
