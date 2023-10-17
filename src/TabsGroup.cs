using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;

public class TabsGroup : MonoBehaviour, INavigationGroup
{
	public List<TabsButton> tabButtons;

	public List<GameObject> objectsToSwap;

	public TabsButton initialTabButton;

	public Sprite tabIdle;

	public Sprite tabHover;

	public Sprite tabSelected;

	public Sprite tabSelectedAndHover;

	private GameObject activeObject;

	private TabsButton selectedTabButton;

	private bool initialized;

	public INavigationGroup NavigationGroupOfActiveObject => activeObject.GetComponent<INavigationGroup>();

	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)tabButtons).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	private void Start()
	{
		Initialize();
	}

	public void OnTabEnter(TabsButton button)
	{
		ResetTabs();
		if ((Object)(object)button == (Object)(object)selectedTabButton)
		{
			button.Background.sprite = tabSelectedAndHover;
		}
		else
		{
			button.Background.sprite = tabHover;
		}
	}

	public void OnTabExit(TabsButton button)
	{
		ResetTabs();
	}

	public void OnTabSelected(TabsButton button)
	{
		selectedTabButton = button;
		ResetTabs();
		button.Background.sprite = tabSelectedAndHover;
		int siblingIndex = ((Component)button).transform.GetSiblingIndex();
		for (int i = 0; i < objectsToSwap.Count; i++)
		{
			bool flag = i == siblingIndex;
			objectsToSwap[i].SetActive(flag);
			if (flag)
			{
				activeObject = objectsToSwap[i];
			}
		}
	}

	public void ResetTabs()
	{
		foreach (TabsButton tabButton in tabButtons)
		{
			if ((Object)(object)tabButton == (Object)(object)selectedTabButton)
			{
				tabButton.Background.sprite = tabSelected;
			}
			else
			{
				tabButton.Background.sprite = tabIdle;
			}
		}
	}

	public void Initialize()
	{
		if (!initialized)
		{
			OnTabSelected(initialTabButton);
			ResetTabs();
			initialized = true;
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, navigationDirection);
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
			UINavigationHelper.InitializeConnectedGroups(this, null, null, NavigationGroupOfActiveObject);
			UINavigationHelper.InitializeConnectedGroups(NavigationGroupOfActiveObject, null, null, null, this);
			return this;
		}
		return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		UINavigationHelper.SelectNewTarget(this, ((Object)(object)selectedTabButton != (Object)null) ? selectedTabButton : initialTabButton);
		UINavigationHelper.InitializeConnectedGroups(this, null, null, NavigationGroupOfActiveObject);
		UINavigationHelper.InitializeConnectedGroups(NavigationGroupOfActiveObject, null, null, null, this);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
