using UINavigation;

namespace UINavigationScreens;

public class MapNavigationScreen : NavigationScreen
{
	public override bool AllowAlternativeNavigation => true;

	public override bool AllowPrevNextNavigation => false;

	public override void Initialize()
	{
		INavigationGroup map = MapManager.Instance.map;
		UINavigationHelper.InitializeConnectedGroups(map);
		base.CurrentGroup = map;
	}
}
