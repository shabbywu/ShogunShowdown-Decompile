using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class DynamicAttackBoostItem : Item
{
	private bool damageBoost;

	public override SkillEnum SkillEnum { get; } = SkillEnum.dynamic_boost;


	public override int MaxLevel { get; protected set; } = 5;


	public override string LocalizationTableKey { get; } = "DynamicBoost";


	private int ExtraDamage => base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, ExtraDamage);
	}

	public override void PickUp()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		base.PickUp();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).AddListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(EndOfCombatTurn));
		EventsManager.Instance.HeroPerformedMoveAttack.AddListener(new UnityAction(HeroPerformedMoveAttack));
	}

	public override void Remove()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		base.Remove();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).RemoveListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
		EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(EndOfCombatTurn));
		EventsManager.Instance.HeroPerformedMoveAttack.RemoveListener(new UnityAction(HeroPerformedMoveAttack));
	}

	private void ProcessAttack(Agent attacker, Agent defender, Hit hit)
	{
		if (!((Object)(object)attacker != (Object)(object)Globals.Hero) && damageBoost)
		{
			hit.Damage += ExtraDamage;
			SoundEffectsManager.Instance.Play("SpecialHit");
		}
	}

	private void EndOfCombatTurn()
	{
		damageBoost = false;
	}

	private void HeroPerformedMoveAttack()
	{
		damageBoost = true;
	}
}
