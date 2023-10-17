using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoBehaviour
{
	public MapScreen mapScreen;

	public Map map;

	private bool preOpenCombatInProgress;

	private bool preOpenCanInteractWithTiles;

	private bool destinationReached;

	public static MapManager Instance { get; private set; }

	public bool Interactable { get; private set; }

	public int Sector => map.CurrentMapLocation.location.sector;

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

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.MapDestinationReached.AddListener(new UnityAction(MapDestinationReached));
	}

	public void CloseMap()
	{
		mapScreen.CloseMap();
		CombatManager.Instance.CombatInProgress = preOpenCombatInProgress;
		TilesManager.Instance.CanInteractWithTiles = preOpenCanInteractWithTiles;
		EventsManager.Instance.InputActionButtonBindersEnabled.Invoke(true);
		Interactable = false;
	}

	public void OpenMap(bool locationSelectionMode = false)
	{
		Interactable = true;
		if (!((Behaviour)mapScreen).enabled && !CombatManager.Instance.TurnInProgress && CombatSceneManager.Instance.CurrentMode != CombatSceneManager.Mode.transition)
		{
			mapScreen.Activate();
			preOpenCombatInProgress = CombatManager.Instance.CombatInProgress;
			preOpenCanInteractWithTiles = TilesManager.Instance.CanInteractWithTiles;
			CombatManager.Instance.CombatInProgress = false;
			TilesManager.Instance.CanInteractWithTiles = false;
			EventsManager.Instance.InputActionButtonBindersEnabled.Invoke(false);
			map.LocationSelectionMode = locationSelectionMode;
			map.UpdateState();
			mapScreen.OpenMap();
		}
	}

	public void OpenOrCloseMap()
	{
		if (CombatSceneManager.Instance.CurrentMode != CombatSceneManager.Mode.transition)
		{
			if (!((Behaviour)mapScreen).enabled)
			{
				OpenMap();
			}
			else if (!map.LocationSelectionMode)
			{
				CloseMap();
			}
		}
	}

	public IEnumerator NexLocationSelection()
	{
		while (mapScreen.IsInTransition)
		{
			yield return null;
		}
		destinationReached = false;
		MusicManager.Instance.Play("Shop");
		OpenMap(locationSelectionMode: true);
		while (mapScreen.IsInTransition)
		{
			yield return null;
		}
		yield return (object)new WaitForSeconds(0.2f);
		map.CurrentLocationCleared();
		while (!destinationReached)
		{
			yield return null;
		}
	}

	private void MapDestinationReached()
	{
		destinationReached = true;
		Interactable = false;
	}
}
