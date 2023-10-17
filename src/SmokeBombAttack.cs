using System.Collections;
using TileEnums;
using UnityEngine;

public class SmokeBombAttack : Attack
{
	private static float preSwapTime = 0.05f;

	private static float postSwapTime = 0.3f;

	public override AttackEnum AttackEnum => AttackEnum.smokeBomb;

	public override string LocalizationTableKey => "SmokeBomb";

	public override int InitialValue => 1;

	public override int InitialCooldown => 5;

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


	protected override bool IsDirectional { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		Agent[] array = AgentsInRange(attacker);
		if (array.Length == 0)
		{
			return false;
		}
		if (!array[0].Movable)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(PerformSmokeBombAttack(array[0]));
		return true;
	}

	private IEnumerator PerformSmokeBombAttack(Agent target)
	{
		attacker.AttackInProgress = true;
		EffectsManager.Instance.CreateInGameEffect("SmokeBombEffect", ((Component)attacker.Cell).transform.position);
		EffectsManager.Instance.CreateInGameEffect("SmokeBombEffect", ((Component)target.Cell).transform.position);
		yield return (object)new WaitForSeconds(preSwapTime);
		Cell cell = target.Cell;
		Cell cell2 = attacker.Cell;
		attacker.Cell = cell;
		target.Cell = cell2;
		((Component)attacker).transform.position = ((Component)attacker.Cell).transform.position;
		((Component)target).transform.position = ((Component)target.Cell).transform.position;
		attacker.Animator.SetTrigger("TriggerIdle");
		HitTarget(target);
		yield return (object)new WaitForSeconds(postSwapTime);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.AttackInProgress = false;
	}
}
