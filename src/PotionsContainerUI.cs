using System;
using System.Collections.Generic;
using UINavigation;
using UnityEngine;

public class PotionsContainerUI : MonoBehaviour, INavigationGroup
{
	[SerializeField]
	private GameObject potionsContainer;

	[SerializeField]
	private GameObject potionSlotsContainer;

	[SerializeField]
	private GameObject potionSlotPrefab;

	private int nSlots;

	public int NPotions => potionsContainer.transform.childCount;

	public Potion[] Potions => potionsContainer.GetComponentsInChildren<Potion>();

	private int IndexOfSelectedTarget
	{
		get
		{
			if (SelectedTarget == null)
			{
				return -1;
			}
			return Array.IndexOf(Potions, (Potion)SelectedTarget);
		}
	}

	public List<INavigationTarget> Targets
	{
		get
		{
			List<INavigationTarget> list = new List<INavigationTarget>();
			Potion[] potions = Potions;
			foreach (Potion potion in potions)
			{
				if (!potion.AlreadyUsed)
				{
					list.Add(potion);
					if (potion.CanBeSold)
					{
						list.Add(potion.SellButton);
					}
				}
			}
			return list;
		}
	}

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => Potions.Length != 0;

	public void SetNumberOfSlots(int n)
	{
		if (nSlots == n)
		{
			return;
		}
		int num = n - nSlots;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				Object.Instantiate<GameObject>(potionSlotPrefab, potionSlotsContainer.transform);
			}
		}
		else
		{
			for (int j = 0; j < -num; j++)
			{
				Object.Destroy((Object)(object)((Component)potionSlotsContainer.transform.GetChild(0)).gameObject);
			}
		}
		nSlots = n;
	}

	public void AddPotion(Potion potion)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (NPotions >= nSlots)
		{
			Debug.LogError((object)"PotionsContainerUI: Trying to add a potion, but there are no free slots!");
			return;
		}
		((Component)potion).transform.SetParent(potionsContainer.transform);
		((Component)potion).transform.localPosition = Vector3.zero;
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, navigationDirection);
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
			return this;
		}
		if (navigationDirection == NavigationDirection.right)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		return this;
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		if (Potions[0].CanBeSold)
		{
			UINavigationHelper.SelectNewTarget(this, Potions[0].SellButton);
		}
		else
		{
			UINavigationHelper.SelectNewTarget(this, Potions[0]);
		}
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		if (!(SelectedTarget is MyButton) && (!(SelectedTarget is Potion) || !((Potion)SelectedTarget).CanBeUsed))
		{
			return this;
		}
		INavigationTarget navigationTarget = null;
		NavigationDirection[] array = new NavigationDirection[2]
		{
			NavigationDirection.down,
			NavigationDirection.up
		};
		foreach (NavigationDirection direction in array)
		{
			navigationTarget = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, direction);
			if (navigationTarget != null)
			{
				break;
			}
		}
		SelectedTarget.Submit();
		if (navigationTarget == null)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.right);
		}
		UINavigationHelper.SelectNewTarget(this, navigationTarget);
		return this;
	}

	public INavigationGroup NavigatePrev()
	{
		if (IndexOfSelectedTarget == NPotions - 1)
		{
			return this;
		}
		return Navigate(NavigationDirection.up);
	}

	public INavigationGroup NavigateNext()
	{
		if (IndexOfSelectedTarget == 0)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.right);
		}
		return Navigate(NavigationDirection.down);
	}
}
