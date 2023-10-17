using UnityEngine;

public class ClearSaveDataMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		if ((Object)(object)TitleScreenManager.Instance == (Object)null)
		{
			Debug.LogError((object)"ClearSaveDataMenuItem not in title screen!");
		}
		TitleScreenManager.Instance.EraseSavedData();
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
