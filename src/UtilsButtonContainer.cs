using UnityEngine;
using UnityEngine.Events;

public class UtilsButtonContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject[] mapRelatedObjects;

	private void Start()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (Globals.Tutorial)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(BeginRun));
		GameObject[] array = mapRelatedObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	private void BeginRun()
	{
		GameObject[] array = mapRelatedObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
	}
}
