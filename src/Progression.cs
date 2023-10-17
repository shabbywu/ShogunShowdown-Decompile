using System.Collections.Generic;
using ProgressionEnums;
using UnityEngine;

public class Progression : MonoBehaviour
{
	public Map map;

	private HashSet<IslandEnum> daimyoIslands = new HashSet<IslandEnum>
	{
		IslandEnum.white,
		IslandEnum.gray,
		IslandEnum.darkGreen
	};

	public Location TutorialLocation;

	private int iRoom;

	public static Progression Instance { get; private set; }

	public Location CurrentLocation { get; set; }

	public bool IsLastLevel
	{
		get
		{
			if (!IsLastRoomInLocation)
			{
				return false;
			}
			if (CurrentLocation.island == IslandEnum.shogun)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsShogunFight
	{
		get
		{
			if (IsLastRoomInLocation)
			{
				return CurrentLocation.island == IslandEnum.shogun;
			}
			return false;
		}
	}

	public bool IsLastRoomInLocation => iRoom == CurrentLocation.NRooms - 1;

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void Initialize()
	{
		if (Globals.ContinueRun)
		{
			map.LoadFromSaveData(SaveDataManager.Instance.runSaveData.mapSaveData);
			map.Initialize();
			LoadFromSaveData(SaveDataManager.Instance.runSaveData.progressionSaveData);
			return;
		}
		map.Initialize();
		if (Globals.Tutorial)
		{
			InitializeLocation(TutorialLocation);
		}
		else
		{
			InitializeLocation(map.CurrentMapLocation.location);
		}
	}

	public Room BuildRoom(Vector3 roomPosition, bool loadRoomStateFromSaveData = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Room component = ((Component)Object.Instantiate<Room>(CurrentLocation.Rooms[iRoom], roomPosition, Quaternion.identity)).GetComponent<Room>();
		component.Initialize(CurrentLocation.RoomName(iRoom), CurrentLocation.RoomId(iRoom), loadRoomStateFromSaveData);
		component.IndexInLocation = iRoom;
		if (loadRoomStateFromSaveData)
		{
			component.LoadFromSaveData(SaveDataManager.Instance.runSaveData);
		}
		if (component is ShopRoom)
		{
			ShopRoomInitialization((ShopRoom)component, loadRoomStateFromSaveData);
		}
		return component;
	}

	private void ShopRoomInitialization(ShopRoom shopRoom, bool loadRoomStateFromSaveData)
	{
		ShopComponent left;
		ShopComponent right;
		if (loadRoomStateFromSaveData)
		{
			left = map.GetShopComponentFromName(SaveDataManager.Instance.runSaveData.shopRoom.leftShopComponentName);
			right = map.GetShopComponentFromName(SaveDataManager.Instance.runSaveData.shopRoom.rightShopComponentName);
		}
		else if (CurrentLocation is ShopLocation)
		{
			left = ((ShopLocation)CurrentLocation).leftShopComponent;
			right = ((ShopLocation)CurrentLocation).rightShopComponent;
		}
		else
		{
			left = map.ShopComponentGeneratorLeft.GetNext();
			right = map.ShopComponentGeneratorRight.GetNext();
		}
		shopRoom.InitializeShop(left, right);
	}

	public void InitializeLocation(Location location)
	{
		CurrentLocation = location;
		iRoom = 0;
		if (Globals.ShortLocations)
		{
			iRoom = CurrentLocation.NRooms - 1;
		}
	}

	public void Next()
	{
		iRoom++;
	}

	public void PopulateSaveData(ProgressionSaveData progressionSaveData)
	{
		progressionSaveData.iRoomInProgress = iRoom;
	}

	public void LoadFromSaveData(ProgressionSaveData progressionSaveData)
	{
		CurrentLocation = map.CurrentMapLocation.location;
		iRoom = progressionSaveData.iRoomInProgress;
	}
}
