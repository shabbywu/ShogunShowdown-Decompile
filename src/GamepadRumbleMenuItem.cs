using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class GamepadRumbleMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		Globals.Options.gamepadRumble = !Globals.Options.gamepadRumble;
		InteractionEffect();
		UpdateState();
		if (Globals.Options.gamepadRumble && Gamepad.current != null)
		{
			((MonoBehaviour)this).StartCoroutine(QuickRumbleCoroutine());
		}
	}

	public override void OnLeft()
	{
		OnSubmit();
	}

	public override void OnRight()
	{
		OnSubmit();
	}

	public override void UpdateState()
	{
		if (Globals.Options.gamepadRumble)
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Rumble_Yes");
		}
		else
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Rumble_No");
		}
	}

	private IEnumerator QuickRumbleCoroutine()
	{
		Gamepad.current.SetMotorSpeeds(0.5f, 0.5f);
		yield return (object)new WaitForSecondsRealtime(0.1f);
		Gamepad.current.SetMotorSpeeds(0f, 0f);
	}
}
