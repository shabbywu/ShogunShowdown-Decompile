using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewConsumableShopItem", menuName = "SO/Shop/ConsumableShopItem", order = 1)]
public class ConsumableShopItem : ShopItemData
{
	public PotionPickup potionPickupPrefab;

	public Sprite sprite;

	public override string Description => potionPickupPrefab.Description;

	public override string ItemTypeName => LocalizationUtils.LocalizedString("Terms", "Consumable");

	public override string Name => potionPickupPrefab.Name;

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.consumable;

	public override Sprite Sprite => sprite;

	public override void Buy()
	{
		shop.InstantiateAndThrowPickupAtHero(potionPickupPrefab.PickupEnum);
	}
}
