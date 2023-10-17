using System.Collections.Generic;

public class ShieldPotion : Potion
{
	public override PotionsManager.PotionEnum PotionEnum { get; } = PotionsManager.PotionEnum.shield;


	public override string LocalizationTableKey { get; } = "Shield";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode> { CombatSceneManager.Mode.combat };


	public override int PriceForHeroSellingPotion { get; } = 2;


	protected override void Effect()
	{
		Globals.Hero.AddShield();
	}
}
