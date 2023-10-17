using System.Linq;
using TileEnums;

public class AddAttackEffectTileUpgrade : TileUpgrade
{
	public AttackEffectEnum effect;

	public int cooldownDelta;

	private string Name => TileEnumsUtils.LocalizedAttackEffectName(effect) ?? "";

	public override string Description => string.Format("{0}\n[reward_color_bad]+{1} {2}[end_color]", Name, cooldownDelta, TileUpgrade.LocalizedString("Cooldown"));

	public override string Details => TileEnumsUtils.LocalizedAttackEffectName(effect) + ": " + TileEnumsUtils.LocalizedAttackEffectDescription(effect);

	public override void Upgrade(Tile tile)
	{
		tile.Attack.AttackEffect = effect;
		tile.Attack.Cooldown += cooldownDelta;
		tile.CooldownCharge = tile.Attack.Cooldown;
		tile.Attack.Level++;
		tile.Graphics.UpdateGraphics();
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		if (tile.Attack.AttackEffect == AttackEffectEnum.none && tile.Attack.CompatibleEffects.Contains(effect) && tile.Attack.Cooldown + cooldownDelta <= Attack.maxCooldown)
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
		if (tile.Attack.AttackEffect != 0)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_HasAttackEffect");
		}
		if (!tile.Attack.CompatibleEffects.Contains(effect))
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_EffectNotCompatible");
		}
		if (tile.Attack.Cooldown + cooldownDelta > Attack.maxCooldown)
		{
			return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_MaxCooldown");
		}
		return "AddAttackEffectUpgrade:CannotUpgradeText ERROR!";
	}
}
