using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewShopUpgradeShopItem", menuName = "SO/Shop/ShopUpgradeShopItem", order = 1)]
public class ShopUpgradeShopItem : ShopItemData
{
	[SerializeField]
	private string SlotTypeLocalizationTableKey;

	public Sprite sprite;

	public ShopItemTypeEnum shopItemTypeToIncrease;

	public override string Description
	{
		get
		{
			string format = LocalizationUtils.LocalizedString("ShopAndNPC", "GenericShopUpgrade_Description");
			string arg = LocalizationUtils.LocalizedString("Terms", SlotTypeLocalizationTableKey);
			return string.Format(format, arg);
		}
	}

	public override string ItemTypeName => LocalizationUtils.LocalizedString("ShopAndNPC", "GenericShopUpgrade_ItemType");

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.shopUpgrade;

	public override string Name => LocalizationUtils.LocalizedString("ShopAndNPC", "GenericShopUpgrade_Name");

	public override Sprite Sprite => sprite;

	public override bool CanBeOnSale { get; }

	public override void Buy()
	{
	}
}
