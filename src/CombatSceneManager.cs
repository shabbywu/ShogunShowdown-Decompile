using System.Collections;
using System.Collections.Generic;
using AgentEnums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils;

public class CombatSceneManager : MonoBehaviour
{
	public enum Mode
	{
		combat,
		reward,
		transition,
		mapSelection
	}

	[SerializeField]
	private AgentsFactory agentsFactory;

	public Progression progression;

	public PauseMenu pauseMenu;

	public GameObject temporaryUI;

	public PostRoomFeedback postRoomFeedback;

	public FightBanner fightBanner;

	public MicroMap microMap;

	private bool firstUpdate = true;

	private Mode _currentMode = Mode.transition;

	public static CombatSceneManager Instance { get; private set; }

	public bool HeroIsAlive { get; set; } = true;


	public Room Room { get; private set; }

	public float RunTime { get; private set; }

	public SimpleCameraFollow CameraFollow { get; set; }

	public bool RunInProgress { get; private set; }

	public bool CanEnableInfoMode => IsCoreGameplay;

	public Mode CurrentMode
	{
		get
		{
			return _currentMode;
		}
		set
		{
			_currentMode = value;
			EventsManager.Instance.ModeSwitched.Invoke(_currentMode);
		}
	}

	public bool IsCoreGameplay
	{
		get
		{
			if ((Object)(object)Room == (Object)null || Room is CampRoom || CurrentMode == Mode.transition || CurrentMode == Mode.mapSelection || MapManager.Instance.mapScreen.IsOpen || MapManager.Instance.mapScreen.IsInTransition)
			{
				return false;
			}
			return true;
		}
	}

	public void EnableInfoMode()
	{
		Globals.FullInfoMode = true;
		EventsManager.Instance.InfoModeEnabled.Invoke();
	}

	public void DisableInfoMode()
	{
		Globals.FullInfoMode = false;
		EventsManager.Instance.InfoModeDisabled.Invoke();
	}

