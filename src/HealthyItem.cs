using SkillEnums;

public class HealthyItem : Item
{
	private static int healthIncreasePerLevel = 2;

	public override SkillEnum SkillEnum { get; } = SkillEnum.hp_up;


	public override int MaxLevel { get; protected set; } = 10;


	public override string LocalizationTableKey { get; } = "Healthy";


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, healthIncreasePerLevel);
	}

	public override void LevelUp()
	{
		base.LevelUp();
		if (base.CurrentlyHeld)
		{
			Globals.Hero.AddToMaxHealth(healthIncreasePerLevel);
		}
	}

	public override void PickUp()
	{
		base.PickUp();
		Globals.Hero.AddToMaxHealth(healthIncreasePerLevel);
	}

	public override void Remove()
	{
		base.Remove();
		Globals.Hero.AddToMaxHealth(-healthIncreasePerLevel);
	}
}
