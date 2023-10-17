using TileEnums;
using UnityEngine;

public class BackStrikeAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.backStrike;

	public override string LocalizationTableKey { get; } = "BackStrike";


	public override int InitialValue => 3;

	public override int InitialCooldown => 3;

	public override int[] Range { get; protected set; } = new int[1] { -1 };


	public override string AnimationTrigger { get; protected set; } = "BackStrike";


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
