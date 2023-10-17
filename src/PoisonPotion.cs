using System.Collections.Generic;
using Parameters;

public class PoisonPotion : Potion
{
	public override PotionsManager.PotionEnum PotionEnum { get; } = PotionsManager.PotionEnum.poison;


	public override string LocalizationTableKey { get; } = "Poison";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode> { CombatSceneManager.Mode.combat };


	public override int PriceForHeroSellingPotion { get; } = 2;


	protected override void Effect()
	{
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			enemy.ApplyPoisonEffect(GameParams.poisonEffectTurnsDuration);
		}
	}
}
