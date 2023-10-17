using UnityEngine;

public class CreditsMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		if ((Object)(object)TitleScreenManager.Instance == (Object)null)
		{
			Debug.LogError((object)"CreditsMenuItem not in title screen!");
		}
		TitleScreenManager.Instance.GoToCreditsScene();
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
