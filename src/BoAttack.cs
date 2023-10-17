using TileEnums;
using UnityEngine;

public class BoAttack : Attack
{
	private Agent target;

	public override AttackEnum AttackEnum => AttackEnum.bo;

	public override string LocalizationTableKey { get; } = "Bo";


	public override int InitialValue => 1;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "BoAttack";


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
		target = AgentInRange(attacker);
		if ((Object)(object)target == (Object)null)
		{
			SoundEffectsManager.Instance.Play("MissHit");
			return;
		}
		HitTarget(target);
		if (target.Movable)
		{
			FlipTarget(target);
		}
	}

	private void FlipTarget(Agent target)
	{
		if (target.AgentStats.ice == 0)
		{
			target.RegisterActionInProgress(Agent.turnAroundTime);
			target.TurnAround();
		}
		else
		{
			target.Flip();
		}
	}

	public override bool WaitingForSomethingToFinish()
	{
		if ((Object)(object)target == (Object)null)
		{
			return false;
		}
		if (!target.IsAlive)
		{
			return false;
		}
		return target.ActionInProgress;
	}
}
