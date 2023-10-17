using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewServiceShopItem", menuName = "SO/Shop/ServiceShopItem", order = 1)]
public class ServiceShopItem : ShopItemData
{
	public ShopServiceEnum shopServiceEnum;

	[SerializeField]
	private string localizationTableKey;

	[SerializeField]
	private Sprite sprite;

	public override string Description => LocalizationUtils.LocalizedString("ShopAndNPC", localizationTableKey + "_Description");

	public override string ItemTypeName => LocalizationUtils.LocalizedString("ShopAndNPC", "BloodService");

	public override string Name => LocalizationUtils.LocalizedString("ShopAndNPC", localizationTableKey + "_Name");

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.service;

	public override Sprite Sprite => sprite;

	public override bool CanBeSold => true;

	public override void Buy()
	{
	}
}
