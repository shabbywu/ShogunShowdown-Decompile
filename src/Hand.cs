using System;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using TilesUtils;
using UINavigation;
using UnityEngine;

public class Hand : MonoBehaviour, ITilesChanged, INavigationGroup
{
	public GameObject tileContainerPrefab;

	public SpriteRenderer frame;

	public GameObject rightUIContainer;

	private Vector3[] containersTargetPosition;

	private Vector3[] containersVelocity;

	private float deltaX = 0.9375f;

	private bool shouldUpdate = true;

	private bool interalShufflingInProgress;

	private int iDragTarget;

	private int iPreviousDragTarget;

	public ITilesChanged NotifyTilesChanged { get; set; }

	public TileContainersCollection TCC { get; private set; }

	public float Width => (float)TCC.NContainers * deltaX + 14f * TechParams.pixelSize;

	private bool FixHandSorting => Globals.Options.handSorting == Options.HandSorting.fixOrder;

	private bool CooldownHandSorting => Globals.Options.handSorting == Options.HandSorting.cooldown;

	public bool CanAddTile => TCC.HasEmptyContainer;

	public int NumberOfActiveContainers
	{
		get
		{
			int num = TCC.NTiles;
			if (TileBeingDraggedOverlapsWithHand())
			{
				num++;
			}
			return num;
		}
	}

