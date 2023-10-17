using System.Collections;
using System.Collections.Generic;
using TMPro;
using UINavigation;
using UnityEngine;

public abstract class MetaProgressionUIPage : MonoBehaviour, INavigationGroup
{
	[SerializeField]
	private TextMeshProUGUI pageNameTMPro;

	[SerializeField]
	private TextMeshProUGUI progressTMPro;

	[SerializeField]
	private PagingScrollBar pagingScrollBar;

	protected abstract int NUnlocks { get; }

	protected abstract int NUnlocked { get; }

	public abstract int NumberOfElementsPerPage { get; }

	public int TotalNumberOfElements => NUnlocks;

	public int CurrentPage { get; set; }

	public int NumberOfPages => Mathf.CeilToInt((float)TotalNumberOfElements / (float)NumberOfElementsPerPage);

	public abstract List<INavigationTarget> Targets { get; }

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	protected void UpdateHeader()
	{
		((TMP_Text)progressTMPro).text = $"{NUnlocked}/{NUnlocks}";
	}

	protected virtual void OnEnable()
	{
		UpdateHeader();
	}

	public abstract void UpdateForPage();

	public void NextPage()
	{
		CurrentPage++;
		if (CurrentPage >= NumberOfPages)
		{
			CurrentPage = 0;
		}
		UpdateForPage();
	}

	public void PreviousPage()
	{
		CurrentPage--;
		if (CurrentPage < 0)
		{
			CurrentPage = NumberOfPages - 1;
		}
		UpdateForPage();
	}

	public List<T> GetElementsOnPage<T>(List<T> elements)
	{
		new List<T>();
		int num = CurrentPage * NumberOfElementsPerPage;
		int count = Mathf.Min(NumberOfElementsPerPage, elements.Count - num);
		return elements.GetRange(num, count);
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		INavigationTarget navigationTargetFromDirection = UINavigationHelper.GetNavigationTargetFromDirection(SelectedTarget, Targets, navigationDirection);
		if (navigationTargetFromDirection == null && navigationDirection == NavigationDirection.right)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		if (navigationTargetFromDirection != null)
		{
			UINavigationHelper.SelectNewTarget(this, navigationTargetFromDirection);
		}
		else if (navigationDirection == NavigationDirection.up && CurrentPage > 0)
		{
			pagingScrollBar.PreviousPage();
			((MonoBehaviour)this).StartCoroutine(CallOnEntryAfterWaitingForAFewUpdates(SelectedTarget.Transform.position + Vector3.down * 100f));
		}
		else if (navigationDirection == NavigationDirection.down && CurrentPage < NumberOfPages - 1)
		{
			pagingScrollBar.NextPage();
			((MonoBehaviour)this).StartCoroutine(CallOnEntryAfterWaitingForAFewUpdates(SelectedTarget.Transform.position + Vector3.up * 100f));
		}
		return this;
	}

	public IEnumerator CallOnEntryAfterWaitingForAFewUpdates(Vector3 entryPosition)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		SelectedTarget = null;
		yield return (object)new WaitForEndOfFrame();
		yield return (object)new WaitForEndOfFrame();
		OnEntry(NavigationDirection.none, null, entryPosition);
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		INavigationTarget newTarget = Targets[0];
		if (previousTarget != null)
		{
			newTarget = UINavigationHelper.FindClosetsTarget(previousTarget, Targets);
		}
		else if (entryPosition.HasValue)
		{
			newTarget = UINavigationHelper.GetClosestNavigationTargetToPosition(Targets, entryPosition.Value);
		}
		UINavigationHelper.SelectNewTarget(this, newTarget);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
