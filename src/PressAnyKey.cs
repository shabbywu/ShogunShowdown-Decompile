using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PressAnyKey : MonoBehaviour
{
	public PlayerInput playerInput;

	public TextMeshProUGUI anyKeyText;

	private void Start()
	{
		playerInput.actions["Menu/AnyKey"].performed += AnyKeyboardKeyPressed;
	}

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (Gamepad.current != null && ((IEnumerable<InputControl>)(object)((InputDevice)Gamepad.current).allControls).Any((InputControl x) => x is ButtonControl && InputControlExtensions.IsPressed(x, 0f) && !x.synthetic))
		{
			AnyGamepadButtonPressed();
		}
	}

	private void AnyKeyboardKeyPressed(CallbackContext context)
	{
		AnyKeyWasPressed(Options.ControlScheme.MouseAndKeyboard);
	}

	private void AnyGamepadButtonPressed()
	{
		AnyKeyWasPressed(Options.ControlScheme.Gamepad);
	}

	private void AnyKeyWasPressed(Options.ControlScheme controlsSchemeUsed)
	{
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		TitleScreenManager.Instance.AnyKeyWasPressed(controlsSchemeUsed);
		playerInput.actions["Menu/AnyKey"].performed -= AnyKeyboardKeyPressed;
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
