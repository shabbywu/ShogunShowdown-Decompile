using System.Collections.Generic;
using UINavigation;
using UnityEngine;

public class MenuPage : MonoBehaviour, INavigationGroup
{
	[SerializeField]
	private GameObject header;

	[SerializeField]
	private OptionsMenuItem backMenuItem;

	protected List<INavigationTarget> navigationTargets = new List<INavigationTarget>();

	private INavigationTarget entryTarget;

	private static readonly string menuItemsContainerPath = "Canvas/MenuItems";

	private static readonly float transitionTime = 0.15f;

	public OptionsMenuItem BackMenuItem
	{
		get
		{
			if (!((Object)(object)backMenuItem != (Object)null))
			{
				return null;
			}
			return backMenuItem;
		}
	}

	public List<INavigationTarget> Targets => navigationTargets;

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	public void Enable(bool isNavigatingBack = false)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).gameObject.activeSelf)
		{
			return;
		}
		((Component)this).gameObject.SetActive(true);
		header.transform.localScale = new Vector3(1f, 0f, 1f);
		LeanTween.scaleY(header, 1f, transitionTime).setIgnoreTimeScale(true);
		if (!((Object)(object)MenuUINavigationManager.Instance != (Object)null))
		{
			return;
		}
		MenuUINavigationManager.Instance.SetCurrentNavigationGroup(this);
		if (MenuUINavigationManager.Instance.NavigationEnabled)
		{
			if (!isNavigatingBack)
			{
				SelectedTarget = null;
			}
			InitializeNavigation();
		}
	}

	public void Disable()
	{
		if (((Component)this).gameObject.activeSelf)
		{
			((Component)this).gameObject.SetActive(false);
			SelectedTarget?.Deselect();
		}
	}

	public virtual void InitializeNavigation()
	{
		navigationTargets = GetActiveNavigationTargets();
		entryTarget = navigationTargets[0];
		UINavigationHelper.InitializeConnectedGroups(this);
		OnEntry(NavigationDirection.none, SelectedTarget);
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == null)
		{
			UINavigationHelper.SelectNewTarget(this, entryTarget);
			return this;
		}
		if (SelectedTarget is OptionsMenuItem && (navigationDirection == NavigationDirection.right || navigationDirection == NavigationDirection.left))
		{
			if (navigationDirection == NavigationDirection.right)
			{
				((OptionsMenuItem)SelectedTarget)?.OnRight();
			}
			if (navigationDirection == NavigationDirection.left)
			{
				((OptionsMenuItem)SelectedTarget)?.OnLeft();
			}
			return this;
		}
		INavigationTarget navigationTarget = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, navigationTargets, navigationDirection);
		if (navigationTarget == null)
		{
			if (navigationDirection == NavigationDirection.down)
			{
				navigationTarget = navigationTargets[0];
			}
			if (navigationDirection == NavigationDirection.up)
			{
				navigationTarget = navigationTargets[navigationTargets.Count - 1];
			}
		}
		if (navigationTarget != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTarget);
		}
		return this;
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		SelectedTarget?.Deselect();
		SelectedTarget = null;
		INavigationTarget newTarget = previousTarget ?? entryTarget;
		UINavigationHelper.SelectNewTarget(this, newTarget);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		SelectedTarget?.Submit();
		return this;
	}

	private List<INavigationTarget> GetActiveNavigationTargets()
	{
		List<INavigationTarget> list = new List<INavigationTarget>();
		Transform val = ((Component)this).transform.Find(menuItemsContainerPath);
		for (int i = 0; i < val.childCount; i++)
		{
			GameObject gameObject = ((Component)val.GetChild(i)).gameObject;
			INavigationTarget component = gameObject.GetComponent<INavigationTarget>();
			if (gameObject.activeSelf && component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}
}
