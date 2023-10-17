using SkillEnums;
using UnityEngine;

public class SpecialMoveBackwardsItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.special_move_backwards;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "SpecialMoveBackwards";


	private int CooldownIncrease { get; } = 1;


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, Globals.Hero.SpecialAbilityName, CooldownIncrease);
	}

	public override void PickUp()
	{
		base.PickUp();
		Globals.Hero.SpecialMove.CanDoBackwards = true;
		Globals.Hero.SpecialMove.Cooldown.Cooldown += CooldownIncrease;
	}

	public override void Remove()
	{
		base.Remove();
		Debug.LogWarning((object)"Removal of SpecialMoveDamageItem not implemented");
	}
}
