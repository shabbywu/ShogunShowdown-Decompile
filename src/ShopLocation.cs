using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewLocation", menuName = "SO/ShopLocation", order = 1)]
public class ShopLocation : Location
{
	public Room room;

	public ShopComponent leftShopComponent;

	public ShopComponent rightShopComponent;

	public override int NRooms => 1;

	public override string Name
	{
		get
		{
			if ((Object)(object)leftShopComponent == (Object)null || (Object)(object)rightShopComponent == (Object)null)
			{
				return "shop";
			}
			return LocalizationUtils.LocalizedString("Locations", leftShopComponent.technicalName) + "\n&\n" + LocalizationUtils.LocalizedString("Locations", rightShopComponent.technicalName);
		}
	}

	public override Room[] Rooms => new Room[1] { room };

	public override string RoomName(int iRoom)
	{
		return "";
	}

	public override string RoomId(int iRoom)
	{
		return $"shop_{island}";
	}

	public void AssignShopComponents(ShopComponent leftShop, ShopComponent rightShop)
	{
		leftShopComponent = leftShop;
		rightShopComponent = rightShop;
		technicalName = leftShopComponent.technicalName + "_" + rightShopComponent.technicalName;
	}

	public override void Validate()
	{
	}
}
