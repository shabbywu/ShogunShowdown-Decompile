using UINavigation;

namespace UINavigationScreens;

public class ShopNavigationScreen : NavigationScreen
{
	private INavigationGroup shop;

	private INavigationGroup hand;

	private INavigationGroup potionsContainerUI;

	private INavigationGroup skillsIconsContainer;

	private INavigationGroup tileUpgradeInShop;

	public override bool AllowAlternativeNavigation => true;

	public override bool AllowPrevNextNavigation => false;

	public override void Initialize()
	{
		ShopRoom shopRoom = (ShopRoom)CombatSceneManager.Instance.Room;
		shop = shopRoom.Shop;
		hand = TilesManager.Instance.hand;
		potionsContainerUI = PotionsManager.Instance.PotionsContainerUI;
		skillsIconsContainer = ItemsManager.Instance.SkillsIconsContainer;
		tileUpgradeInShop = shopRoom.TileUpgradeReward;
		UINavigationHelper.InitializeConnectedGroups(hand, left: potionsContainerUI, up: tileUpgradeInShop);
		UINavigationHelper.InitializeConnectedGroups(tileUpgradeInShop, left: skillsIconsContainer, up: shop, right: shop, down: hand);
		UINavigationHelper.InitializeConnectedGroups(potionsContainerUI, right: hand, up: skillsIconsContainer);
		UINavigationHelper.InitializeConnectedGroups(shop, down: hand, left: tileUpgradeInShop, up: skillsIconsContainer);
		UINavigationHelper.InitializeConnectedGroups(skillsIconsContainer, null, hand, right: shop, left: potionsContainerUI);
		base.CurrentGroup = hand;
	}
}
