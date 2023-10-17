using Steamworks;
using UnityEngine;
using UnityEngine.Events;

public static class Globals
{
	public enum Platform
	{
		pc,
		steamdeck
	}

	private static int coins;

	private static int maxCoins = 999;

	private static int killCount;

	public static bool Developer { get; set; } = false;


	public static bool Quick { get; set; } = false;


	public static bool SkipTitleScreen { get; set; } = false;


	public static bool ShortLocations { get; set; } = false;


	public static bool RecordRunHistory { get; set; } = false;


	public static bool UseUnityAnalytics { get; set; } = false;


	public static bool AutoSave { get; set; } = true;


	public static bool QuickTransitions => Quick;

	public static bool QuickAnimations => Quick;

	public static bool AllowFullUnlockInTitleScreen { get; private set; } = false;


	public static bool FeedbackMenu { get; set; } = false;


	public static bool GameInitialized { get; set; }

	public static bool FirstEverRun { get; set; }

	public static Platform CurrentPlatform { get; set; }

	public static Options Options { get; set; } = new Options();


	public static Hero Hero { get; set; }

	public static bool Tutorial { get; set; } = false;


	public static bool InCamp { get; set; } = false;


	public static bool FullInfoMode { get; set; } = false;


	public static bool TilesInfoMode { get; set; } = false;


	public static bool GamepadTilesInfoMode { get; set; } = false;


	public static int Day { get; set; } = 1;


	public static int CurrentlyImplementedMaxDay { get; } = 5;


	public static int NRuns { get; set; }

	public static bool ContinueRun { get; set; }

	public static int Coins
	{
		get
		{
			return coins;
		}
		set
		{
			coins = value;
			coins = Mathf.Clamp(coins, 0, maxCoins);
			if ((Object)(object)EventsManager.Instance != (Object)null)
			{
				((UnityEvent<int>)EventsManager.Instance.CoinsUpdate).Invoke(coins);
			}
		}
	}

	public static int KillCount
	{
		get
		{
			return killCount;
		}
		set
		{
			killCount = value;
			killCount = Mathf.Clamp(killCount, 0, 9999);
			if ((Object)(object)EventsManager.Instance != (Object)null)
			{
				((UnityEvent<int>)EventsManager.Instance.MetaCurrencyUpdate).Invoke(killCount);
			}
		}
	}

	public static int MaxRoomSize => 11;

	public static void InitGlobalSettings()
	{
		QualitySettings.vSyncCount = 1;
		CurrentPlatform = Platform.pc;
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			CurrentPlatform = Platform.steamdeck;
		}
	}

	public static void SetInitialValues()
	{
		Day = 1;
		Coins = 10;
		ContinueRun = false;
		Tutorial = false;
		InCamp = false;
		FullInfoMode = false;
		TilesInfoMode = false;
		GamepadTilesInfoMode = false;
	}

	public static void QuitGame()
	{
		Application.Quit();
	}
}
