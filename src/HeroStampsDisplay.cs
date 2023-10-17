using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;
using UnityEngine.UI;

public class HeroStampsDisplay : MonoBehaviour, INavigationGroup
{
	public HeroStampUI[] stamps;

	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)stamps).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	public void Initialize(Hero hero)
	{
		HeroStampUI[] array = stamps;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize(hero);
		}
	}

	public void SetUIInteractionEnabled(bool value)
	{
		((Behaviour)((Component)this).GetComponent<GraphicRaycaster>()).enabled = value;
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == null)
		{
			OnEntry(navigationDirection);
			return this;
		}
		if (navigationDirection == NavigationDirection.down)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, stamps.Cast<INavigationTarget>().ToList(), navigationDirection);
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
		}
		return this;
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		UINavigationHelper.SelectNewTarget(this, stamps[0]);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
