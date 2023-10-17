using TileEnums;
using UnityEngine;

public class TetsuboAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.tetsubo;

	public override string LocalizationTableKey => "Tetsubo";

	public override int InitialValue => 4;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "TetsuboAttack";


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
