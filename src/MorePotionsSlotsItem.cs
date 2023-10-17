using SkillEnums;

public class MorePotionsSlotsItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.extra_consumable_slot;


	public override int MaxLevel { get; protected set; } = 3;


	public override string LocalizationTableKey { get; } = "BigPockets";


	public override void LevelUp()
	{
		base.LevelUp();
		if (base.CurrentlyHeld)
		{
			PotionsManager.Instance.NPotionsSlots++;
		}
	}

	public override void PickUp()
	{
		base.PickUp();
		PotionsManager.Instance.NPotionsSlots++;
	}

	public override void Remove()
	{
		base.Remove();
		PotionsManager.Instance.NPotionsSlots -= base.Level;
	}
}
