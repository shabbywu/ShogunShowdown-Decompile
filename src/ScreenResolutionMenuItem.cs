using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils;

public class ScreenResolutionMenuItem : OptionsMenuItem
{
	private List<Options.Resolution> options = Enum.GetValues(typeof(Options.Resolution)).Cast<Options.Resolution>().ToList();

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
		((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Video_Resolution");
		TextMeshProUGUI obj = text;
		((TMP_Text)obj).text = ((TMP_Text)obj).text + " " + Globals.Options.resolution.ToString().Replace("_", "").Replace("x", " x ");
	}

	private void SelectNext(int delta)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		int num = options.IndexOf(Globals.Options.resolution);
		num = MyMath.ModularizeIndex(num + delta, options.Count);
		Globals.Options.resolution = options[num];
		FullScreenMode fullScreenMode = Screen.fullScreenMode;
		switch (Globals.Options.resolution)
		{
		case Options.Resolution._1920x1080:
			Screen.SetResolution(1920, 1080, fullScreenMode);
			break;
		case Options.Resolution._1440x810:
			Screen.SetResolution(1440, 810, fullScreenMode);
			break;
		case Options.Resolution._1280x800:
			Screen.SetResolution(1280, 800, fullScreenMode);
			break;
		case Options.Resolution._1280x720:
			Screen.SetResolution(1280, 720, fullScreenMode);
			break;
		}
		UpdateState();
	}
}
