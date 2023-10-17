using System.Collections;
using TileEnums;
using UnityEngine;

public class DragonPunchAttack : Attack
{
	private Agent target;

	private bool inProgress;

	public override AttackEnum AttackEnum => AttackEnum.dragonPunch;

	public override string LocalizationTableKey { get; } = "DragonPunch";


	public override int InitialValue => 1;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "DragonPunch";


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
		inProgress = false;
		target = AgentInRange(attacker);
		if ((Object)(object)target == (Object)null)
		{
			SoundEffectsManager.Instance.Play("MissHit");
			return;
		}
		HitTarget(target);
		if (target.Movable)
		{
			((MonoBehaviour)this).StartCoroutine(PerformAttack(target));
		}
	}

	private IEnumerator PerformAttack(Agent target)
	{
		inProgress = true;
		yield return ((MonoBehaviour)this).StartCoroutine(target.Pushed(attacker.FacingDir));
		inProgress = false;
	}

	public override bool WaitingForSomethingToFinish()
	{
		return inProgress;
	}
}
