using ProgressionEnums;
using UINavigation;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
	public static EventsManager Instance { get; private set; }

	public UnityEvent BeginRun { get; private set; } = new UnityEvent();


	public UpdateValueEvent CoinsUpdate { get; private set; } = new UpdateValueEvent();


	public UpdateValueEvent MetaCurrencyUpdate { get; private set; } = new UpdateValueEvent();


	public UnityEvent EndOfCombatTurn { get; private set; } = new UnityEvent();


	public UnityEvent BeginningOfCombatTurn { get; private set; } = new UnityEvent();


	public UnityEvent<Room> EnterRoom { get; private set; } = new UnityEvent<Room>();


	public UnityEvent<Room> ExitRoom { get; private set; } = new UnityEvent<Room>();


	public UnityEvent BeginningOfCombat { get; private set; } = new UnityEvent();


	public UnityEvent EndOfCombat { get; private set; } = new UnityEvent();


	public UnityEvent<Wave> NewWaveSpawns { get; private set; } = new UnityEvent<Wave>();


	public UnityEvent BeginBossFight { get; private set; } = new UnityEvent();


	public UnityEvent EndBossFight { get; private set; } = new UnityEvent();


	public UnityEvent<(IslandEnum, int)> IslandCleared { get; private set; } = new UnityEvent<(IslandEnum, int)>();


	public UnityEvent<HeroStamp> HeroStampObtained { get; private set; } = new UnityEvent<HeroStamp>();


	public UnityEvent<bool> GameOver { get; private set; } = new UnityEvent<bool>();


	public UnityEvent InfoModeEnabled { get; private set; } = new UnityEvent();


	public UnityEvent InfoModeDisabled { get; private set; } = new UnityEvent();


	public UnityEvent<Enemy> EnemyDied { get; private set; } = new UnityEvent<Enemy>();


	public UnityEvent<Boss> BossDied { get; private set; } = new UnityEvent<Boss>();


	public UnityEvent EnemyFriendlyKill { get; private set; } = new UnityEvent();


	public EnemyKilledEvent ComboKill { get; private set; } = new EnemyKilledEvent();


	public UnityEvent<Agent> PreciseKill { get; private set; } = new UnityEvent<Agent>();


	public UnityEvent<Pickup> PickupCreated { get; private set; } = new UnityEvent<Pickup>();


	public UnityEvent<Pickup> PickupPickedUp { get; private set; } = new UnityEvent<Pickup>();


	public UnityEvent<Tile> TileUpgraded { get; private set; } = new UnityEvent<Tile>();


	public UnityEvent<Tile> NewTilePicked { get; private set; } = new UnityEvent<Tile>();


	public UnityEvent<string> ShopItemBought { get; private set; } = new UnityEvent<string>();


	public UnityEvent ShopBegin { get; private set; } = new UnityEvent();


	public UnityEvent UnlocksShopBegin { get; private set; } = new UnityEvent();


	public UnityEvent ShopEnd { get; private set; } = new UnityEvent();


	public UnityEvent<string> PlayerChoiceInNewTileReward { get; private set; } = new UnityEvent<string>();


	public UnityEvent<string> PlayerChoiceInTileUpgradeReward { get; private set; } = new UnityEvent<string>();


	public AttackEvent Attack { get; private set; } = new AttackEvent();


	public UnityEvent NewHeroSelected { get; private set; } = new UnityEvent();


	public UnityEvent NewDifficultySelected { get; private set; } = new UnityEvent();


	public UnityEvent BeginHeroSelection { get; private set; } = new UnityEvent();


	public UnityEvent EndHeroSelection { get; private set; } = new UnityEvent();


	public UnityEvent OpenOptionsMenu { get; private set; } = new UnityEvent();


	public UnityEvent CloseOptionsMenu { get; private set; } = new UnityEvent();


	public UnityEvent ArchiveOpened { get; private set; } = new UnityEvent();


	public UnityEvent ArchiveClosed { get; private set; } = new UnityEvent();


	public UnityEvent SaveRunProgress { get; private set; } = new UnityEvent();


	public UnityEvent<string> LogToGameScreen { get; private set; } = new UnityEvent<string>();


	public UnityEvent<HeroStampsDisplay> VictoryScreenStampDisplaySequenceOver { get; private set; } = new UnityEvent<HeroStampsDisplay>();


	public UnityEvent HeroPerformedMoveAttack { get; private set; } = new UnityEvent();


	public UnityEvent HeroPlayedAttack { get; private set; } = new UnityEvent();


	public UnityEvent HeroSpecialMove { get; private set; } = new UnityEvent();


	public UnityEvent HeroTurnedAround { get; private set; } = new UnityEvent();


	public UnityEvent HeroWaited { get; private set; } = new UnityEvent();


	public UnityEvent<AttackQueue> HeroAttacks { get; private set; } = new UnityEvent<AttackQueue>();


	public UnityEvent<string> PushNotification { get; private set; } = new UnityEvent<string>();


	public UnityEvent<Potion> PotionUsed { get; private set; } = new UnityEvent<Potion>();


	public UnityEvent<(Hit hit, Agent attacker)> HeroIsHit { get; private set; } = new UnityEvent<(Hit, Agent)>();


	public UnityEvent<int> HeroTookDamage { get; private set; } = new UnityEvent<int>();


	public UnityEvent<int> HeroDealtDamage { get; private set; } = new UnityEvent<int>();


	public UpdateValueEvent HeroHPUpdate { get; private set; } = new UpdateValueEvent();


	public UpdateValueEvent HeroMaxHPUpdate { get; private set; } = new UpdateValueEvent();


	public UnityEvent RewardsScreenUpgradeChoosen { get; private set; } = new UnityEvent();


	public UnityEvent RewardReady { get; private set; } = new UnityEvent();


	public UnityEvent RewardBusy { get; private set; } = new UnityEvent();


	public UnityEvent RewardRoomBegin { get; private set; } = new UnityEvent();


	public UnityEvent RewardRoomEnd { get; private set; } = new UnityEvent();


	public UnityEvent MapDestinationReached { get; private set; } = new UnityEvent();


	public UnityEvent MapOpened { get; private set; } = new UnityEvent();


	public UnityEvent MapClosed { get; private set; } = new UnityEvent();


	public UnityEvent MapActivated { get; private set; } = new UnityEvent();


	public UnityEvent MapDeactivated { get; private set; } = new UnityEvent();


	public UnityEvent MapCurrentLocationCleared { get; private set; } = new UnityEvent();


	public UnityEvent TileInteractionEnabled { get; private set; } = new UnityEvent();


	public UnityEvent TileInteractionDisabled { get; private set; } = new UnityEvent();


	public UnityEvent<Options.ControlScheme> ControlsSchemeUpdated { get; private set; } = new UnityEvent<Options.ControlScheme>();


	public UnityEvent<Options.ControlScheme> ControlSchemeForMenuNavigationUpdated { get; private set; } = new UnityEvent<Options.ControlScheme>();


	public UnityEvent ColorblindModeUpdated { get; private set; } = new UnityEvent();


	public UnityEvent<TileUpgrade> BeginTileUpgradeMode { get; private set; } = new UnityEvent<TileUpgrade>();


	public UnityEvent EndTileUpgradeMode { get; private set; } = new UnityEvent();


	public UnityEvent HandStateWasUpdated { get; private set; } = new UnityEvent();


	public UnityEvent<CombatSceneManager.Mode> ModeSwitched { get; private set; } = new UnityEvent<CombatSceneManager.Mode>();


	public UnityEvent<INavigationTarget> NavigationTargetChanged { get; private set; } = new UnityEvent<INavigationTarget>();


	public UnityEvent<bool> InputActionButtonBindersEnabled { get; private set; } = new UnityEvent<bool>();


	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}
}
