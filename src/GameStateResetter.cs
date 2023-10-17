using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateResetter : MonoBehaviour
{
	private void Start()
	{
		Time.timeScale = 1f;
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		foreach (GameObject val in array)
		{
			if (!(((Object)val).name == "SteamManager"))
			{
				Object.Destroy((Object)(object)val);
			}
		}
		SceneManager.LoadScene("GameInitialization");
	}
}
