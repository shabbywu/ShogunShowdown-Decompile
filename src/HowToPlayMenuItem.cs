using UnityEngine;

public class HowToPlayMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		if ((Object)(object)TitleScreenManager.Instance == (Object)null)
		{
			Debug.LogError((object)"HowToPlayMenuItem not in title screen!");
		}
		Globals.ContinueRun = false;
		TitleScreenManager.Instance.GoToTutorial();
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
