using System.Collections;
using AgentEnums;
using PickupEnums;
using SkillEnums;
using TileEnums;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnlocksID;
using Utils;

public class GameInitialization : MonoBehaviour
{
	[SerializeField]
	private AudioMixerSnapshot unpausedAudioMixerSnapshot;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(InitializeAndSwitchScene());
	}

	private IEnumerator InitializeAndSwitchScene()
	{
		Globals.GameInitialized = true;
		Globals.SetInitialValues();
		Globals.InitGlobalSettings();
		Globals.KillCount = SaveDataManager.Instance.saveData.metaCurrency;
		Globals.NRuns = SaveDataManager.Instance.saveData.nRuns;
		PlatformSpecificInitialization();
		if (Globals.UseUnityAnalytics)
		{
			InitializeAnalytics();
		}
		LeanTween.reset();
		if (Globals.Developer)
		{
			Validations();
		}
		unpausedAudioMixerSnapshot.TransitionTo(0f);
		yield return LocalizationSettings.InitializationOperation;
		if (Globals.SkipTitleScreen)
		{
			SceneManager.LoadScene("Combat");
		}
		else
		{
			SceneManager.LoadScene("TitleScreen");
		}
	}

	private void Validations()
	{
		EnumUtils.AssertNoDuplicateValuesInEnum<AttackEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<AttackEffectEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<TileEffectEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<TileUpgradeEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<SkillEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<PickupEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<UnlockID>();
		EnumUtils.AssertNoDuplicateValuesInEnum<EnemyEnum>();
		EnumUtils.AssertNoDuplicateValuesInEnum<HeroEnum>();
	}

	private void InitializeAnalytics()
	{
		UnityServices.InitializeAsync();
	}

	private void PlatformSpecificInitialization()
	{
		if (Globals.CurrentPlatform == Globals.Platform.steamdeck)
		{
			SteamDeckInitialization();
		}
	}

	private void SteamDeckInitialization()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Resolution currentResolution = Screen.currentResolution;
		if (((Resolution)(ref currentResolution)).width == 1280)
		{
			currentResolution = Screen.currentResolution;
			if (((Resolution)(ref currentResolution)).height == 800)
			{
				Globals.Options.resolution = Options.Resolution._1280x800;
				Screen.SetResolution(1280, 800, (FullScreenMode)1);
				Globals.Options.controlScheme = Options.ControlScheme.Gamepad;
			}
		}
	}
}