	public List<INavigationTarget> Targets
	{
		get
		{
			if (TCC.NTiles == 0)
			{
				return new List<INavigationTarget> { TCC.Containers[0] };
			}
			List<INavigationTarget> list = new List<INavigationTarget>();
			foreach (TileContainer container in TCC.Containers)
			{
				if (container.HasTile)
				{
					list.Add(container);
				}
			}
			return list;
		}
	}

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	public void Initialize(int nContainers)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		TCC = ((Component)this).GetComponentInChildren<TileContainersCollection>();
		TCC.NotifyTilesChanged = this;
		TCC.InstantiateContainers(nContainers, tileContainerPrefab);
		for (int i = 0; i < nContainers; i++)
		{
			((Object)((Component)TCC.Containers[i]).gameObject).name = $"Hand_{i}";
		}
		containersTargetPosition = (Vector3[])(object)new Vector3[nContainers];
		containersVelocity = (Vector3[])(object)new Vector3[nContainers];
		frame.size = new Vector2(Width, frame.size.y);
		rightUIContainer.transform.localPosition = new Vector3(Width / 2f - 3f * TechParams.pixelSize, 0f, 0f);
		shouldUpdate = true;
	}

	public void Resize(int nContainers)
	{
		Tile[] tiles = TCC.Tiles;
		Tile[] array = tiles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TileContainer.RemoveTile();
		}
		TCC.DestroyContainers();
		Initialize(nContainers);
		array = tiles;
		foreach (Tile tile in array)
		{
			TCC.FirstEmptyContainer.AddTile(tile);
		}
		shouldUpdate = true;
	}

	public void AddTile(Tile tile, bool putInClosestHandContainer = false)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (!TCC.HasEmptyContainer)
		{
			Debug.LogError((object)"Trying to add a tile to the hand, but there are no empty containers.");
		}
		if (CooldownHandSorting && putInClosestHandContainer)
		{
			FreeUpContainer(DesiredIndexInHandFromPosition(((Component)tile).transform.position));
		}
		if (tile.PreferredHandContainer == -1)
		{
			int num = -1;
			if ((Object)(object)TilesManager.Instance != (Object)null && TilesManager.Instance.Deck != null)
			{
				foreach (Tile item in TilesManager.Instance.Deck)
				{
					num = Mathf.Max(num, item.PreferredHandContainer);
				}
				tile.PreferredHandContainer = num + 1;
			}
		}
		TCC.FirstEmptyContainer.AddTile(tile);
	}

	public void UpdateHandState()
	{
		interalShufflingInProgress = true;
		ActivateContainers();
		DistributeTilesInContainers();
		PositionContainers();
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.HandStateWasUpdated.Invoke();
		}
		interalShufflingInProgress = false;
	}

	private void LateUpdate()
	{
		if (TileBeingDraggedOverlapsWithHand())
		{
			iDragTarget = DesiredIndexInHandForDraggedTile();
		}
		else
		{
			iDragTarget = -1;
		}
		if (iPreviousDragTarget != iDragTarget)
		{
			shouldUpdate = true;
		}
		iPreviousDragTarget = iDragTarget;
		if (shouldUpdate)
		{
			UpdateHandState();
			shouldUpdate = false;
		}
	}

	public bool TileBeingDraggedOverlapsWithHand()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)TilesManager.Instance == (Object)null || (Object)(object)TilesManager.Instance.TileBeingDragged == (Object)null)
		{
			return false;
		}
		Vector3 position = ((Component)TilesManager.Instance.TileBeingDragged).transform.position;
		float num = ((Component)this).transform.position.x + frame.size.x / 2f;
		float num2 = ((Component)this).transform.position.x - frame.size.x / 2f;
		if (CooldownHandSorting)
		{
			Tile[] tiles = TCC.Tiles;
			foreach (Tile tile in tiles)
			{
				if (!tile.Interactable)
				{
					num = ((Component)tile).transform.position.x + 0.1f;
					break;
				}
			}
		}
		if (position.x > num2 && position.x < num && position.y > ((Component)this).transform.position.y - frame.size.y / 2f)
		{
			return position.y < ((Component)this).transform.position.y + frame.size.y / 2f;
		}
		return false;
	}

	private void ActivateContainers()
	{
		int numberOfActiveContainers = NumberOfActiveContainers;
		for (int i = 0; i < TCC.NContainers; i++)
		{
			TCC.Containers[i].Interactable = i < numberOfActiveContainers;
		}
	}

	private void PositionContainers()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		int numberOfActiveContainers = NumberOfActiveContainers;
		for (int i = 0; i < numberOfActiveContainers; i++)
		{
			TileContainer tileContainer = TCC.Containers[i];
			Vector3 val = (float)(numberOfActiveContainers - 1 - 2 * i) * deltaX * Vector3.left / 2f;
			if (tileContainer.HasTile)
			{
				Transform transform = ((Component)tileContainer.Tile).transform;
				transform.localPosition += ((Component)tileContainer).transform.localPosition - val;
			}
			((Component)tileContainer).transform.localPosition = val;
		}
	}

	private void DistributeTilesInContainers()
	{
		if (CooldownHandSorting)
		{
			DistributeTilesInContainersCooldownOrder();
		}
		else if (FixHandSorting)
		{
			DistributeTilesInContainersFixedOrder();
		}
	}

	private void DistributeTilesInContainersCooldownOrder()
	{
		TCC.SortByTurnsBeforeCharged();
		if (TileBeingDraggedOverlapsWithHand())
		{
			FreeUpContainer(DesiredIndexInHandForDraggedTile());
		}
	}

	private void DistributeTilesInContainersFixedOrder()
	{
		SortByPreferredContainer();
		if (TileBeingDraggedOverlapsWithHand())
		{
			int num = DesiredIndexInHandForDraggedTile();
			FreeUpContainer(num);
			Tile tileBeingDragged = TilesManager.Instance.TileBeingDragged;
			Tile tile = null;
			Tile tile2 = null;
			if (num > 0 && TCC.Containers[num - 1].HasTile)
			{
				tile = TCC.Containers[num - 1].Tile;
			}
			if (num < TCC.NContainers - 1 && TCC.Containers[num + 1].HasTile)
			{
				tile2 = TCC.Containers[num + 1].Tile;
			}
			if ((Object)(object)tile != (Object)null && tileBeingDragged.PreferredHandContainer < tile.PreferredHandContainer)
			{
				int preferredHandContainer = tileBeingDragged.PreferredHandContainer;
				int preferredHandContainer2 = tile.PreferredHandContainer;
				tileBeingDragged.PreferredHandContainer = preferredHandContainer2;
				tile.PreferredHandContainer = preferredHandContainer;
			}
			if ((Object)(object)tile2 != (Object)null && tileBeingDragged.PreferredHandContainer > tile2.PreferredHandContainer)
			{
				int preferredHandContainer3 = tileBeingDragged.PreferredHandContainer;
				int preferredHandContainer4 = tile2.PreferredHandContainer;
				tileBeingDragged.PreferredHandContainer = preferredHandContainer4;
				tile2.PreferredHandContainer = preferredHandContainer3;
			}
		}
	}

	private void SortByPreferredContainer()
	{
		TCC.Defragment();
		Tile[] array = TCC.Tiles.OrderBy((Tile c) => c.PreferredHandContainer).ToArray();
		foreach (TileContainer container in TCC.Containers)
		{
			container.RemoveTile();
		}
		Tile[] array2 = array;
		foreach (Tile tile in array2)
		{
			TCC.FirstEmptyContainer.AddTile(tile);
		}
	}

	private void FreeUpContainer(int iToFree)
	{
		for (int num = TCC.NContainers - 1; num > iToFree; num--)
		{
			if (!TCC.Containers[num].HasTile && TCC.Containers[num - 1].HasTile)
			{
				TCC.Containers[num - 1].MoveTileTo(TCC.Containers[num]);
			}
		}
		for (int i = 0; i < iToFree; i++)
		{
			if (!TCC.Containers[i].HasTile && TCC.Containers[i + 1].HasTile)
			{
				TCC.Containers[i + 1].MoveTileTo(TCC.Containers[i]);
			}
		}
	}

	public int DesiredIndexInHandFromPosition(Vector3 position)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Tile[] tiles = TCC.Tiles;
		if (tiles.Length == 0)
		{
			return 0;
		}
		if (position.x < ((Component)tiles[0]).transform.position.x)
		{
			num = 0;
		}
		else if (position.x > ((Component)tiles[^1]).transform.position.x)
		{
			num = tiles.Length;
		}
		else
		{
			for (int i = 1; i < tiles.Length; i++)
			{
				if (position.x > ((Component)tiles[i - 1]).transform.position.x && position.x <= ((Component)tiles[i]).transform.position.x)
				{
					num = i;
					break;
				}
			}
		}
		if (CooldownHandSorting)
		{
			int num2 = -1;
			for (int j = 0; j < tiles.Length && tiles[j].Interactable; j++)
			{
				num2 = j;
			}
			num = Mathf.Min(num2 + 1, num);
		}
		return num;
	}

	public int DesiredIndexInHandForDraggedTile()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return DesiredIndexInHandFromPosition(((Component)TilesManager.Instance.TileBeingDragged).transform.position);
	}

	void ITilesChanged.TilesChanged()
	{
		if (!interalShufflingInProgress)
		{
			shouldUpdate = true;
		}
		if (NotifyTilesChanged != null)
		{
			NotifyTilesChanged.TilesChanged();
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == null)
		{
			Debug.LogWarning((object)"SelectedKeyNavigatable is null in HandContainerGroup.Navigate()");
			return null;
		}
		TileContainer item = (TileContainer)SelectedTarget;
		int num = TCC.Containers.IndexOf(item);
		if (navigationDirection == NavigationDirection.left)
		{
			num--;
		}
		if (navigationDirection == NavigationDirection.right)
		{
			num++;
		}
		if (num < 0 || num >= TCC.NTiles || navigationDirection == NavigationDirection.up)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		num = Mathf.Clamp(num, 0, TCC.NTiles - 1);
		UINavigationHelper.SelectNewTarget(this, TCC.Containers[num]);
		return this;
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		int num;
		switch (entryDirection)
		{
		case NavigationDirection.left:
			num = TCC.NTiles - 1;
			while (num >= 0 && (!TCC.Containers[num].HasTile || !TCC.Containers[num].Tile.Interactable))
			{
				num--;
			}
			break;
		case NavigationDirection.right:
			num = 0;
			break;
		default:
			if ((Object)((previousTarget is Object) ? previousTarget : null) != (Object)null && TCC.NTiles > 0)
			{
				List<INavigationTarget> list = new List<INavigationTarget>();
				foreach (TileContainer container in TCC.Containers)
				{
					if (container.HasTile)
					{
						list.Add(container);
					}
				}
				INavigationTarget navigationTarget = UINavigationHelper.FindClosetsTarget(previousTarget, list);
				num = TCC.Containers.IndexOf((TileContainer)navigationTarget);
			}
			else
			{
				num = Math.Max((TCC.NTiles - 1) / 2, 0);
			}
			break;
		}
		num = Math.Max(num, 0);
		UINavigationHelper.SelectNewTarget(this, TCC.Containers[num]);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		SelectedTarget.Submit();
		SelectedTarget.Select();
		return this;
	}

	public INavigationGroup NavigatePrev()
	{
		return Navigate(NavigationDirection.left);
	}

	public INavigationGroup NavigateNext()
	{
		NavigationDirection navigationDirection = NavigationDirection.right;
		bool num = TCC.Containers.IndexOf((TileContainer)SelectedTarget) == TCC.NTiles - 1 || TCC.NTiles == 0;
		bool flag = Globals.Hero.AttackQueue.NTiles > 0;
		if (num && flag)
		{
			navigationDirection = NavigationDirection.up;
		}
		return Navigate(navigationDirection);
	}

	public void UpdateSelectedNavigateTargetAfterHandStateUpdate()
	{
		TileContainer tileContainer = (TileContainer)SelectedTarget;
		if (!tileContainer.HasTile && !CombatManager.Instance.CombatInProgress)
		{
			UINavigationHelper.SelectNewTarget(this, TCC.Containers[TCC.NTiles - 1]);
		}
		else if ((!tileContainer.HasTile || !tileContainer.Tile.Interactable) && TCC.NumberOfInteractableTiles > 0)
		{
			UINavigationHelper.SelectNewTarget(this, TCC.Containers[TCC.NumberOfInteractableTiles - 1]);
		}
		else
		{
			SelectedTarget.Select();
		}
	}
}
