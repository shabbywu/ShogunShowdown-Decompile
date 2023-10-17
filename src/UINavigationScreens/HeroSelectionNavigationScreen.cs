using UINavigation;

namespace UINavigationScreens;

public class HeroSelectionNavigationScreen : NavigationScreen
{
	public override bool AllowAlternativeNavigation => false;

	public override bool AllowPrevNextNavigation => false;

	public override void Initialize()
	{
		INavigationGroup heroSelection = ((CampRoom)CombatSceneManager.Instance.Room).HeroSelection;
		INavigationGroup heroStampsDisplay = ((CampRoom)CombatSceneManager.Instance.Room).HeroSelection.heroStampsDisplay;
		INavigationGroup startingDeckSelection = ((CampRoom)CombatSceneManager.Instance.Room).HeroSelection.StartingDeckSelection;
		INavigationGroup hand = TilesManager.Instance.hand;
		UINavigationHelper.InitializeConnectedGroups(heroStampsDisplay, null, heroSelection);
		INavigationGroup down = hand;
		UINavigationHelper.InitializeConnectedGroups(heroSelection, heroStampsDisplay, down);
		UINavigationHelper.InitializeConnectedGroups(hand, heroSelection, null, startingDeckSelection);
		UINavigationHelper.InitializeConnectedGroups(startingDeckSelection, heroSelection, null, null, hand);
		base.CurrentGroup = heroSelection;
	}
}
