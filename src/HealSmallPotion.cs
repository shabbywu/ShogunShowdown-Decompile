using System.Collections.Generic;
using UnityEngine;

public class HealSmallPotion : Potion
{
	public static int sellingPrice = 4;

	private static int heal = 3;

	public override PotionsManager.PotionEnum PotionEnum { get; }

	public override string LocalizationTableKey { get; } = "Heal";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode>
	{
		CombatSceneManager.Mode.combat,
		CombatSceneManager.Mode.reward
	};


	public override int PriceForHeroSellingPotion { get; } = sellingPrice;


	protected override string ProcessDescription(string description)
	{
		return string.Format(description, heal);
	}

	protected override void Effect()
	{
		EffectsManager.Instance.CreateInGameEffect("HealEffect", ((Component)Globals.Hero.AgentGraphics).transform);
		Globals.Hero.AddToHealth(heal);
	}
}
