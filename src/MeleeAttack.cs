using TileEnums;
using UnityEngine;

public class MeleeAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.sword;

	public override string LocalizationTableKey => "Sword";

	public override int InitialValue => 2;

	public override int InitialCooldown => 1;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "MeleeAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	public override void ApplyEffect()
	{
		Agent agent = AgentInRange(attacker);
		if ((Object)(object)agent == (Object)null)
		{
			SoundEffectsManager.Instance.Play("MissHit");
		}
		else
		{
			HitTarget(agent);
		}
	}
}
