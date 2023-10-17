using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewUnlockConsumableShopItem", menuName = "SO/Shop/UnlockConsumableShopItem", order = 1)]
public class UnlockConsumableShopItem : UnlockShopItemData
{
	public PotionPickup potionPickupPrefab;

	public Sprite sprite;

	public override string Description => LocalizationUtils.LocalizedString("ShopAndNPC", "UnlockTile_Consumable");

	public override string ItemTypeName
	{
		get
		{
			string format = LocalizationUtils.LocalizedString("Terms", "Unlock");
			string arg = LocalizationUtils.LocalizedString("Terms", "Consumable");
			return string.Format(format, arg);
		}
	}

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.unlock;

	public override string Name => potionPickupPrefab.Name;

	public override Sprite Sprite => sprite;

	public override bool CanBeOnSale { get; }

	public override void Buy()
	{
	}
}
