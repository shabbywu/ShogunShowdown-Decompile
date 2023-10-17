using System.Collections.Generic;

public class CoolUpPotion : Potion
{
	public override PotionsManager.PotionEnum PotionEnum { get; } = PotionsManager.PotionEnum.coolUp;


	public override string LocalizationTableKey { get; } = "CoolUp";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode> { CombatSceneManager.Mode.combat };


	public override int PriceForHeroSellingPotion { get; } = 2;


	protected override void Effect()
	{
		TilesManager.Instance.RechargeCooldownForDeck();
		Globals.Hero.SpecialMove.Cooldown.FullyRecharge();
	}
}
