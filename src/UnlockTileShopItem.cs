using Parameters;
using ShopStuff;
using TileEnums;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewUnlockTileShopItem", menuName = "SO/Shop/UnlockTileShopItem", order = 1)]
public class UnlockTileShopItem : UnlockShopItemData
{
	public AttackEnum attackEnum;

	private Tile tile;

	public override string Description => "";

	public override string ItemTypeName
	{
		get
		{
			string format = LocalizationUtils.LocalizedString("Terms", "Unlock");
			string arg = LocalizationUtils.LocalizedString("Terms", "Tile");
			return string.Format(format, arg);
		}
	}

	public override ShopItemTypeEnum ShopItemTypeEnum => ShopItemTypeEnum.unlock;

	public override string Name => ((Object)tile).name;

	public override Sprite Sprite => null;

	public override bool CanBeOnSale { get; }

	public override bool CanBeSold => !UnlocksManager.Instance.TileUnlocked(attackEnum);

	public override void Initialize(Transform parent, Shop shop)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(parent, shop);
		tile = TilesFactory.Instance.Create(attackEnum);
		((Component)tile).transform.SetParent(parent);
		((Component)tile).transform.localPosition = Vector3.up * 7f * TechParams.pixelSize;
		tile.Interactable = false;
		tile.InfoBoxActivator.ColliderEnabled = false;
	}

	public override void Buy()
	{
	}
}
