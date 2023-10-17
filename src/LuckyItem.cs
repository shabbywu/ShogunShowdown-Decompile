using SkillEnums;

public class LuckyItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.lucky;


	public override int MaxLevel { get; protected set; } = 10;


	public override string LocalizationTableKey { get; } = "Lucky";


	private float Luck => 0.2f * (float)base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, (int)(100f * Luck));
	}

	public override void LevelUp()
	{
		base.LevelUp();
		if (base.CurrentlyHeld)
		{
			Globals.Hero.Luck = Luck;
		}
	}

	public override void PickUp()
	{
		base.PickUp();
		Globals.Hero.Luck = Luck;
	}

	public override void Remove()
	{
		base.Remove();
		Globals.Hero.Luck -= Luck;
	}
}
