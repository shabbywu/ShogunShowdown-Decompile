using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenSceneInput : MonoBehaviour
{
	public void UnlockAll(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && Globals.AllowFullUnlockInTitleScreen)
		{
			Debug.Log((object)"Unlock all tiles");
			UnlocksManager.Instance.UnlockEverything();
			SaveDataManager.Instance.StoreSaveData();
			SceneManager.LoadScene("ResetGameState");
		}
	}
}
