using UnityEngine;
using UnityEngine.Events;

public class SwitchToGiveUpPageMenuItem : SwitchMenuPageItem
{
	private void OnEnable()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (!CombatSceneManager.Instance.RunInProgress)
		{
			EventsManager.Instance.BeginRun.AddListener(new UnityAction(UponBeginRun));
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void UponBeginRun()
	{
		((Component)this).gameObject.SetActive(true);
	}
}