	public void TogglePause()
	{
		pauseMenu.TogglePause();
	}

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
		if (!Globals.GameInitialized)
		{
			SceneManager.LoadScene("GameInitialization");
			return;
		}
		agentsFactory.Initialize();
		InstatiateHero();
		UnlocksManager.Instance.LoadFromSaveData(SaveDataManager.Instance.saveData);
		Globals.KillCount = SaveDataManager.Instance.saveData.metaCurrency;
		progression.Initialize();
	}

	private void Start()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		SaveDataManager.Instance.StoreSaveData();
		SaveDataManager.Instance.StoreOptions();
		CameraFollow = ((Component)Camera.main).GetComponent<SimpleCameraFollow>();
		if (Globals.ContinueRun)
		{
			LoadStuffFromRunInProgressSaveData(SaveDataManager.Instance.runSaveData);
		}
		MusicManager.Instance.Play("Title");
		List<Tile> tiles = (Globals.ContinueRun ? TilesFactory.Instance.Create(SaveDataManager.Instance.runSaveData.deck) : Globals.Hero.InstantiateInitialTiles());
		TilesManager.Instance.InitializeDeck(tiles);
		TilesManager.Instance.CanInteractWithTiles = false;
		Room = progression.BuildRoom(Vector3.zero, Globals.ContinueRun);
		if (Globals.ContinueRun)
		{
			Globals.Hero.Cell = Room.Grid.Cells[SaveDataManager.Instance.runSaveData.hero.iCell];
			Globals.Hero.LoadFromSaveData(SaveDataManager.Instance.runSaveData.hero);
		}
		else
		{
			Globals.Hero.Cell = Room.Grid.initialPlayerCell;
			Globals.Hero.FacingDir = Dir.right;
		}
		Globals.Hero.SetPositionToCellPosition();
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(EndOfCombat));
		EventsManager.Instance.BeginningOfCombatTurn.AddListener(new UnityAction(BeginningOfCombatTurn));
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(BeginRun));
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
		EventsManager.Instance.SaveRunProgress.AddListener(new UnityAction(SaveRunProgress));
		((MonoBehaviour)this).StartCoroutine(OpeningTransition());
		if (!Globals.ContinueRun)
		{
			RunTime = 0f;
			if (MusicManager.Instance.CurrentTrack.name != "Title")
			{
				MusicManager.Instance.Play("Title");
			}
		}
	}

	private void Update()
	{
		if (firstUpdate)
		{
			FirstUpdateInitializations();
			firstUpdate = false;
		}
		if (RunInProgress && CombatManager.Instance.CombatInProgress)
		{
			RunTime += Time.deltaTime;
		}
	}

	public void BeginRun()
	{
		TilesFactory.Instance.InitializeRandomGenerator();
		Globals.FirstEverRun = false;
		Globals.NRuns++;
		RunInProgress = true;
		SaveDataManager.Instance.saveData.lastHeroUsed = Globals.Hero.HeroEnum;
		SaveDataManager.Instance.StoreSaveData();
	}

	public void GoToNextRoom()
	{
		((MonoBehaviour)this).StartCoroutine(GoToNextRoomCoroutine());
	}

	private IEnumerator GoToNextRoomCoroutine()
	{
		Room previousRoom = Room;
		if (progression.IsLastRoomInLocation)
		{
			CurrentMode = Mode.mapSelection;
			microMap.Clear();
			EventsManager.Instance.SaveRunProgress.Invoke();
			yield return ((MonoBehaviour)this).StartCoroutine(MapManager.Instance.NexLocationSelection());
			microMap.Initialize(progression.CurrentLocation.Rooms);
		}
		else
		{
			progression.Next();
		}
		Room = progression.BuildRoom(((Component)previousRoom).transform.position + RoomTransitionsManager.roomsDeltaX * Vector3.right);
		PlayMusicForRoom();
		Globals.Hero.Cell = Room.Grid.initialPlayerCell;
		CurrentMode = Mode.transition;
		yield return RoomTransitionsManager.Instance.RoomToRoomTransition(previousRoom, Room, CameraFollow);
		if (MapManager.Instance.mapScreen.IsOpen)
		{
			MapManager.Instance.CloseMap();
		}
		yield return EnterRoomCoroutine();
	}

	public void EndOfCombat()
	{
		((MonoBehaviour)this).StartCoroutine(ExitRoomCoroutine());
	}

	private IEnumerator OpeningTransition()
	{
		if (!Globals.Quick)
		{
			Transform handParent = ((Component)TilesManager.Instance.hand).transform.parent;
			((Component)TilesManager.Instance.hand).transform.SetParent((Transform)null);
			CameraFollow.SetInitialPosition();
			yield return (object)new WaitForSeconds(0.5f);
			Globals.Hero.SetCombatUIActive(value: false);
			yield return ((MonoBehaviour)this).StartCoroutine(CameraFollow.EnableOpeningTransition());
			((Component)TilesManager.Instance.hand).transform.SetParent(handParent, false);
		}
		if (Globals.ContinueRun && SaveDataManager.Instance.runSaveData.mapSelectionInProgress)
		{
			((MonoBehaviour)this).StartCoroutine(GoToNextRoomCoroutine());
			yield break;
		}
		((MonoBehaviour)this).StartCoroutine(EnterRoomCoroutine());
		PlayMusicForRoom();
		microMap.Initialize(progression.CurrentLocation.Rooms);
	}

	private IEnumerator EnterRoomCoroutine()
	{
		CurrentMode = Mode.transition;
		yield return (object)new WaitForSeconds(0.1f);
		EventsManager.Instance.EnterRoom.Invoke(Room);
		if (!Globals.QuickTransitions)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(fightBanner.BeginFightTransition(Room));
		}
		if (Room.CameraMode == SimpleCameraFollow.CameraMode.followHero)
		{
			CameraFollow.EnableSmoothDampFollow(((Component)Room).transform.position.x);
		}
		Room.Begin();
		Globals.Hero.PopulateSaveData(SaveDataManager.Instance.saveData);
		EventsManager.Instance.SaveRunProgress.Invoke();
	}

	private IEnumerator ExitRoomCoroutine()
	{
		bool win = progression.IsLastLevel;
		CurrentMode = Mode.transition;
		yield return ((MonoBehaviour)this).StartCoroutine(Room.PickUpAllPickUps());
		if (!win && !Globals.QuickTransitions)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(fightBanner.EndFightTransition(Room));
		}
		if (Globals.FeedbackMenu)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(postRoomFeedback.WaitForFeedback());
		}
		if (progression.CurrentLocation.isLastCombatLocationInIsland && Room is BossRoom)
		{
			EventsManager.Instance.IslandCleared.Invoke((progression.CurrentLocation.island, Globals.Day));
		}
		if (win)
		{
			EventsManager.Instance.GameOver.Invoke(win);
			yield break;
		}
		EventsManager.Instance.ExitRoom.Invoke(Room);
		GoToNextRoom();
	}

	public void GameOver(bool win)
	{
		SaveDataManager.Instance.DeleteStoredRunSaveData();
		CombatManager.Instance.CombatInProgress = false;
		HeroIsAlive = false;
		TilesManager.Instance.CanInteractWithTiles = false;
	}

	private void FirstUpdateInitializations()
	{
		Globals.Coins = Globals.Coins;
		Globals.KillCount = Globals.KillCount;
		if (!Progression.Instance.CurrentLocation.isCamp && !Globals.Tutorial)
		{
			EventsManager.Instance.BeginRun.Invoke();
		}
	}

	private void BeginningOfCombatTurn()
	{
		if (Globals.FullInfoMode)
		{
			DisableInfoMode();
		}
	}

	private void LoadStuffFromRunInProgressSaveData(RunSaveData runSaveData)
	{
		ItemsManager.Instance.LoadFromSaveData(runSaveData);
		PotionsManager.Instance.LoadFromSaveData(runSaveData);
		MetricsManager.Instance.runMetrics.runStats = runSaveData.runStats;
		Globals.Day = runSaveData.runStats.day;
		Globals.Coins = runSaveData.runStats.coins;
		RunTime = runSaveData.runStats.time;
	}

	public void SaveRunProgress()
	{
		if (!Globals.AutoSave || Globals.Tutorial || Progression.Instance.CurrentLocation.isCamp)
		{
			return;
		}
		RunSaveData runSaveData = new RunSaveData();
		runSaveData.hasRunInProgress = true;
		runSaveData.mapSelectionInProgress = CurrentMode == Mode.mapSelection;
		MetricsManager.Instance.UpdateRunStats();
		runSaveData.runStats = MetricsManager.Instance.runMetrics.runStats;
		MapManager.Instance.map.PopulateSaveData(runSaveData.mapSaveData);
		progression.PopulateSaveData(runSaveData.progressionSaveData);
		foreach (Tile item in TilesManager.Instance.Deck)
		{
			runSaveData.deck.Add(item.GetTileSaveData());
		}
		PotionsManager.Instance.PopulateSaveData(runSaveData);
		ItemsManager.Instance.PopulateSaveData(runSaveData);
		Room.PopulateSaveData(runSaveData);
		runSaveData.hero = Globals.Hero.GetHeroCombatSaveData();
		SaveDataManager.Instance.runSaveData = runSaveData;
		SaveDataManager.Instance.StoreRunSaveData();
	}

	private void InstatiateHero()
	{
		HeroEnum heroEnum = (Globals.ContinueRun ? SaveDataManager.Instance.runSaveData.hero.heroEnum : SaveDataManager.Instance.saveData.lastHeroUsed);
		Globals.Hero = agentsFactory.InstantiateHero(heroEnum);
		Globals.Hero.LoadFromSaveData(SaveDataManager.Instance.saveData);
	}

	private void PlayMusicForRoom()
	{
		if (Room is WaveRoom)
		{
			MusicManager.Instance.Play(progression.CurrentLocation.waveMusicTrackName);
		}
		else if (Room is ShogunBossRoom)
		{
			MusicManager.Instance.Play("ShogunShowdown");
		}
		else if (Room is BossRoom)
		{
			MusicManager.Instance.Play("Boss");
		}
		else if (Room is ShopRoom)
		{
			MusicManager.Instance.Play("Shop");
		}
		else if (Room is CampRoom)
		{
			MusicManager.Instance.Play("Title");
		}
	}
}
