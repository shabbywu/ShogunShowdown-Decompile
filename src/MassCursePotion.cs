using System.Collections.Generic;

public class MassCursePotion : Potion
{
	public override PotionsManager.PotionEnum PotionEnum { get; } = PotionsManager.PotionEnum.massCurse;


	public override string LocalizationTableKey { get; } = "Curse";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode> { CombatSceneManager.Mode.combat };


	public override int PriceForHeroSellingPotion { get; } = 2;


	protected override void Effect()
	{
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			enemy.GetMarked();
		}
	}
}
