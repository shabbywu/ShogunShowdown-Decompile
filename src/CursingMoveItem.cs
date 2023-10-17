using SkillEnums;

public class CursingMoveItem : Item
{
	private readonly int cooldownIncrease = 1;

	public override SkillEnum SkillEnum { get; } = SkillEnum.cursing_move;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "CursingMove";


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, Globals.Hero.SpecialAbilityName, cooldownIncrease);
	}

	public override void PickUp()
	{
		base.PickUp();
		Globals.Hero.SpecialMove.Curse = true;
		Globals.Hero.SpecialMove.Cooldown.Cooldown += cooldownIncrease;
	}
}
