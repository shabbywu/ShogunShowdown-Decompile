using TMPro;
using UnityEngine;
using Utils;

public class ColorblindModeMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		Globals.Options.colorblindMode = !Globals.Options.colorblindMode;
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.ColorblindModeUpdated.Invoke();
		}
		InteractionEffect();
		UpdateState();
	}

	public override void UpdateState()
	{
		if (Globals.Options.colorblindMode)
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_ColorblindMode_Yes");
		}
		else
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_ColorblindMode_No");
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
}
