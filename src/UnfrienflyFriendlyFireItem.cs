using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class UnfrienflyFriendlyFireItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.unfriendly_fire;


	public override int MaxLevel { get; protected set; } = 5;


	public override string LocalizationTableKey { get; } = "UnfriendlyFire";


	private int ExtraDamage => base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, ExtraDamage);
	}

	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).AddListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).RemoveListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
	}

	private void ProcessAttack(Agent attacker, Agent defender, Hit hit)
	{
		if (!((Object)(object)attacker == (Object)null) && !hit.IsCollision && !attacker.IsOpponent(defender))
		{
			hit.Damage += ExtraDamage;
			SoundEffectsManager.Instance.Play("SpecialHit");
		}
	}
}
