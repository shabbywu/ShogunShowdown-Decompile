using UINavigation;

namespace UINavigationScreens;

public class FullInfoModeNavigationScreen : NavigationScreen
{
	public override bool AllowPrevNextNavigation => false;

	public override bool AllowAlternativeNavigation => false;

	public override void Initialize()
	{
		INavigationGroup instance = InfoBoxManager.Instance;
		UINavigationHelper.InitializeConnectedGroups(instance);
		base.CurrentGroup = instance;
	}
}
