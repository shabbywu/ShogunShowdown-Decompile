using System.Collections.Generic;
using Parameters;
using UnityEngine;

public class SacrificeTileUpgrade : TileUpgrade
{
	[SerializeField]
	private int nCoins;

	public override string Description
	{
		get
		{
			string text = string.Format(TileUpgrade.LocalizedString("GetMoney"), nCoins);
			return "[reward_color_bad]" + TileUpgrade.LocalizedString("Sacrifice") + "[end_color]\n[reward_color_good]" + text + "[end_color]";
		}
	}

	public override string ButtonText => TileUpgrade.LocalizedString("SacrificeButton");

	public override void Upgrade(Tile tile)
	{
		tile.TileContainer.RemoveTile();
		TilesManager.Instance.Deck.Remove(tile);
		Object.Destroy((Object)(object)((Component)tile).gameObject);
		TilesManager.Instance.hand.Resize(TilesManager.Instance.Deck.Count);
		Globals.Coins += nCoins;
		SoundEffectsManager.Instance.Play("MoneySpent");
	}

	public override bool CanUpgradeTile(Tile tile)
	{
		return TilesManager.Instance.Deck.Count > 1;
	}

	public override bool CanBeOfferedGivenThisDeck(List<Tile> deck)
	{
		return deck.Count >= GameParams.deckSizeBeforeOfferingSacrificeUpgrade;
	}

	public override string CannotUpgradeText(Tile tile)
	{
		return base.CannotUpgradeHeader + TileUpgrade.LocalizedString("CannotUpgrade_CannotSacrifice");
	}
}
