using System.Collections;
using TMPro;
using UnityEngine;

public class PostRoomFeedback : MonoBehaviour
{
	public GameObject canvasGO;

	public GameObject difficultyFeedbackGO;

	public TMP_InputField imputField;

	private string difficultyFeedback;

	private bool waiting;

	private void Start()
	{
		canvasGO.SetActive(false);
	}

	public void Enable()
	{
		canvasGO.SetActive(true);
		difficultyFeedbackGO.SetActive(true);
		difficultyFeedback = "";
		imputField.text = "";
		waiting = true;
	}

	public void Continue()
	{
		canvasGO.SetActive(false);
		waiting = false;
	}

	public IEnumerator WaitForFeedback()
	{
		Enable();
		while (waiting)
		{
			yield return null;
		}
	}

	public void DifficultyFeedback(string value)
	{
		difficultyFeedback = value;
		difficultyFeedbackGO.SetActive(false);
	}
}
