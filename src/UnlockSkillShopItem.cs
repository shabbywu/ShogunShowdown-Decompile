using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewUnlockSkillShopItem", menuName = "SO/Shop/UnlockSkillShopItem", order = 1)]
public class UnlockSkillShopItem : UnlockShopItemData
{
	public Item itemPrefab;

	private Item item;

	public override string Description => "";

	public override string ItemTypeName
	{
		get
		{
			string format = LocalizationUtils.LocalizedString("Terms", "Unlock");
			string arg = LocalizationUtils.LocalizedString("Terms", "Skill");
			return string.Format(format, arg);
		}
	}

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.unlock;

	public override string Name => item.Name;

	public override Sprite Sprite => item.Sprite;

	public override bool CanBeOnSale { get; }

	public override bool CanBeSold => !UnlocksManager.Instance.SkillUnlocked(((object)itemPrefab).GetType());

	public override void Initialize(Transform parent, Shop shop)
	{
		base.Initialize(parent, shop);
		GameObject val = Object.Instantiate<GameObject>(((Component)itemPrefab).gameObject, parent);
		item = val.GetComponent<Item>();
	}

	public override void Buy()
	{
	}
}
