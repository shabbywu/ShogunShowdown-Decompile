using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class ComboHealItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.combo_heal;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ComboHeal";


	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)UponComboKill);
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).RemoveListener((UnityAction<Enemy>)UponComboKill);
	}

	private void UponComboKill(Enemy enemy)
	{
		if (CombatManager.Instance.KillStreak >= 3)
		{
			Globals.Hero.AddToHealth(1);
			EffectsManager.Instance.CreateInGameEffect("HealEffect", ((Component)Globals.Hero.AgentGraphics).transform);
		}
	}
}
