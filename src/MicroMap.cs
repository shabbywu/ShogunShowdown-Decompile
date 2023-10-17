using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MicroMap : MonoBehaviour
{
	[SerializeField]
	private MicroMapLocationUI microMapLocationPrefab;

	[SerializeField]
	private MicroMapConnection microMapConnectionPrefab;

	[SerializeField]
	private Transform container;

	private List<MicroMapLocationUI> microMapLocationsUI;

	private bool initialized;

	private void Start()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		EventsManager.Instance.EnterRoom.AddListener((UnityAction<Room>)EnterRoom);
		EventsManager.Instance.ExitRoom.AddListener((UnityAction<Room>)ExitRoom);
		EventsManager.Instance.BeginBossFight.AddListener(new UnityAction(Hide));
		EventsManager.Instance.EndBossFight.AddListener(new UnityAction(Show));
	}

	public void Initialize(Room[] rooms)
	{
		Clear();
		if (rooms.Length <= 1)
		{
			return;
		}
		microMapLocationsUI = new List<MicroMapLocationUI>();
		for (int i = 0; i < rooms.Length; i++)
		{
			if (!rooms[i].microMapLocation.showInMicroMap)
			{
				continue;
			}
			MicroMapConnection microMapConnection = null;
			if (microMapLocationsUI.Count > 0)
			{
				microMapConnection = InstantiateConnection(container);
			}
			microMapLocationsUI.Add(InstantiateLocation(rooms[i], i, container));
			if ((Object)(object)microMapConnection != (Object)null)
			{
				microMapLocationsUI[^1].LeftConnection = microMapConnection;
				if (microMapLocationsUI.Count > 1)
				{
					microMapLocationsUI[^2].RightConnection = microMapConnection;
				}
			}
		}
		initialized = true;
	}

	public void Clear()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in container)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		initialized = false;
	}

	private void EnterRoom(Room room)
	{
		if (!initialized)
		{
			return;
		}
		int num = -1;
		foreach (MicroMapLocationUI item in microMapLocationsUI)
		{
			if (room.IndexInLocation == item.RoomIndex)
			{
				item.CurrentState = MicroMapLocationUI.State.current;
			}
			else if (room.IndexInLocation > item.RoomIndex)
			{
				item.CurrentState = MicroMapLocationUI.State.cleared;
				if ((Object)(object)item.RightConnection != (Object)null)
				{
					item.RightConnection.CurrentState = MicroMapConnection.State.cleared;
				}
				if ((Object)(object)item.LeftConnection != (Object)null)
				{
					item.LeftConnection.CurrentState = MicroMapConnection.State.cleared;
				}
				num = Mathf.Max(num, microMapLocationsUI.IndexOf(item));
			}
		}
		if (!room.microMapLocation.showInMicroMap && num >= 0 && (Object)(object)microMapLocationsUI[num].RightConnection != (Object)null)
		{
			microMapLocationsUI[num].RightConnection.CurrentState = MicroMapConnection.State.current;
		}
	}

	public void ExitRoom(Room room)
	{
		if (!initialized)
		{
			return;
		}
		MicroMapLocationUI microMapLocationUI = microMapLocationsUI.First((MicroMapLocationUI loc) => loc.RoomIndex == room.IndexInLocation);
		if (!((Object)(object)microMapLocationUI == (Object)null))
		{
			microMapLocationUI.CurrentState = MicroMapLocationUI.State.cleared;
			if ((Object)(object)microMapLocationUI.RightConnection != (Object)null)
			{
				microMapLocationUI.RightConnection.CurrentState = MicroMapConnection.State.current;
			}
		}
	}

	private MicroMapLocationUI InstantiateLocation(Room room, int iRoom, Transform parent)
	{
		MicroMapLocationUI component = Object.Instantiate<GameObject>(((Component)microMapLocationPrefab).gameObject, parent).GetComponent<MicroMapLocationUI>();
		component.Initialize(room.microMapLocation, iRoom);
		return component;
	}

	private MicroMapConnection InstantiateConnection(Transform parent)
	{
		MicroMapConnection component = Object.Instantiate<GameObject>(((Component)microMapConnectionPrefab).gameObject, parent).GetComponent<MicroMapConnection>();
		component.CurrentState = MicroMapConnection.State.todo;
		return component;
	}

	private void Hide()
	{
		((Component)container).gameObject.SetActive(false);
	}

	private void Show()
	{
		((Component)container).gameObject.SetActive(true);
	}
}
