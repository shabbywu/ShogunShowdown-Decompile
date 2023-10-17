using TMPro;
using Utils;

public class ScreenShakeMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		Globals.Options.screenShake = !Globals.Options.screenShake;
		InteractionEffect();
		UpdateState();
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
		if (Globals.Options.screenShake)
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_ScreenShake_Yes");
		}
		else
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_ScreenShake_No");
		}
	}
}
