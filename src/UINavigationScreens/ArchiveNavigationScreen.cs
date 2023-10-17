using UINavigation;

namespace UINavigationScreens;

public class ArchiveNavigationScreen : NavigationScreen
{
	public override bool AllowAlternativeNavigation => false;

	public override bool AllowPrevNextNavigation => false;

	public override void Initialize()
	{
		INavigationGroup tabsGroup = ((CampRoom)CombatSceneManager.Instance.Room).MetaProgressionUI.tabsGroup;
		UINavigationHelper.InitializeConnectedGroups(tabsGroup);
		base.CurrentGroup = tabsGroup;
	}
}
