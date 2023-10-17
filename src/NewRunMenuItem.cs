using UnityEngine;

public class NewRunMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		if ((Object)(object)TitleScreenManager.Instance == (Object)null)
		{
			Debug.LogError((object)"NewRunMenuItem not in title screen!");
		}
		Globals.ContinueRun = false;
		if (Globals.FirstEverRun)
		{
			TitleScreenManager.Instance.GoToTutorial();
		}
		else
		{
			TitleScreenManager.Instance.NewRun();
		}
		InteractionEffect();
	}

	public override void UpdateState()
	{
	}

	public override void OnLeft()
	{
	}

	public override void OnRight()
	{
	}
}
