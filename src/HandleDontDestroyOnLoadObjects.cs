using System.Collections.Generic;
using UnityEngine;

public class HandleDontDestroyOnLoadObjects : MonoBehaviour
{
	public bool disableInScene;

	private List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

	public void Start()
	{
		if (!disableInScene)
		{
			return;
		}
		DontDestroyOnLoad[] array = Object.FindObjectsOfType<DontDestroyOnLoad>();
		foreach (DontDestroyOnLoad dontDestroyOnLoad in array)
		{
			dontDestroyOnLoadObjects.Add(((Component)dontDestroyOnLoad).gameObject);
		}
		foreach (GameObject dontDestroyOnLoadObject in dontDestroyOnLoadObjects)
		{
			dontDestroyOnLoadObject.SetActive(false);
		}
	}

	public void SwitchingScene()
	{
		if (!disableInScene)
		{
			return;
		}
		foreach (GameObject dontDestroyOnLoadObject in dontDestroyOnLoadObjects)
		{
			dontDestroyOnLoadObject.SetActive(true);
		}
	}
}
