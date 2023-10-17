using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class KarmaItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.karma;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "Karma";


	public override void PickUp()
	{
		base.PickUp();
		EventsManager.Instance.HeroIsHit.AddListener((UnityAction<(Hit, Agent)>)OnHeroIsHit);
	}

	private void OnHeroIsHit((Hit hit, Agent attacker) value)
	{
		if (!((Object)(object)value.attacker == (Object)null) && value.hit.Damage != 0)
		{
			value.attacker.ReceiveAttack(value.hit.Clone(), null);
			SoundEffectsManager.Instance.Play("SpecialHit");
		}
	}
}
