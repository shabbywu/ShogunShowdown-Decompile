public class DecreaseAttackAndCooldownTileUpgrade : TileUpgrade
{
	public int attackDelta;

	public int cooldownDelta;

	public override string Description => string.Format("[reward_color_good]{0} {1}[end_color]\n[reward_color_bad]{2} {3}[end_color]", cooldownDelta, TileUpgrade.LocalizedString("Cooldown"), attackDelta, TileUpgrade.LocalizedString("Damage"));

	private void Start()
	{
	}

	public override void Upgrade(Tile tile)
	{
		tile.Attack.Value += attackDelta;
		tile.Attack.BaseValue += attackDelta;
		tile.Attack.Cooldown += cooldownDelta;
		tile.CooldownCharge = tile.Attack.Cooldown;
		tile.Attack.Level++;
		tile.Graphics.UpdateGraphics();
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		if (tile.Attack.Cooldown + cooldownDelta <= Attack.maxCooldown && tile.Attack.Level < tile.Attack.MaxLevel && tile.Attack.HasValue)
		{
			return tile.Attack.Value + attackDelta >= 0;
		}
		return false;
	}

	public override string CannotUpgradeText(Tile tile)
	{
		if (tile.Attack.Level >= tile.Attack.MaxLevel)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_NoEmptyLevel");
		}
		if (!tile.Attack.HasValue)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_NoAttack");
		}
		if (tile.Attack.Value + attackDelta < 0)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MinAttack");
		}
		return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MaxCooldown");
	}
}
