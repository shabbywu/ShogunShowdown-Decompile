using System.Collections.Generic;
using UINavigation;

namespace UINavigationScreens;

public class CombatNavigationScreen : NavigationScreen
{
	private INavigationGroup potionsContainerUI;

	private INavigationGroup hand;

	private INavigationGroup skillsIconsContainer;

	private INavigationGroup heroAttackQueue;

	public override bool AllowPrevNextNavigation => !Globals.Tutorial;

	public override bool AllowAlternativeNavigation => false;

	public override void Initialize()
	{
		potionsContainerUI = PotionsManager.Instance.PotionsContainerUI;
		hand = TilesManager.Instance.hand;
		skillsIconsContainer = ItemsManager.Instance.SkillsIconsContainer;
		heroAttackQueue = (HeroAttackQueue)Globals.Hero.AttackQueue;
		UINavigationHelper.InitializeConnectedGroups(hand, left: potionsContainerUI, up: heroAttackQueue);
		UINavigationHelper.InitializeConnectedGroups(potionsContainerUI, right: hand, up: skillsIconsContainer);
		UINavigationHelper.InitializeConnectedGroups(heroAttackQueue, down: hand, up: skillsIconsContainer);
		UINavigationHelper.InitializeConnectedGroups(skillsIconsContainer, null, heroAttackQueue);
		base.CurrentGroup = hand;
	}

	public void HandleHandStateWasUpdated()
	{
		if (base.CurrentGroup is HeroAttackQueue && Globals.Hero.AttackQueue.TCC.NTiles == 0)
		{
			INavigationGroup group = (HeroAttackQueue)Globals.Hero.AttackQueue;
			base.CurrentGroup = UINavigationHelper.HandleOutOfGroupNavigation(group, NavigationDirection.down, NavigationDirection.left);
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
		(INavigationGroup, INavigationTarget) tuple = UINavigationHelper.FindClosestGroupAndTarget(lastNavigationTarget, new List<INavigationGroup> { hand, heroAttackQueue });
		INavigationGroup item = tuple.Item1;
		INavigationTarget item2 = tuple.Item2;
		base.CurrentGroup = item;
		UINavigationHelper.SelectNewTarget(item, item2);
	}
}
