using ShopStuff;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "SO/Shop/SkillShopItem", order = 1)]
public class SkillShopItemData : ShopItemData
{
	public Item itemPrefab;

	private Item item;

	public override string Description => item.Description;

	public override string ItemTypeName => LocalizationUtils.LocalizedString("Terms", "Skill");

	public override string Name => item.Name;

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.skill;

	public override Sprite Sprite => item.Sprite;

	public override bool CanBeSold
	{
		get
		{
			if (!UnlocksManager.Instance.SkillUnlocked(((object)itemPrefab).GetType()))
			{
				return false;
			}
			if (ItemsManager.Instance.HasThisItemType(itemPrefab))
			{
				Item itemOfThisItemType = ItemsManager.Instance.GetItemOfThisItemType(itemPrefab);
				if (itemOfThisItemType.Level >= itemOfThisItemType.MaxLevel)
				{
					return false;
				}
			}
			return true;
		}
	}

	public override void Initialize(Transform parent, Shop shop)
	{
		base.Initialize(parent, shop);
		GameObject val = Object.Instantiate<GameObject>(((Component)itemPrefab).gameObject, parent);
		item = val.GetComponent<Item>();
	}

	public override void Buy()
	{
		ItemsManager.Instance.PickUpItem(item);
	}
}
