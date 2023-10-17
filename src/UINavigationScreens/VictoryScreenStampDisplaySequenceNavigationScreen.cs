using UINavigation;

namespace UINavigationScreens;

public class VictoryScreenStampDisplaySequenceNavigationScreen : NavigationScreen
{
	public override bool AllowAlternativeNavigation => true;

	public override bool AllowPrevNextNavigation => false;

	public HeroStampsDisplay StampsDisplay { get; set; }

	public override void Initialize()
	{
		UINavigationHelper.InitializeConnectedGroups(StampsDisplay);
		base.CurrentGroup = StampsDisplay;
	}

	public override void Activate(INavigationTarget lastNavigationTarget = null)
	{
		base.Interactable = true;
	}
}
