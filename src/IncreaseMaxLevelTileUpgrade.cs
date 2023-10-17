using System.Collections.Generic;
using System.Linq;
using Parameters;

public class IncreaseMaxLevelTileUpgrade : TileUpgrade
{
	public int levelDelta;

	public override string Description => string.Format("[reward_color_good]+{0} {1}[end_color]", levelDelta, TileUpgrade.LocalizedString("MaxLevel"));

	public override void Upgrade(Tile tile)
	{
		tile.Attack.MaxLevel += levelDelta;
		tile.Graphics.UpdateGraphics();
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		return tile.Attack.MaxLevel + levelDelta <= Attack.maxMaxLevel;
	}

	public override bool CanBeOfferedGivenThisDeck(List<Tile> deck)
	{
		if (!base.CanBeOfferedGivenThisDeck(deck))
		{
			return false;
		}
		return deck.Sum((Tile tile) => tile.Attack.Level) >= GameParams.totalLevelsBeforeOfferingIncreaseMaxLevelTileUpgrade;
	}

	public override string CannotUpgradeText(Tile tile)
	{
		return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MaxLevel");
	}
}
