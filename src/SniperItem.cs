using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class SniperItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.sniper;


	public override int MaxLevel { get; protected set; } = 10;


	public override string LocalizationTableKey { get; } = "Sniper";


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
		if (!((Object)(object)attacker != (Object)(object)Globals.Hero) && attacker.Cell.Distance(defender.Cell) >= 4)
		{
			hit.Damage += ExtraDamage;
			SoundEffectsManager.Instance.Play("SpecialHit");
		}
	}
}
