using TileEnums;
using UnityEngine;

public class ShadowKamaAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.shadowKama;

	public override string LocalizationTableKey => "ShadowKama";

	public override int InitialValue => 3;

	public override int InitialCooldown => 2;

	public override int[] Range { get; protected set; } = new int[1] { 2 };


	public override string AnimationTrigger { get; protected set; } = "ShadowKama";


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
		if ((Object)(object)attacker.Cell.Neighbour(attacker.FacingDir, Range[0]) == (Object)null)
		{
			return false;
		}
		SoundEffectsManager.Instance.PlayAfterDeltaT("ShadowKama", 0.05f);
		return true;
	}

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
