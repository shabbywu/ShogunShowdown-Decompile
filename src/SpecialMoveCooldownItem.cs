using SkillEnums;
using UnityEngine;

public class SpecialMoveCooldownItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.special_move_cooldown;


	public override int MaxLevel { get; protected set; } = 3;


	public override string LocalizationTableKey { get; } = "SpecialMoveCooldown";


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, base.Level, Globals.Hero.SpecialAbilityName);
	}

	public override void LevelUp()
	{
		base.LevelUp();
		Process();
	}

	public override void PickUp()
	{
		base.PickUp();
		Process();
	}

	public override void Remove()
	{
		base.Remove();
		Debug.LogWarning((object)"Removal of SpecialMoveCooldownItem not implemented");
	}

	private void Process()
	{
		Globals.Hero.SpecialMove.Cooldown.Cooldown--;
	}
}
