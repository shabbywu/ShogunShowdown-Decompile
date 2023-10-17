public class DecreaseCooldownTileUpgrade : TileUpgrade
{
	public int cooldownDelta;

	public override string Description => string.Format("[reward_color_good]{0} {1}[end_color]", cooldownDelta, TileUpgrade.LocalizedString("Cooldown"));

	public override void Upgrade(Tile tile)
	{
		tile.Attack.Cooldown += cooldownDelta;
		tile.CooldownCharge = tile.Attack.Cooldown;
		tile.Attack.Level++;
		tile.Graphics.UpdateGraphics();
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		if (tile.Attack.Cooldown > 0)
		{
			return tile.Attack.Level < tile.Attack.MaxLevel;
		}
		return false;
	}

	public override string CannotUpgradeText(Tile tile)
	{
		if (tile.Attack.Level >= tile.Attack.MaxLevel)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_NoEmptyLevel");
		}
		return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MinCooldown");
	}
}
