using System.Collections.Generic;
using UINavigation;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class HeroAttackQueue : AttackQueue, INavigationGroup
{
	[SerializeField]
	private RearrangeQueuePrompt rearrangeQueuePrompt;

	private INavigationTarget _selectedTarget;

	private PlayerInput PlayerInput => InputManager.Instance.playerInput;

	public List<INavigationTarget> Targets
	{
		get
		{
			List<INavigationTarget> list = new List<INavigationTarget>();
			for (int i = 0; i < base.TCC.NTiles; i++)
			{
				list.Add(base.TCC.Containers[i]);
			}
			return list;
		}
	}

	private int IndexOfSelectedTarget
	{
		get
		{
			if (SelectedTarget == null)
			{
				return -1;
			}
			return base.TCC.Containers.IndexOf((TileContainer)SelectedTarget);
		}
	}

	public INavigationTarget SelectedTarget
	{
		get
		{
			return _selectedTarget;
		}
		set
		{
			_selectedTarget = value;
			rearrangeQueuePrompt.UpdateRearrangeQueuePrompt();
		}
	}

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => base.TCC.NTiles > 0;

	public override void TileAdded(Tile tile)
	{
		if (tile.TileContainerOrigin is HandTileContainer)
		{
			CombatManager.Instance.HeroPlayedTile(tile);
		}
	}

	public override void Hide()
	{
		if ((Object)(object)TilesManager.Instance != (Object)null && base.TCC.NTiles > 0)
		{
			base.TCC.MoveAllTilesTo(TilesManager.Instance.hand.TCC);
		}
		base.Hide();
	}

	public override void BeginAttack()
	{
		base.BeginAttack();
		rearrangeQueuePrompt.DisableRearrangeQueuePrompt();
	}

	public override void TilesWerePlayed()
	{
		MoveTilesToHand();
		rearrangeQueuePrompt.UpdateRearrangeQueuePrompt();
	}

	public void MoveTilesToHand()
	{
		SoundEffectsManager.Instance.Play("TileSubmit");
		Tile[] componentsInChildren = ((Component)this).GetComponentsInChildren<Tile>();
		foreach (Tile tile in componentsInChildren)
		{
			((Component)tile.Graphics).gameObject.SetActive(true);
			if ((Object)(object)tile.TileContainer != (Object)null)
			{
				tile.TileContainer.RemoveTile();
			}
			TilesManager.Instance.TakeTile(tile);
			tile.Interactable = false;
			tile.TileIsEnabled = tile.FullyCharged;
		}
		TilesManager.Instance.hand.UpdateHandState();
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == null)
		{
			Debug.LogWarning((object)"SelectedKeyNavigatable is null in HandContainerGroup.Navigate()");
			return null;
		}
		int num = IndexOfSelectedTarget;
		if (navigationDirection == NavigationDirection.up)
		{
			num++;
		}
		if (navigationDirection == NavigationDirection.down)
		{
			num--;
		}
		if (num < 0 || num >= base.TCC.NTiles)
		{
			INavigationGroup result = UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
			rearrangeQueuePrompt.UpdateRearrangeQueuePrompt();
			return result;
		}
		UINavigationHelper.SelectNewTarget(this, base.TCC.Containers[num]);
		return this;
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		INavigationTarget navigationTarget;
		if (previousTarget != null)
		{
			navigationTarget = UINavigationHelper.FindClosetsTarget(previousTarget, Targets);
		}
		else
		{
			INavigationTarget navigationTarget2 = base.TCC.Containers[0];
			navigationTarget = navigationTarget2;
		}
		INavigationTarget newTarget = navigationTarget;
		UINavigationHelper.SelectNewTarget(this, newTarget);
	}

	public INavigationGroup NavigatePrev()
	{
		if (IndexOfSelectedTarget == 0)
		{
			INavigationGroup result = UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.down, NavigationDirection.left);
			rearrangeQueuePrompt.UpdateRearrangeQueuePrompt();
			return result;
		}
		return Navigate(NavigationDirection.down);
	}

	public INavigationGroup NavigateNext()
	{
		if (IndexOfSelectedTarget == base.NTiles - 1)
		{
			return this;
		}
		return Navigate(NavigationDirection.up);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		SelectedTarget.Submit();
		if (base.TCC.NTiles == 0)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.down, NavigationDirection.left);
		}
		int num = base.TCC.Containers.IndexOf((TileContainer)SelectedTarget);
		int num2 = Mathf.Clamp(num, 0, base.NTiles - 1);
		if (num2 == num)
		{
			SelectedTarget.Deselect();
			SelectedTarget.Select();
			rearrangeQueuePrompt.UpdateRearrangeQueuePrompt();
		}
		else
		{
			UINavigationHelper.SelectNewTarget(this, base.TCC.Containers[num2]);
		}
		return this;
	}

	public void SwapQueue(int y)
	{
		if (base.NTiles > 1)
		{
			TileContainer tileContainer = (TileContainer)SelectedTarget;
			int index = MyMath.ModularizeIndex(base.TCC.Containers.IndexOf(tileContainer) + y, base.NTiles);
			Tile tile = tileContainer.RemoveTile();
			tileContainer.AddTile(base.TCC.Containers[index].RemoveTile());
			base.TCC.Containers[index].AddTile(tile);
			UINavigationHelper.SelectNewTarget(this, base.TCC.Containers[index]);
		}
	}
}
