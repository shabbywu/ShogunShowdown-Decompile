using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;
using UnityEngine.Events;

public class SkillsIconsContainer : MonoBehaviour, INavigationGroup
{
	public Vector3 gameOverLocalPosition;

	public List<INavigationTarget> Targets
	{
		get
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			List<INavigationTarget> list = new List<INavigationTarget>();
			foreach (Transform item in ((Component)this).transform)
			{
				Transform val = item;
				list.Add(((Component)val).GetComponent<INavigationTarget>());
			}
			return list;
		}
	}

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => ((Component)this).transform.childCount > 0;

	private void Start()
	{
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
	}

	private void GameOver(bool win)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = gameOverLocalPosition;
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, navigationDirection);
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
			return this;
		}
		return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		UINavigationHelper.SelectNewTarget(this, Targets.Last());
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
