using System.Collections.Generic;
using InfoBoxUtils;
using UINavigation;
using UnityEngine;
using Utils;

public class StartingDeckSelection : MonoBehaviour, IInfoBoxable, INavigationGroup
{
	[SerializeField]
	private MyButton button;

	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	private Hero hero;

	private Vector3 handLocalPosition = new Vector3(-1.5f, 0f, 0f);

	private HeroSelection heroSelection;

	private bool AlternaticeDeckEnabled => hero.CharacterSaveData.stamps.Count >= 3;

	public string InfoBoxText => LocalizationUtils.LocalizedString("Metaprogression", "AlternativeStartingDeckUnlocking");

	public bool InfoBoxEnabled => true;

	public BoxWidth BoxWidth => BoxWidth.small;

	public List<INavigationTarget> Targets => new List<INavigationTarget> { button };

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => UnlocksManager.Instance.ShogunDefeated;

	private void Awake()
	{
		heroSelection = ((Component)this).GetComponentInParent<HeroSelection>();
	}

	public void Open(Hero hero)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (!UnlocksManager.Instance.ShogunDefeated)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		UpdateState(hero);
		((Component)this).gameObject.SetActive(true);
		((Component)this).transform.SetParent(((Component)TilesManager.Instance.hand).transform);
		((Component)this).transform.localPosition = handLocalPosition;
	}

	public void Close()
	{
		((Component)this).gameObject.SetActive(false);
		((Component)this).transform.SetParent(((Component)heroSelection).transform);
	}

	public void UpdateState(Hero hero)
	{
		this.hero = hero;
		((Component)button).gameObject.SetActive(hero.Unlocked);
		((Component)infoBoxActivator).gameObject.SetActive(hero.Unlocked && !AlternaticeDeckEnabled);
		button.Interactable = AlternaticeDeckEnabled;
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (navigationDirection == NavigationDirection.up || navigationDirection == NavigationDirection.right)
		{
			if (((Component)infoBoxActivator).gameObject.activeSelf)
			{
				infoBoxActivator.Close();
			}
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		return this;
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		UINavigationHelper.SelectNewTarget(this, button);
		if (((Component)infoBoxActivator).gameObject.activeSelf)
		{
			infoBoxActivator.Open();
		}
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		if (button.Interactable)
		{
			button.Click();
		}
		return this;
	}
}
