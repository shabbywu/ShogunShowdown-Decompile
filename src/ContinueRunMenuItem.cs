using UnityEngine;
using Utils;

public class ContinueRunMenuItem : OptionsMenuItem
{
	private string infoBoxDescription;

	public override string InfoBoxText => infoBoxDescription;

	public override void OnSubmit()
	{
		if ((Object)(object)TitleScreenManager.Instance == (Object)null)
		{
			Debug.LogError((object)"ContinueRunMenuItem not in title screen!");
		}
		TitleScreenManager.Instance.ContinueRun();
		InteractionEffect();
		UpdateState();
	}

	public override void UpdateState()
	{
		if ((Object)(object)SaveDataManager.Instance == (Object)null || !SaveDataManager.Instance.runSaveData.hasRunInProgress)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		infoBoxDescription = SaveDataManager.Instance.runSaveData.Description;
		infoBoxDescription = string.Format(infoBoxDescription, LocalizationUtils.LocalizedString("Menu", "ContinueInfo_RunTime"), LocalizationUtils.LocalizedString("Menu", "ContinueInfo_Hero"), LocalizationUtils.LocalizedString("Menu", "ContinueInfo_Location"));
	}

	public override void OnLeft()
	{
	}

	public override void OnRight()
	{
	}
}
