using System.Collections.Generic;
using TMPro;
using Utils;

public class ControlsMenuItem : OptionsMenuItem
{
	private List<Options.ControlSchemePreference> options = new List<Options.ControlSchemePreference>
	{
		Options.ControlSchemePreference.AutoDetect,
		Options.ControlSchemePreference.MouseAndKeyboard,
		Options.ControlSchemePreference.Gamepad
	};

	public override void OnSubmit()
	{
		SelectNext(1);
	}

	public override void OnLeft()
	{
		SelectNext(-1);
	}

	public override void OnRight()
	{
		SelectNext(1);
	}

	public override void UpdateState()
	{
		switch (Globals.Options.controlSchemePreference)
		{
		case Options.ControlSchemePreference.AutoDetect:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scheme_AutoDetect");
			break;
		case Options.ControlSchemePreference.MouseAndKeyboard:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scheme_MouseAndKeyboard");
			break;
		case Options.ControlSchemePreference.Gamepad:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scheme_Controller");
			break;
		}
	}

	private void SelectNext(int delta)
	{
		int num = options.IndexOf(Globals.Options.controlSchemePreference);
		num = MyMath.ModularizeIndex(num + delta, options.Count);
		Globals.Options.controlSchemePreference = options[num];
		if (Globals.Options.controlSchemePreference == Options.ControlSchemePreference.Gamepad)
		{
			Globals.Options.controlScheme = Options.ControlScheme.Gamepad;
			EventsManager.Instance.ControlsSchemeUpdated.Invoke(Globals.Options.controlScheme);
		}
		else if (Globals.Options.controlSchemePreference == Options.ControlSchemePreference.MouseAndKeyboard)
		{
			Globals.Options.controlScheme = Options.ControlScheme.MouseAndKeyboard;
			EventsManager.Instance.ControlsSchemeUpdated.Invoke(Globals.Options.controlScheme);
		}
		InteractionEffect();
		UpdateState();
	}
}
