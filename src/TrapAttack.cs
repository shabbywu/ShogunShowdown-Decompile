using TileEnums;
using UnityEngine;

public class TrapAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.trap;

	public override string LocalizationTableKey => "Trap";

	public override int InitialValue => 3;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[4]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison
	};


	public override bool Begin(Agent attackingAgent)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		base.Begin(attackingAgent);
		Cell cell = attacker.Cell.Neighbour(attacker.FacingDir, 1);
		if ((Object)(object)cell == (Object)null)
		{
			return false;
		}
		SoundEffectsManager.Instance.Play("TrapPlacing");
		Object.Instantiate<GameObject>(Resources.Load<GameObject>("Combat/CombatObjects/Trap"), ((Component)attacker).transform.position, Quaternion.identity).GetComponent<Trap>().Initialize(base.Value, base.AttackEffect, ((Component)attacker).transform.position, ((Component)cell).transform.position);
		attacker.RegisterAttackInProgress(Trap.placingTime);
		return true;
	}
}
