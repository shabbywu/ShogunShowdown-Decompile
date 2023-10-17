using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameScreenLogging : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI tmproText;

	private void Start()
	{
		EventsManager.Instance.LogToGameScreen.AddListener((UnityAction<string>)LogToGameScreen);
	}

	private void LogToGameScreen(string message)
	{
		((TMP_Text)tmproText).text = "- " + message + "\n" + ((TMP_Text)tmproText).text;
	}
}
