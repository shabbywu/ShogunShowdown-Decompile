using UINavigation;

namespace UINavigationScreens;

public class UnlockShopNavigationScreen : NavigationScreen
{
	public override bool AllowAlternativeNavigation => false;

	public override bool AllowPrevNextNavigation => false;

	public override void Initialize()
	{
		INavigationGroup unlocksShop = ((CampRoom)CombatSceneManager.Instance.Room).UnlocksShop;
		UINavigationHelper.InitializeConnectedGroups(unlocksShop);
		base.CurrentGroup = unlocksShop;
	}
}
