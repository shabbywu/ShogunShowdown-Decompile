using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
	public TextMeshProUGUI versionText;

	[SerializeField]
	private MenuPage titleScreenMenuPage;

	[SerializeField]
	private CanvasGroup titleScreenMenuCanvasGroup;

	[SerializeField]
	private TitleScreenSceneInput titleScreenSceneInput;

	public static TitleScreenManager Instance { get; protected set; }

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
		}
	}

	private void Start()
	{
		MusicManager.Instance.Play("Title");
		((TMP_Text)versionText).text = "early access version " + Application.version;
	}

	public void AnyKeyWasPressed(Options.ControlScheme controlsSchemeUsed)
	{
		MenuUINavigationManager.Instance.CurrentMenuControlScheme = controlsSchemeUsed;
		EventsManager.Instance.ControlsSchemeUpdated.Invoke(controlsSchemeUsed);
		titleScreenMenuPage.Enable();
	}

	public void GoToTutorial()
	{
		Globals.ContinueRun = false;
		Globals.Tutorial = true;
		GoToCombatScene();
	}

	public void ContinueRun()
	{
		Globals.ContinueRun = true;
		Globals.Tutorial = false;
		GoToCombatScene();
	}

	public void NewRun()
	{
		Globals.ContinueRun = false;
		Globals.Tutorial = false;
		GoToCombatScene();
	}

	private void GoToCombatScene()
	{
		Globals.SkipTitleScreen = true;
		if (Globals.Options.controlSchemePreference == Options.ControlSchemePreference.AutoDetect)
		{
			Globals.Options.controlScheme = MenuUINavigationManager.Instance.CurrentMenuControlScheme;
		}
		EventsManager.Instance.ControlsSchemeUpdated.Invoke(Globals.Options.controlScheme);
		((MonoBehaviour)this).StartCoroutine(GoToCombatTransition());
	}

	public void GoToCreditsScene()
	{
		((Component)titleScreenSceneInput).gameObject.SetActive(false);
		SceneLoader.Instance.LoadScene("Credits");
	}

	private IEnumerator GoToCombatTransition()
	{
		if (!Globals.Quick)
		{
			((Component)titleScreenSceneInput).gameObject.SetActive(false);
			SoundEffectsManager.Instance.Play("EndFight");
			yield return (object)new WaitForSeconds(0.3f);
			LeanTween.alphaCanvas(titleScreenMenuCanvasGroup, 0f, 0.2f);
			float num = 0.75f;
			LeanTween.move(((Component)Camera.main).gameObject, 8.5f * Vector3.down, 0.75f).setEase((LeanTweenType)3);
			yield return (object)new WaitForSeconds(num);
		}
		SceneManager.LoadScene("Combat");
	}

	public void QuitGame()
	{
		Globals.QuitGame();
	}

	public void EraseSavedData()
	{
		SaveDataManager.Instance.DeleteStoredSaveData();
		SaveDataManager.Instance.DeleteStoredRunSaveData();
		SaveDataManager.Instance.DeleteStoredOptions();
		SceneManager.LoadScene("ResetGameState");
	}
}
