using SkillEnums;
using UnityEngine.Events;

public class ReactiveShieldItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.reactive_shield;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ReactiveShield";


	public override void PickUp()
	{
		base.PickUp();
		EventsManager.Instance.HeroTookDamage.AddListener((UnityAction<int>)UponHeroTookDamage);
	}

	public override void Remove()
	{
		base.Remove();
		EventsManager.Instance.HeroTookDamage.RemoveListener((UnityAction<int>)UponHeroTookDamage);
	}

	private void UponHeroTookDamage(int damage)
	{
		if (damage > 0 && Globals.Hero.IsAlive && !Globals.Hero.HasShield)
		{
			Globals.Hero.AddShield();
		}
	}
}
