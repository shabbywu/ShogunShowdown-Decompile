using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;
using Utils;

public class Map : MonoBehaviour, INavigationGroup
{
	[SerializeField]
	private MapLocation initialMapLocation;

	[SerializeField]
	private ShopComponent[] shopComponentsLeft;

	[SerializeField]
	private ShopComponent[] shopComponentsRight;

	private Dictionary<MapLocation, List<MapLocation>> connectedLocations;

	private Dictionary<MapLocation, List<MapConnection>> outgoingConnections;

	private MapConnection currentConnection;

	private MapLocation currentMapLocation;

	private MapLocation[] mapLocations;

	private static float maxDeltaXForSameNavigationColumn = 1f;

	public MapLocation[] MapLocations
	{
		get
		{
			if (mapLocations == null)
			{
				mapLocations = ((Component)this).GetComponentsInChildren<MapLocation>(true);
			}
			return mapLocations;
		}
	}

	public MapConnection[] Connections { get; private set; }

	public MapSprites MapSprites { get; private set; }

	public bool LocationSelectionMode { get; set; }

	public bool MovingInProgress { get; private set; }

	private List<MapLocation> ReachableLocations => connectedLocations[CurrentMapLocation];

	private List<MapConnection> ReachableConnections => outgoingConnections[CurrentMapLocation];

	private MapPlayer MapPlayer { get; set; }

	public float CameraTargetY { get; private set; }

	public PseudoRandomWithMemory<ShopComponent> ShopComponentGeneratorLeft { get; private set; }

	public PseudoRandomWithMemory<ShopComponent> ShopComponentGeneratorRight { get; private set; }

	public MapLocation CurrentMapLocation
	{
		get
		{
			if ((Object)(object)currentMapLocation == (Object)null)
			{
				currentMapLocation = initialMapLocation;
			}
			return currentMapLocation;
		}
		private set
		{
			currentMapLocation = value;
			CameraTargetY = currentMapLocation.cameraTargetY;
		}
	}

