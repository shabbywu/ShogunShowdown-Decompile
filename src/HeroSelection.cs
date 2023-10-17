using System.Collections;
using System.Collections.Generic;
using InfoBoxUtils;
using TileEnums;
using UINavigation;
using UnityEngine;
using Utils;

public class HeroSelection : MonoBehaviour, IInfoBoxable, INavigationGroup
{
	public GameObject HerosContainer;

	public MyButton[] myButtons;

	public Hero[] heroes;

	public InfoBoxActivator infoBoxActivator;

	public HeroStampsDisplay heroStampsDisplay;

	public HeroSelectionSideBars heroSelectionSideBars;

	public GameObject difficultySelection;

	public MyButton goButton;

	[SerializeField]
	private DaySelection daySelection;

	[SerializeField]
	private HeroSelectorFrame heroSelectorFrame;

	[SerializeField]
	private StartingDeckSelection startingDeckSelection;

	[SerializeField]
	private MyButton nextHeroButton;

	[SerializeField]
	private MyButton prevHeroButton;

	[SerializeField]
	private MyButton nextDayButton;

	[SerializeField]
	private MyButton prevDayButton;

	public GameObject goTextBelowCell;

	private int iHero;

	private int iDeck;

	private bool heroSelected;

	public Animator heroSelectionUIAnimator;

	public float openDelay;

	private FamilyCrests familyCrests;

	private bool transitionInProgress;

	private int NHeros => heroes.Length;

	private bool DaySelectionActive
	{
		get
		{
			if (UnlocksManager.Instance.ShogunDefeated)
			{
				return Globals.Hero.Unlocked;
			}
			return false;
		}
	}

	private bool HeroStampsDisplayActive
	{
		get
		{
			if (UnlocksManager.Instance.ShogunDefeated)
			{
				return Globals.Hero.Unlocked;
			}
			return false;
		}
	}

	public StartingDeckSelection StartingDeckSelection => startingDeckSelection;

	public string InfoBoxText
	{
		get
		{
			if (Globals.Hero.Unlocked)
			{
				return Globals.Hero.InfoBoxText;
			}
			return TextUitls.ReplaceTags(string.Concat("[header_color]" + TextUitls.SingleLineHeader(LocalizationUtils.LocalizedString("Terms", "Locked")) + "[end_color]\n[vspace]", Globals.Hero.QuestForUnlocking.Description));
		}
	}

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.medium;

	public List<INavigationTarget> Targets => new List<INavigationTarget> { heroSelectorFrame, daySelection };

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => true;

