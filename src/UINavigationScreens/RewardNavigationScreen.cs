using System.Collections.Generic;
using UINavigation;

namespace UINavigationScreens;

public class RewardNavigationScreen : NavigationScreen
{
	private INavigationGroup hand;

	private INavigationGroup potionsContainerUI;

	private INavigationGroup skillsIconsContainer;

	private INavigationGroup rewardNavGroup;

	public override bool AllowPrevNextNavigation => true;

	public override bool AllowAlternativeNavigation => true;

	public override void Initialize()
	{
		hand = TilesManager.Instance.hand;
		potionsContainerUI = PotionsManager.Instance.PotionsContainerUI;
		skillsIconsContainer = ItemsManager.Instance.SkillsIconsContainer;
		Reward reward = ((RewardRoom)CombatSceneManager.Instance.Room).Reward;
		if (!reward.Exausted)
		{
			rewardNavGroup = null;
			if (reward is NewTileReward newTileReward)
			{
				rewardNavGroup = newTileReward;
				base.CurrentGroup = rewardNavGroup;
			}
			else if (reward is TileUpgradeReward tileUpgradeReward)
			{
				rewardNavGroup = tileUpgradeReward;
				base.CurrentGroup = hand;
			}
			UINavigationHelper.InitializeConnectedGroups(hand, left: potionsContainerUI, up: rewardNavGroup);
			UINavigationHelper.InitializeConnectedGroups(potionsContainerUI, right: hand, up: skillsIconsContainer);
			UINavigationHelper.InitializeConnectedGroups(rewardNavGroup, down: hand, left: potionsContainerUI, up: skillsIconsContainer);
			UINavigationHelper.InitializeConnectedGroups(skillsIconsContainer, null, rewardNavGroup, null, rewardNavGroup);
		}
	}

	public override void ReActivate(INavigationTarget lastNavigationTarget = null)
	{
		if (lastNavigationTarget == null)
		{
			base.ReActivate();
			return;
		}
		base.Interactable = true;
		(INavigationGroup, INavigationTarget) tuple = UINavigationHelper.FindClosestGroupAndTarget(lastNavigationTarget, new List<INavigationGroup> { hand, rewardNavGroup });
		INavigationGroup item = tuple.Item1;
		INavigationTarget item2 = tuple.Item2;
		base.CurrentGroup = item;
		UINavigationHelper.SelectNewTarget(item, item2);
	}
}
