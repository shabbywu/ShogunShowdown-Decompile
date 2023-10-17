using System.Collections.Generic;
using TileEnums;
using UnityEngine;
using Utils;

public abstract class TileUpgrade : MonoBehaviour
{
	[SerializeField]
	private TileUpgradeEnum tileUpgradeEnum;

	public abstract string Description { get; }

	public virtual string Details { get; } = "";


	public virtual string ButtonText => LocalizedString("UpgradeButton");

	public string TechName => TileUpgradeEnum.ToString();

	public TileUpgradeEnum TileUpgradeEnum => tileUpgradeEnum;

	public string CannotUpgradeHeader => "[bad_color]" + LocalizedString("CannotUpgrade_Header") + "[end_color]\n";

	public abstract void Upgrade(Tile tile);

	public abstract bool CanUpgradeTile(Tile tile);

	public abstract string CannotUpgradeText(Tile tile);

	protected static string LocalizedString(string key)
	{
		return LocalizationUtils.LocalizedString("TileUpgrades", key);
	}

	public virtual bool CanBeOfferedGivenThisDeck(List<Tile> deck)
	{
		foreach (Tile item in deck)
		{
			if (CanUpgradeTile(item))
			{
				return true;
			}
		}
		return false;
	}
}
