using System;
using System.Collections;
using UnityEngine;
using Utils;

public abstract class SpecialMove : MonoBehaviour
{
	public abstract int InitialCooldown { get; }

	public abstract RelativeDir DefaultRelativeDir { get; }

	public SpecialMoveCooldown Cooldown { get; private set; }

	public bool IsEnabled { get; set; } = true;


	public int Damage { get; set; }

	public bool CanDoBackwards { get; set; }

	public bool Curse { get; set; }

	protected bool HasEffectOnTarget
	{
		get
		{
			if (Damage <= 0)
			{
				return Curse;
			}
			return true;
		}
	}

	public abstract bool Allowed(Hero hero, Dir dir);

	public abstract IEnumerator Perform(Hero hero, Dir dir, bool depleteSpecialMoveCooldown = true);

	public void Awake()
	{
		Cooldown = ((Component)this).GetComponentInChildren<SpecialMoveCooldown>();
		Cooldown.Initialize(InitialCooldown);
	}

	protected void ApplyEffectOnTarget(Hero hero, Agent target)
	{
		if (Damage > 0)
		{
			target.ReceiveAttack(new Hit(Damage, isDirectional: true), hero);
			SoundEffectsManager.Instance.Play("CombatHit");
		}
		if (target.IsAlive && Curse)
		{
			target.GetMarked();
		}
	}

	protected IEnumerator ApplyEffectOnTargetAfterPositionCrossing(Hero hero, Agent target)
	{
		int initialSign = Math.Sign(((Component)hero).transform.position.x - ((Component)target).transform.position.x);
		while (true)
		{
			int num = Math.Sign(((Component)hero).transform.position.x - ((Component)target).transform.position.x);
			if (initialSign * num == -1)
			{
				break;
			}
			yield return null;
		}
		ApplyEffectOnTarget(hero, target);
	}
}
