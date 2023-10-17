using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UINavigationScreens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CombatSceneUINavigationManager : MonoBehaviour
{
	private readonly List<NavigationScreen> navigationScreens = new List<NavigationScreen>();

	private bool mainNavigateInputInProgress;

	private bool holdingTileGrab;

	private AutorepeatNavigationHandler navigationAutorepeatHandler;

	private AutorepeatPrevNextHandler autorepeatPrevNextHandler;

	private HeroAttackQueue HeroAttackQueue => (HeroAttackQueue)Globals.Hero.AttackQueue;

	private NavigationScreen CurrentNavigationScreen
	{
		get
		{
			if (navigationScreens.Count <= 0)
			{
				return null;
			}
			return navigationScreens.Last();
		}
	}

	private INavigationTarget LastNavigationTarget { get; set; }

	private bool InUse
	{
		get
		{
			if (Globals.Options.controlScheme != Options.ControlScheme.Keyboard)
			{
				return Globals.Options.controlScheme == Options.ControlScheme.Gamepad;
			}
			return true;
		}
	}

	private bool CurrentlyInteractable
	{
		get
		{
			if (CurrentNavigationScreen != null)
			{
				return CurrentNavigationScreen.Interactable;
			}
			return false;
		}
	}

	public bool AllowAlternativeNavigation
	{
		get
		{
			if (CurrentNavigationScreen != null)
			{
				return CurrentNavigationScreen.AllowAlternativeNavigation;
			}
			return false;
		}
	}

	private void Awake()
	{
		navigationAutorepeatHandler = ((Component)this).GetComponent<AutorepeatNavigationHandler>();
		navigationAutorepeatHandler.Initialize(ProcessTileNavigationInput);
		autorepeatPrevNextHandler = ((Component)this).GetComponent<AutorepeatPrevNextHandler>();
		autorepeatPrevNextHandler.Initialize(ProcessPrevNavigation, ProcessNextNavigation);
	}

	private void Start()
	{
		InitializeEventsListeners();
	}

	public void OnTileSubmit(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && CurrentlyInteractable && !CombatManager.Instance.TurnInProgress)
		{
			CurrentNavigationScreen.SubmitCurrentTarget();
		}
	}

	public void OnTileNavigate(CallbackContext context)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (((CallbackContext)(ref context)).performed)
		{
			mainNavigateInputInProgress = true;
		}
		else if (((CallbackContext)(ref context)).canceled)
		{
			mainNavigateInputInProgress = false;
		}
		navigationAutorepeatHandler.OnNavigate(context);
	}

	public void OnAlternativeTileNavigate(CallbackContext context)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (AllowAlternativeNavigation && !mainNavigateInputInProgress)
		{
			navigationAutorepeatHandler.OnNavigate(context);
		}
	}

	public void OnTileGrab(CallbackContext context)
	{
		holdingTileGrab = ((CallbackContext)(ref context)).performed;
		if (CurrentlyInteractable && ((CallbackContext)(ref context)).performed && CurrentNavigationScreen.CurrentGroup is Hand && HeroAttackQueue.NTiles > 0)
		{
			CurrentNavigationScreen.Navigate(NavigationDirection.up);
		}
	}

	public void OnPrev(CallbackContext context)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		autorepeatPrevNextHandler.OnPrev(context);
	}

	public void OnNext(CallbackContext context)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		autorepeatPrevNextHandler.OnNext(context);
	}

	public void SwapQueueUp()
	{
		if (CurrentNavigationScreen.CurrentGroup is HeroAttackQueue)
		{
			HeroAttackQueue.SwapQueue(1);
		}
	}

	public void SwapQueueDown()
	{
		if (CurrentNavigationScreen.CurrentGroup is HeroAttackQueue)
		{
			HeroAttackQueue.SwapQueue(-1);
		}
	}

	private void ProcessNextNavigation()
	{
		if (CurrentlyInteractable)
		{
			if (holdingTileGrab)
			{
				SwapQueueUp();
			}
			else
			{
				CurrentNavigationScreen.NavigateNext();
			}
		}
	}

	private void ProcessPrevNavigation()
	{
		if (CurrentlyInteractable)
		{
			if (holdingTileGrab)
			{
				SwapQueueDown();
			}
			else
			{
				CurrentNavigationScreen.NavigatePrev();
			}
		}
	}

	private void ProcessTileNavigationInput(CallbackContext context)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (!CurrentlyInteractable)
		{
			return;
		}
		int num = (int)((CallbackContext)(ref context)).ReadValue<Vector2>().x;
		int num2 = (int)((CallbackContext)(ref context)).ReadValue<Vector2>().y;
		if (num == 0 && num2 == 0)
		{
			return;
		}
		if (holdingTileGrab)
		{
			if (num2 > 0)
			{
				SwapQueueUp();
			}
			if (num2 < 0)
			{
				SwapQueueDown();
			}
		}
		else
		{
			CurrentNavigationScreen.Navigate(UINavigationHelper.DirectionFromXY(num, num2));
		}
	}

	private void InitializeEventsListeners()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		EventsManager.Instance.NavigationTargetChanged.AddListener((UnityAction<INavigationTarget>)NavigationTargetChanged);
		EventsManager.Instance.HandStateWasUpdated.AddListener(new UnityAction(HandStateWasUpdated));
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(EnterNavigationScreen<CombatNavigationScreen>));
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.RewardReady.AddListener(new UnityAction(OnRewardReady));
		EventsManager.Instance.RewardBusy.AddListener(new UnityAction(OnRewardBusy));
		EventsManager.Instance.RewardRoomEnd.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.MapOpened.AddListener(new UnityAction(EnterNavigationScreen<MapNavigationScreen>));
		EventsManager.Instance.MapCurrentLocationCleared.AddListener(new UnityAction(RefreshNavigationScreen));
		EventsManager.Instance.MapClosed.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.ShopBegin.AddListener(new UnityAction(EnterNavigationScreen<ShopNavigationScreen>));
		EventsManager.Instance.ShopEnd.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.UnlocksShopBegin.AddListener(new UnityAction(EnterNavigationScreen<UnlockShopNavigationScreen>));
		EventsManager.Instance.ArchiveOpened.AddListener(new UnityAction(EnterNavigationScreen<ArchiveNavigationScreen>));
		EventsManager.Instance.BeginHeroSelection.AddListener(new UnityAction(EnterNavigationScreen<HeroSelectionNavigationScreen>));
		EventsManager.Instance.EndHeroSelection.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.ArchiveClosed.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.InfoModeEnabled.AddListener(new UnityAction(EnterNavigationScreen<FullInfoModeNavigationScreen>));
		EventsManager.Instance.InfoModeDisabled.AddListener(new UnityAction(ExitNavigationScreen));
		EventsManager.Instance.VictoryScreenStampDisplaySequenceOver.AddListener((UnityAction<HeroStampsDisplay>)InitializeForVictoryScreenStampDisplaySequence);
	}

	private void EnterNavigationScreen(NavigationScreen navigationScreen)
	{
		CurrentNavigationScreen?.Deactivate();
		navigationScreen.Initialize();
		navigationScreen.Activate(LastNavigationTarget);
		navigationScreens.Add(navigationScreen);
	}

	private void ExitNavigationScreen()
	{
		if (CurrentNavigationScreen != null)
		{
			CurrentNavigationScreen.Deactivate();
			navigationScreens.RemoveAt(navigationScreens.Count - 1);
		}
		CurrentNavigationScreen?.ReActivate(LastNavigationTarget);
	}

	private void EnterNavigationScreen<T>() where T : NavigationScreen, new()
	{
		if (InUse)
		{
			EnterNavigationScreen(new T());
		}
	}

	private void RefreshNavigationScreen()
	{
		if (InUse)
		{
			CurrentNavigationScreen?.Refresh();
		}
	}

	private void OnRewardReady()
	{
		if (CombatSceneManager.Instance.Room is RewardRoom)
		{
			EnterNavigationScreen<RewardNavigationScreen>();
		}
	}

	private void OnRewardBusy()
	{
		if (CombatSceneManager.Instance.Room is RewardRoom)
		{
			ExitNavigationScreen();
		}
	}

	private void InitializeForVictoryScreenStampDisplaySequence(HeroStampsDisplay stampsDisplay)
	{
		if (InUse)
		{
			VictoryScreenStampDisplaySequenceNavigationScreen navigationScreen = new VictoryScreenStampDisplaySequenceNavigationScreen
			{
				StampsDisplay = stampsDisplay
			};
			EnterNavigationScreen(navigationScreen);
		}
	}

	private void HandStateWasUpdated()
	{
		if (CurrentlyInteractable && TilesManager.Instance.CanInteractWithTiles)
		{
			if (CurrentNavigationScreen is CombatNavigationScreen combatNavigationScreen)
			{
				combatNavigationScreen.HandleHandStateWasUpdated();
			}
			if (CurrentNavigationScreen.CurrentGroup is Hand)
			{
				TilesManager.Instance.hand.UpdateSelectedNavigateTargetAfterHandStateUpdate();
			}
		}
	}

	private void NavigationTargetChanged(INavigationTarget target)
	{
		LastNavigationTarget = target;
	}
}
