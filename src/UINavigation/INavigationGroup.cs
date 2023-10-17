using System.Collections.Generic;
using UnityEngine;

namespace UINavigation;

public interface INavigationGroup
{
	Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	INavigationTarget SelectedTarget { get; set; }

	List<INavigationTarget> Targets { get; }

	bool CanBeNavigatedTo { get; }

	INavigationGroup SubmitCurrentTarget();

	INavigationGroup Navigate(NavigationDirection navigationDirection);

	INavigationGroup NavigateNext()
	{
		return this;
	}

	INavigationGroup NavigatePrev()
	{
		return this;
	}

	void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null);
}
