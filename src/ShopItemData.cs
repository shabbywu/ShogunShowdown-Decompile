using ShopStuff;
using UnityEngine;

public abstract class ShopItemData : ScriptableObject
{
	public CurrencyEnum currency;

	public int price;

	public Sprite idleSprite;

	public Sprite highlighCannotAffordSprite;

	public Sprite highlighCanAffordSprite;

	public string itemTypeColorHex;

	protected Transform parent;

	protected Shop shop;

	public abstract string Name { get; }

	public abstract string Description { get; }

	public abstract string ItemTypeName { get; }

	public abstract ShopItemTypeEnum ShopItemTypeEnum { get; }

	public abstract Sprite Sprite { get; }

	public virtual bool CanBeOnSale { get; } = true;


	public virtual bool CanBeSold { get; } = true;


	public virtual float Probability { get; } = 1f;


	public virtual void Initialize(Transform parent, Shop shop)
	{
		this.parent = parent;
		this.shop = shop;
	}

	public abstract void Buy();
}
