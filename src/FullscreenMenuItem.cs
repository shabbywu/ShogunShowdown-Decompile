using TMPro;
using UnityEngine;
using Utils;

public class FullscreenMenuItem : OptionsMenuItem
{
	public int fullscreenValue
	{
		get
		{
			if (Screen.fullScreen)
			{
				return 1;
			}
			return 0;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (Globals.CurrentPlatform == Globals.Platform.steamdeck)
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	public override void OnSubmit()
	{
		InteractionEffect();
		UpdateState();
		ToggleFullscreen();
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
		UpdateText(Screen.fullScreen);
	}

	private void UpdateText(bool fullScreen)
	{
		if (fullScreen)
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_FullScreen_Yes");
		}
		else
		{
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_FullScreen_No");
		}
	}

	private void ToggleFullscreen()
	{
		bool fullScreen = Screen.fullScreen;
		if (fullScreen)
		{
			Screen.fullScreenMode = (FullScreenMode)3;
		}
		else
		{
			Screen.fullScreenMode = (FullScreenMode)1;
		}
		UpdateText(!fullScreen);
	}
}
