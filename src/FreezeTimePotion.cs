using System.Collections.Generic;
using Parameters;

public class FreezeTimePotion : Potion
{
	public override PotionsManager.PotionEnum PotionEnum { get; } = PotionsManager.PotionEnum.freezeTime;


	public override string LocalizationTableKey { get; } = "Ice";


	public override List<CombatSceneManager.Mode> AllowedModes { get; } = new List<CombatSceneManager.Mode> { CombatSceneManager.Mode.combat };


	public override int PriceForHeroSellingPotion { get; } = 2;


	protected override void Effect()
	{
		foreach (Enemy enemy in CombatManager.Instance.Enemies)
		{
			enemy.Freeze(GameParams.iceEffectTurnsDuration);
		}
	}
}
