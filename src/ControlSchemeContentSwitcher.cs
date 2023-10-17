using UnityEngine;

public class ControlSchemeContentSwitcher : MonoBehaviour
{
	[SerializeField]
	private GameObject[] keyboardAndMouseContent;

	[SerializeField]
	private GameObject[] gamepadContent;

	private void Awake()
	{
		bool active = Globals.Options.controlScheme == Options.ControlScheme.MouseAndKeyboard;
		bool active2 = Globals.Options.controlScheme == Options.ControlScheme.Gamepad;
		GameObject[] array = keyboardAndMouseContent;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(active);
		}
		array = gamepadContent;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(active2);
		}
	}
}
