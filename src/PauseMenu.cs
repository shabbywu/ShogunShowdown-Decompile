using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnlocksID;

public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private MenuPage[] menuPages;

	[SerializeField]
	private MenuPage initialMenuPage;

	[SerializeField]
	private CanvasGroup darkBackground;

	[SerializeField]
	private AudioMixerSnapshot pausedAudioMixerSnapshot;

	[SerializeField]
	private AudioMixerSnapshot unpausedAudioMixerSnapshot;

	public GameObject menuContainer;

	private bool gamePaused;

	public TextMeshProUGUI currentRoomName;

	public HeroStampsDisplay heroStampsDisplay;

	private void Start()
	{
		if (UnlocksManager.Instance.Unlocked(UnlockID.q_shogun_defeated))
		{
			((Component)heroStampsDisplay).gameObject.SetActive(true);
			heroStampsDisplay.Initialize(Globals.Hero);
		}
		else
		{
			((Component)heroStampsDisplay).gameObject.SetActive(false);
		}
		DisableAllMenuPages();
	}

	public void TogglePause()
	{
		if (gamePaused)
		{
			ResumeGame();
		}
		else
		{
			PauseGame();
		}
	}

	private void PauseGame()
	{
		if (((Component)this).gameObject.activeSelf)
		{
			((TMP_Text)currentRoomName).text = CombatSceneManager.Instance.Room.Name.Replace('\n', ' ');
			MenuUINavigationManager.Instance.CurrentMenuControlScheme = Globals.Options.controlScheme;
			gamePaused = true;
			menuContainer.SetActive(true);
			darkBackground.alpha = 0f;
			((Component)darkBackground).gameObject.SetActive(true);
			LeanTween.alphaCanvas(darkBackground, 0.9f, 0.1f).setIgnoreTimeScale(true);
			pausedAudioMixerSnapshot.TransitionTo(0f);
			initialMenuPage.Enable();
			heroStampsDisplay.Initialize(Globals.Hero);
			InputManager.Instance.SwitchToMenuMode();
			Time.timeScale = 0f;
		}
	}

	public void ResumeGame()
	{
		EventsManager.Instance.CloseOptionsMenu.Invoke();
		gamePaused = false;
		menuContainer.SetActive(false);
		InputManager.Instance.SwitchToCombatMode();
		Time.timeScale = 1f;
		DisableAllMenuPages();
		LeanTween.alphaCanvas(darkBackground, 0f, 0.15f).setOnComplete((Action)DisableDarkBackground).setIgnoreTimeScale(true);
		unpausedAudioMixerSnapshot.TransitionTo(0.1f);
	}

	public void GiveUp()
	{
		if (Globals.Tutorial)
		{
			SceneManager.LoadScene("ResetGameState");
		}
		ResumeGame();
		if (CombatSceneManager.Instance.IsCoreGameplay)
		{
			Globals.Hero.CommitSeppuku();
			return;
		}
		MusicManager.Instance.SmoothSetVolume(Globals.Options.musicVolume, 0, 1f);
		SceneLoader.Instance.LoadScene("ResetGameState");
	}

	public void QuitGame()
	{
		Globals.QuitGame();
	}

	private void DisableDarkBackground()
	{
		if (!gamePaused)
		{
			((Component)darkBackground).gameObject.SetActive(false);
		}
	}

	private void DisableAllMenuPages()
	{
		MenuPage[] array = menuPages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disable();
		}
	}
}
