using ShopStuff;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopData", menuName = "SO/Shop/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
	public string technicalName;

	public ShopItemPool[] pools;

	public bool giveFreePotion;
}
