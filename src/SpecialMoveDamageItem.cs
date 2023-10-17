using SkillEnums;
using UnityEngine;

public class SpecialMoveDamageItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.special_move_damage;


	public override int MaxLevel { get; protected set; } = 5;


	public override string LocalizationTableKey { get; } = "SpecialMoveDamage";


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, Globals.Hero.SpecialAbilityName, base.Level, base.Level);
	}

	public override void PickUp()
	{
		base.PickUp();
		Process();
	}

	public override void LevelUp()
	{
		base.LevelUp();
		Process();
	}

	public override void Remove()
	{
		base.Remove();
		Debug.LogWarning((object)"Removal of SpecialMoveDamageItem not implemented");
	}

	private void Process()
	{
		Globals.Hero.SpecialMove.Damage++;
		Globals.Hero.SpecialMove.Cooldown.Cooldown++;
	}
}
