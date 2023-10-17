using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsSceneManager : MonoBehaviour
{
	[SerializeField]
	private PlayerInput playerInput;

	private void Awake()
	{
		if (!Globals.GameInitialized)
		{
			SceneManager.LoadScene("GameInitialization");
		}
	}

	private void Start()
	{
		playerInput.SwitchCurrentActionMap("Menu");
	}

	public void BackToTitleScreen()
	{
		Globals.SkipTitleScreen = false;
		SceneLoader.Instance.LoadScene("ResetGameState");
	}

	public void BackToTitleScreen(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed)
		{
			BackToTitleScreen();
		}
	}
}
