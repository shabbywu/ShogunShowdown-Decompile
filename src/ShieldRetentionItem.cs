using SkillEnums;

public class ShieldRetentionItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.shield_retention;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ShieldRetention";


	public override void PickUp()
	{
		base.PickUp();
		Globals.Hero.RemoveShieldWhenExitCombatMode = false;
	}

	public override void Remove()
	{
		base.Remove();
		Globals.Hero.RemoveShieldWhenExitCombatMode = true;
	}
}