	public MapLocation CurrentMovingToMapLocation { get; private set; }

	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)GetVisibleUncoveredLocations()).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	private List<MapLocation> GetVisibleUncoveredLocations()
	{
		return MapLocations.Where((MapLocation loc) => loc.IsInsideVisibleArea && loc.Uncovered).ToList();
	}

	public void Initialize()
	{
		MapSprites = ((Component)this).GetComponent<MapSprites>();
		MapSprites.LoadSprites();
		Connections = ((Component)this).GetComponentsInChildren<MapConnection>(true);
		InitializeRandomShopComponentsGenerators();
		InitializeConnectionDictionaries();
		if (!Globals.ContinueRun)
		{
			InitializeRandomNonCombatLocations();
		}
		MapLocation[] array = MapLocations;
		foreach (MapLocation mapLocation in array)
		{
			if ((Object)(object)mapLocation == (Object)(object)initialMapLocation || AreConnected(initialMapLocation, mapLocation))
			{
				mapLocation.InitiallyUncovered = true;
			}
			mapLocation.Initialize();
		}
	}

	private void Start()
	{
		UpdateState();
		MapLocation[] array = MapLocations;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].location.Validate();
		}
	}

	public void UpdateState()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)MapPlayer == (Object)null)
		{
			MapPlayerFactory component = ((Component)this).GetComponent<MapPlayerFactory>();
			MapPlayer = component.InstantiateMapPlayer(Globals.Hero);
			((Component)MapPlayer).transform.position = ((Component)CurrentMapLocation).transform.position;
		}
		MapPlayer.UpdateState();
		MapLocation[] array = MapLocations;
		foreach (MapLocation mapLocation in array)
		{
			mapLocation.Reachable = LocationSelectionMode && AreConnected(CurrentMapLocation, mapLocation);
		}
		MapConnection[] connections = Connections;
		foreach (MapConnection mapConnection in connections)
		{
			mapConnection.Walkable = LocationSelectionMode && ReachableConnections.Contains(mapConnection);
		}
	}

	public void HighlightLocation(MapLocation mapLocation)
	{
		if (!MovingInProgress && AreConnected(CurrentMapLocation, mapLocation))
		{
			currentConnection = GetConnection(CurrentMapLocation, mapLocation);
			currentConnection.Highlight = true;
			mapLocation.Highlight = true;
		}
	}

	public void DeHighlightLocation(MapLocation mapLocation)
	{
		if (!MovingInProgress)
		{
			mapLocation.Highlight = false;
			if ((Object)(object)currentConnection != (Object)null)
			{
				currentConnection.Highlight = false;
			}
		}
	}

	public void CurrentLocationCleared()
	{
		CurrentMapLocation.Cleared = true;
		UncoverLocations(CurrentMapLocation, CurrentMapLocation.uncoverDepth);
		if (CurrentMapLocation.uncoverDepth > 0)
		{
			EffectsManager.Instance.CreateInGameEffect("MapLocationCompletionEffect", ((Component)CurrentMapLocation).transform);
		}
		EventsManager.Instance.MapCurrentLocationCleared.Invoke();
	}

	public void UncoverLocations(MapLocation mapLocation, int depth)
	{
		if (!mapLocation.Uncovered)
		{
			mapLocation.Uncover();
		}
		if (depth == 0)
		{
			return;
		}
		foreach (MapLocation item in connectedLocations[mapLocation])
		{
			UncoverLocations(item, depth - 1);
		}
	}

	public bool SelectLocation(MapLocation mapLocation)
	{
		if (MovingInProgress || !LocationSelectionMode || !AreConnected(CurrentMapLocation, mapLocation))
		{
			return false;
		}
		SoundEffectsManager.Instance.Play("MapDestinationSelected");
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		((MonoBehaviour)this).StartCoroutine(MoveToNextLocation(mapLocation));
		Progression.Instance.InitializeLocation(mapLocation.location);
		return true;
	}

	private MapConnection GetConnection(MapLocation startLocation, MapLocation endLocation)
	{
		MapConnection[] connections = Connections;
		foreach (MapConnection mapConnection in connections)
		{
			if ((Object)(object)mapConnection.start == (Object)(object)startLocation && (Object)(object)mapConnection.end == (Object)(object)endLocation)
			{
				return mapConnection;
			}
		}
		return null;
	}

	private bool AreConnected(MapLocation startLocation, MapLocation endLocation)
	{
		return connectedLocations[startLocation].Contains(endLocation);
	}

	private IEnumerator MoveToNextLocation(MapLocation mapLocation)
	{
		CurrentMovingToMapLocation = mapLocation;
		MapConnection connection = GetConnection(CurrentMapLocation, mapLocation);
		MovingInProgress = true;
		foreach (MapLocation reachableLocation in ReachableLocations)
		{
			reachableLocation.Reachable = (Object)(object)reachableLocation == (Object)(object)connection.end;
		}
		CameraTargetY = connection.end.cameraTargetY;
		yield return ((MonoBehaviour)this).StartCoroutine(connection.MoveToEndLocation(MapPlayer));
		CurrentMapLocation = connection.end;
		((Component)MapPlayer).transform.localPosition = ((Component)CurrentMapLocation).transform.localPosition;
		EventsManager.Instance.MapDestinationReached.Invoke();
		MovingInProgress = false;
		CurrentMovingToMapLocation = null;
	}

	private void InitializeConnectionDictionaries()
	{
		connectedLocations = new Dictionary<MapLocation, List<MapLocation>>();
		outgoingConnections = new Dictionary<MapLocation, List<MapConnection>>();
		MapLocation[] array = MapLocations;
		foreach (MapLocation key in array)
		{
			outgoingConnections.Add(key, new List<MapConnection>());
			connectedLocations.Add(key, new List<MapLocation>());
		}
		MapConnection[] connections = Connections;
		foreach (MapConnection mapConnection in connections)
		{
			if (mapConnection.IsWalkable)
			{
				outgoingConnections[mapConnection.start].Add(mapConnection);
				connectedLocations[mapConnection.start].Add(mapConnection.end);
			}
		}
	}

	private void InitializeRandomNonCombatLocations()
	{
		MapLocation[] array = MapLocations;
		foreach (MapLocation mapLocation in array)
		{
			if (mapLocation.location is ShopLocation)
			{
				((ShopLocation)mapLocation.location).AssignShopComponents(ShopComponentGeneratorLeft.GetNext(), ShopComponentGeneratorRight.GetNext());
			}
		}
	}

	public void DeveloperUncoverAllLocations()
	{
		MapLocation[] array = MapLocations;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Uncover();
			LocationSelectionMode = true;
			UpdateState();
		}
	}

	private void InitializeRandomShopComponentsGenerators()
	{
		List<(ShopComponent, float)> list = new List<(ShopComponent, float)>();
		ShopComponent[] array = shopComponentsLeft;
		foreach (ShopComponent item in array)
		{
			list.Add((item, 1f));
		}
		ShopComponentGeneratorLeft = new PseudoRandomWithMemory<ShopComponent>(list.ToArray(), 2f, allowSameConsecutiveResults: false);
		List<(ShopComponent, float)> list2 = new List<(ShopComponent, float)>();
		array = shopComponentsRight;
		foreach (ShopComponent item2 in array)
		{
			list2.Add((item2, 1f));
		}
		ShopComponentGeneratorRight = new PseudoRandomWithMemory<ShopComponent>(list2.ToArray(), 2f, allowSameConsecutiveResults: false);
	}

	public ShopComponent GetShopComponentFromName(string shopComponentName)
	{
		ShopComponent[] array = shopComponentsLeft;
		foreach (ShopComponent shopComponent in array)
		{
			if (shopComponent.technicalName == shopComponentName)
			{
				return shopComponent;
			}
		}
		array = shopComponentsRight;
		foreach (ShopComponent shopComponent2 in array)
		{
			if (shopComponent2.technicalName == shopComponentName)
			{
				return shopComponent2;
			}
		}
		Debug.LogError((object)("Map: GetShopComponentFromName: did not find a ShopComponenet with the name '" + shopComponentName + "'"));
		return null;
	}

	public void PopulateSaveData(MapSaveData mapSaveData)
	{
		mapSaveData.currentMapLocationID = CurrentMapLocation.ID;
		mapSaveData.currentLocationName = CurrentMapLocation.location.Name;
		MapLocation[] array = MapLocations;
		foreach (MapLocation mapLocation in array)
		{
			if (mapLocation.Uncovered)
			{
				mapSaveData.uncoveredMapLocationIDs.Add(mapLocation.ID);
			}
			if (mapLocation.location is ShopLocation)
			{
				ShopLocation shopLocation = (ShopLocation)mapLocation.location;
				mapSaveData.shopComponent.AddRange(new string[3]
				{
					mapLocation.ID,
					shopLocation.leftShopComponent.technicalName,
					shopLocation.rightShopComponent.technicalName
				});
			}
		}
	}

	public void LoadFromSaveData(MapSaveData mapSaveData)
	{
		string currentID = mapSaveData.currentMapLocationID;
		CurrentMapLocation = Array.Find(MapLocations, (MapLocation loc) => loc.ID == currentID);
		MapLocation[] array = MapLocations;
		foreach (MapLocation mapLocation in array)
		{
			if (mapSaveData.uncoveredMapLocationIDs.Contains(mapLocation.ID))
			{
				mapLocation.InitiallyUncovered = true;
			}
			if (mapLocation.location is ShopLocation)
			{
				int num = mapSaveData.shopComponent.IndexOf(mapLocation.ID);
				((ShopLocation)mapLocation.location).AssignShopComponents(GetShopComponentFromName(mapSaveData.shopComponent[num + 1]), GetShopComponentFromName(mapSaveData.shopComponent[num + 2]));
			}
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (MovingInProgress)
		{
			return this;
		}
		if (SelectedTarget == null)
		{
			return this;
		}
		MapLocation newTarget = (MapLocation)SelectedTarget;
		if (UINavigationHelper.Axis(navigationDirection) == NavigationAxis.vertical)
		{
			newTarget = VerticalNavigation((MapLocation)SelectedTarget, navigationDirection);
		}
		else if (UINavigationHelper.Axis(navigationDirection) == NavigationAxis.horizontal)
		{
			newTarget = HorizontalNavigation((MapLocation)SelectedTarget, navigationDirection);
		}
		UINavigationHelper.SelectNewTarget(this, newTarget);
		return this;
	}

	private MapLocation HorizontalNavigation(MapLocation currentlySelected, NavigationDirection navigationDirection)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		List<MapLocation> visibleUncoveredLocations = GetVisibleUncoveredLocations();
		MapLocation result = currentlySelected;
		float num = float.PositiveInfinity;
		foreach (MapLocation item in visibleUncoveredLocations)
		{
			if (!((Object)(object)item == (Object)(object)currentlySelected) && !(Mathf.Abs(((Component)item).transform.localPosition.x - ((Component)currentlySelected).transform.localPosition.x) < maxDeltaXForSameNavigationColumn) && (navigationDirection != NavigationDirection.left || !(((Component)item).transform.localPosition.x > ((Component)currentlySelected).transform.localPosition.x)) && (navigationDirection != NavigationDirection.right || !(((Component)item).transform.localPosition.x < ((Component)currentlySelected).transform.localPosition.x)))
			{
				float num2 = Vector3.Distance(((Component)item).transform.localPosition, ((Component)currentlySelected).transform.localPosition);
				if (num2 < num)
				{
					num = num2;
					result = item;
				}
			}
		}
		return result;
	}

	private MapLocation VerticalNavigation(MapLocation currentlySelected, NavigationDirection navigationDirection)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		List<MapLocation> visibleUncoveredLocations = GetVisibleUncoveredLocations();
		MapLocation result = currentlySelected;
		float num = float.PositiveInfinity;
		foreach (MapLocation item in visibleUncoveredLocations)
		{
			if (!((Object)(object)item == (Object)(object)currentlySelected) && !(Mathf.Abs(((Component)item).transform.localPosition.x - ((Component)currentlySelected).transform.localPosition.x) > maxDeltaXForSameNavigationColumn) && (navigationDirection != 0 || !(((Component)item).transform.localPosition.y < ((Component)currentlySelected).transform.localPosition.y)) && (navigationDirection != NavigationDirection.down || !(((Component)item).transform.localPosition.y > ((Component)currentlySelected).transform.localPosition.y)))
			{
				float num2 = Mathf.Abs(((Component)item).transform.localPosition.y - ((Component)currentlySelected).transform.localPosition.y);
				if (num2 < num)
				{
					num = num2;
					result = item;
				}
			}
		}
		return result;
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		MapLocation newTarget = ((connectedLocations[CurrentMapLocation].Count <= 0 || !connectedLocations[CurrentMapLocation][0].Uncovered) ? CurrentMapLocation : connectedLocations[CurrentMapLocation][0]);
		UINavigationHelper.SelectNewTarget(this, newTarget);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		SelectedTarget?.Submit();
		return this;
	}
}
