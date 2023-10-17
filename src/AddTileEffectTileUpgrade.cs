using TileEnums;

public class AddTileEffectTileUpgrade : TileUpgrade
{
	public TileEffectEnum effect;

	public int cooldownDelta;

	private string Name => TileEnumsUtils.LocalizedTileEffectName(effect);

	public override string Details => TileEnumsUtils.LocalizedTileEffectDescription(effect);

	public override string Description => string.Format("{0}\n[reward_color_bad]+{1} {2}[end_color]", Name, cooldownDelta, TileUpgrade.LocalizedString("Cooldown"));

	public override void Upgrade(Tile tile)
	{
		tile.Attack.TileEffect = effect;
		tile.Attack.Cooldown += cooldownDelta;
		tile.CooldownCharge = tile.Attack.Cooldown;
		tile.Attack.Level++;
		tile.Graphics.UpdateGraphics();
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		if (tile.Attack.TileEffect == TileEffectEnum.none && tile.Attack.Cooldown + cooldownDelta <= Attack.maxCooldown)
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
		if (tile.Attack.TileEffect != 0)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_HasTileEffect");
		}
		if (tile.Attack.Cooldown + cooldownDelta > Attack.maxCooldown)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MaxCooldown");
		}
		return "AddTileEffectUpgrade:CannotUpgradeText ERROR!";
	}
}
