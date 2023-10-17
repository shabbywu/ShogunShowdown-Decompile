namespace UINavigation;

public abstract class NavigationScreen
{
	public abstract bool AllowAlternativeNavigation { get; }

	public abstract bool AllowPrevNextNavigation { get; }

	public bool Interactable { get; protected set; }

	public INavigationGroup CurrentGroup { get; set; }

	public abstract void Initialize();

	public virtual void Activate(INavigationTarget lastTarget = null)
	{
		Interactable = true;
		CurrentGroup.OnEntry(NavigationDirection.none, lastTarget);
	}

	public virtual void ReActivate(INavigationTarget lastTarget = null)
	{
		Activate(lastTarget);
	}

	public void Deactivate()
	{
		Interactable = false;
		if (CurrentGroup != null && CurrentGroup.SelectedTarget != null)
		{
			CurrentGroup.SelectedTarget.Deselect();
			CurrentGroup.SelectedTarget = null;
		}
	}

	public void Refresh(INavigationTarget lastNavigationTarget = null)
	{
		CurrentGroup.OnEntry(NavigationDirection.none, lastNavigationTarget);
	}

	public void Navigate(NavigationDirection direction)
	{
		if (Interactable)
		{
			CurrentGroup = CurrentGroup.Navigate(direction);
		}
	}

	public void NavigateNext()
	{
		if (Interactable && AllowPrevNextNavigation)
		{
			CurrentGroup = CurrentGroup.NavigateNext();
		}
	}

	public void NavigatePrev()
	{
		if (Interactable && AllowPrevNextNavigation)
		{
			CurrentGroup = CurrentGroup.NavigatePrev();
		}
	}

	public void SubmitCurrentTarget()
	{
		if (Interactable)
		{
			CurrentGroup = CurrentGroup.SubmitCurrentTarget();
		}
	}
}
