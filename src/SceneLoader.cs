using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public Animator sceneTransition;

	[SerializeField]
	private GameObject[] objectsToDisableUponLoadScene;

	public static SceneLoader Instance { get; protected set; }

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
		if (!Globals.Quick)
		{
			sceneTransition.SetTrigger("In");
		}
	}

	public void LoadScene(string sceneName, bool quick = false)
	{
		GameObject[] array = objectsToDisableUponLoadScene;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		if (quick)
		{
			SceneManager.LoadScene(sceneName);
		}
		((MonoBehaviour)this).StartCoroutine(LoadSceneCoroutine(sceneName));
	}

	private IEnumerator LoadSceneCoroutine(string sceneName)
	{
		if (!Globals.Quick)
		{
			sceneTransition.SetTrigger("Out");
			yield return null;
			while (true)
			{
				AnimatorStateInfo currentAnimatorStateInfo = sceneTransition.GetCurrentAnimatorStateInfo(0);
				if (!(((AnimatorStateInfo)(ref currentAnimatorStateInfo)).normalizedTime < 1f))
				{
					break;
				}
				yield return null;
			}
		}
		SceneManager.LoadScene(sceneName);
	}
}
