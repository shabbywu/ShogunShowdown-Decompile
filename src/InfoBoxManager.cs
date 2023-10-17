using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;

public class InfoBoxManager : MonoBehaviour, INavigationGroup
{
	public static InfoBoxManager Instance { get; private set; }

	private List<InfoBoxActivatorWithFrame> InfoBoxActivatorsWithFrame { get; set; } = new List<InfoBoxActivatorWithFrame>();


	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)InfoBoxActivatorsWithFrame).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

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

	public void Register(InfoBoxActivatorWithFrame infoBoxActivatorWithFrame)
	{
		InfoBoxActivatorsWithFrame.Add(infoBoxActivatorWithFrame);
	}

	public void Deregister(InfoBoxActivatorWithFrame infoBoxActivatorWithFrame)
	{
		InfoBoxActivatorsWithFrame.Remove(infoBoxActivatorWithFrame);
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, navigationDirection);
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
		}
		return this;
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		INavigationTarget navigationTarget = null;
		if (previousTarget != null)
		{
			navigationTarget = UINavigationHelper.FindClosetsTarget(previousTarget, Targets);
		}
		if (navigationTarget == null)
		{
			navigationTarget = InfoBoxActivatorsWithFrame[0];
		}
		UINavigationHelper.SelectNewTarget(this, navigationTarget);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