	private void Awake()
	{
		familyCrests = ((Component)this).GetComponentInChildren<FamilyCrests>();
		Hero[] array = heroes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].LoadFromSaveData(SaveDataManager.Instance.saveData);
		}
	}

	private void Start()
	{
		familyCrests.Initialize(heroes);
		InitializeForNewHero(Globals.Hero);
		for (int i = 0; i < heroes.Length; i++)
		{
			if (heroes[i].HeroEnum == Globals.Hero.HeroEnum)
			{
				iHero = i;
			}
		}
		Close();
	}

	public void AgentEnters(Agent agent)
	{
		if (!transitionInProgress)
		{
			((MonoBehaviour)this).StartCoroutine(AgentEntersCoroutine());
		}
	}

	private IEnumerator AgentEntersCoroutine()
	{
		yield return (object)new WaitForSeconds(openDelay);
		Open();
	}

	public void AgentExits(Agent agent)
	{
		if (!transitionInProgress)
		{
			Close();
			if (!heroSelected)
			{
				agent.SetCombatUIActive(value: false);
			}
		}
	}

	private void Open()
	{
		goTextBelowCell.SetActive(false);
		heroSelectionSideBars.Show = true;
		Globals.Hero.SetCombatUIActive(value: true);
		infoBoxActivator.Open();
		MyButton[] array = myButtons;
		foreach (MyButton obj in array)
		{
			obj.Appear();
			obj.Interactable = true;
		}
		goButton.Interactable = true;
		daySelection.SetInitiallySelectedDayForHero(Globals.Hero);
		((Component)familyCrests).gameObject.SetActive(true);
		heroSelectionUIAnimator.SetBool("Open", true);
		daySelection.Open = DaySelectionActive;
		((Component)heroStampsDisplay).gameObject.SetActive(HeroStampsDisplayActive);
		startingDeckSelection.Open(Globals.Hero);
		EventsManager.Instance.BeginHeroSelection.Invoke();
	}

	private void Close()
	{
		startingDeckSelection.Close();
		infoBoxActivator.Close();
		MyButton[] array = myButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disappear();
		}
		((Component)familyCrests).gameObject.SetActive(false);
		heroSelectionUIAnimator.SetBool("Open", false);
		goButton.Interactable = false;
		daySelection.Open = false;
		((Component)heroStampsDisplay).gameObject.SetActive(false);
		if (!heroSelected)
		{
			goTextBelowCell.SetActive(true);
			heroSelectionSideBars.Show = false;
		}
		EventsManager.Instance.EndHeroSelection.Invoke();
	}

	public void GoToNextRoom()
	{
		if (!transitionInProgress)
		{
			heroSelected = true;
			Close();
			EventsManager.Instance.EndOfCombat.Invoke();
		}
	}

	public void HeroIndexUpdate(int i)
	{
		if (!transitionInProgress)
		{
			iHero += i;
			if (iHero >= NHeros)
			{
				iHero = 0;
			}
			if (iHero < 0)
			{
				iHero = NHeros - 1;
			}
			((MonoBehaviour)this).StartCoroutine(SwitchHeroCoroutine(heroes[iHero]));
		}
	}

	public void UpdateDeck(int delta)
	{
		if (!transitionInProgress)
		{
			int count = Globals.Hero.Decks.Count;
			iDeck += delta;
			if (iDeck >= count)
			{
				iDeck = 0;
			}
			if (iDeck < 0)
			{
				iDeck = count - 1;
			}
			UpdateHandForHero(Globals.Hero, iDeck);
		}
	}

	public void UpdateDay(int delta)
	{
		if (!transitionInProgress)
		{
			daySelection.UpdateDay(Globals.Day + delta);
		}
	}

	private IEnumerator SwitchHeroCoroutine(Hero hero)
	{
		transitionInProgress = true;
		CombatManager.Instance.AllowHeroAction = false;
		heroSelectionUIAnimator.SetTrigger("SwitchHero");
		SoundEffectsManager.Instance.Play("Spawn");
		infoBoxActivator.Close();
		daySelection.CloseInfoBox();
		familyCrests.UpdateForSelectedHero(heroes, hero);
		yield return (object)new WaitForSeconds(0.1f);
		InitializeForNewHero(hero);
		infoBoxActivator.Open();
		daySelection.Open = DaySelectionActive;
		((Component)heroStampsDisplay).gameObject.SetActive(HeroStampsDisplayActive);
		yield return (object)new WaitForSeconds(0.1f);
		transitionInProgress = false;
		CombatManager.Instance.AllowHeroAction = Globals.Hero.Unlocked;
		goButton.Interactable = Globals.Hero.Unlocked;
	}

	public void InitializeForNewHero(Hero hero)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		Cell cell = Globals.Hero.Cell;
		Dir facingDir = Globals.Hero.FacingDir;
		cell.Agent = null;
		((Component)Globals.Hero).transform.SetParent(HerosContainer.transform);
		((Component)Globals.Hero).transform.localPosition = Vector3.zero;
		Globals.Hero = hero;
		((Component)hero).transform.SetParent((Transform)null);
		hero.Cell = cell;
		hero.FacingDir = facingDir;
		((Component)hero).transform.position = ((Component)cell).transform.position;
		hero.LoadFromSaveData(SaveDataManager.Instance.saveData);
		hero.InitializeHP();
		iDeck = 0;
		UpdateHandForHero(hero, iDeck);
		if (!hero.Unlocked)
		{
			hero.SetLockedCharacterGraphics();
			hero.SetCombatUIActive(value: false);
		}
		familyCrests.UpdateForSelectedHero(heroes, hero);
		daySelection.SetInitiallySelectedDayForHero(hero);
		heroStampsDisplay.Initialize(hero);
		startingDeckSelection.UpdateState(hero);
		EventsManager.Instance.NewHeroSelected.Invoke();
	}

	private void UpdateHandForHero(Hero hero, int iDeckLocal)
	{
		TilesManager.Instance.hand.TCC.DestroyAllTiles();
		TilesManager.Instance.Deck = new List<Tile>();
		TilesManager.Instance.hand.Initialize(hero.InitialHandContainers);
		TilesManager.Instance.hand.Resize(hero.InitialHandContainers);
		if (hero.Unlocked)
		{
			AttackEnum[] array = hero.Decks[iDeckLocal];
			foreach (AttackEnum attackEnum in array)
			{
				Tile tile = TilesFactory.Instance.Create(attackEnum, hero.InitialTilesLevel);
				TilesManager.Instance.TakeTile(tile);
				tile.TileContainer.TeleportTileInContainer();
				tile.Interactable = false;
			}
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == daySelection)
		{
			if (navigationDirection == NavigationDirection.left)
			{
				prevDayButton.PressFromCode();
			}
			if (navigationDirection == NavigationDirection.right)
			{
				nextDayButton.PressFromCode();
			}
			if (navigationDirection == NavigationDirection.down)
			{
				UINavigationHelper.SelectNewTarget(this, heroSelectorFrame);
			}
			if (HeroStampsDisplayActive && navigationDirection == NavigationDirection.up)
			{
				return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
			}
		}
		else if (SelectedTarget == heroSelectorFrame)
		{
			if (navigationDirection == NavigationDirection.left)
			{
				prevHeroButton.PressFromCode();
			}
			if (navigationDirection == NavigationDirection.right)
			{
				nextHeroButton.PressFromCode();
			}
			if (navigationDirection == NavigationDirection.down)
			{
				return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
			}
			if (DaySelectionActive && navigationDirection == NavigationDirection.up)
			{
				UINavigationHelper.SelectNewTarget(this, daySelection);
			}
		}
		return this;
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		if (entryDirection == NavigationDirection.down)
		{
			UINavigationHelper.SelectNewTarget(this, daySelection);
		}
		else
		{
			UINavigationHelper.SelectNewTarget(this, heroSelectorFrame);
		}
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		return this;
	}
}
