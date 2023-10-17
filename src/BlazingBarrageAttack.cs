using System.Collections;
using TileEnums;
using UnityEngine;

public class BlazingBarrageAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.blazingBarrage;

	public override string LocalizationTableKey { get; } = "BlazingBarrage";


	public override int InitialValue => 3;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[2] { -1, 1 };


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	protected override bool IsDirectional { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		Object.Instantiate<GameObject>(Resources.Load<GameObject>("Combat/CombatObjects/BlazingBarrage"), ((Component)attacker).transform.position, Quaternion.identity).GetComponent<BlazingBarrage>().Initialize(base.Value, attacker.Cell, attacker);
		yield return (object)new WaitForSeconds(0.2f);
		attacker.AttackInProgress = false;
	}
}
