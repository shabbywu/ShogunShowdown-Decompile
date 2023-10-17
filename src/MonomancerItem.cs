using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class MonomancerItem : Item
{
	private bool shouldDealExtraDamage;

	public override SkillEnum SkillEnum { get; } = SkillEnum.monomancer;


	public override int MaxLevel { get; protected set; } = 5;


	public override string LocalizationTableKey { get; } = "Monomancer";


	private int ExtraDamage => base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, ExtraDamage);
	}

	public override void PickUp()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		base.PickUp();
		EventsManager.Instance.HeroAttacks.AddListener((UnityAction<AttackQueue>)OnHeroAttacks);
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(OnEndOfCombatTurn));
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).AddListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
	}

	private void OnHeroAttacks(AttackQueue attackQueue)
	{
		shouldDealExtraDamage = attackQueue.NTiles == 1;
	}

	private void OnEndOfCombatTurn()
	{
		shouldDealExtraDamage = false;
	}

	private void ProcessAttack(Agent attacker, Agent defender, Hit hit)
	{
		if (!((Object)(object)attacker != (Object)(object)Globals.Hero) && shouldDealExtraDamage)
		{
			hit.Damage += ExtraDamage;
			SoundEffectsManager.Instance.Play("SpecialHit");
		}
	}
}
