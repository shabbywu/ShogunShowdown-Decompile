using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class ShieldAtBeginningOfCombatItem : Item
{
	private static int hpDecrease = 2;

	public override SkillEnum SkillEnum { get; } = SkillEnum.fortress;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "Fortress";


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, hpDecrease);
	}

	public override void PickUp()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		base.PickUp();
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(BeginningOfCombat));
		Globals.Hero.SetMaxHealth(Mathf.Max(Globals.Hero.AgentStats.maxHP - hpDecrease, 1));
	}

	public override void Remove()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		base.Remove();
		EventsManager.Instance.BeginningOfCombat.RemoveListener(new UnityAction(BeginningOfCombat));
	}

	private void BeginningOfCombat()
	{
		if (!CombatSceneManager.Instance.Room.RoomWasLoadedFromSaveData)
		{
			Globals.Hero.AddShield();
		}
	}
}
