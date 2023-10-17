using UnityEngine;

public class OpenOptionsMenuItem : OptionsMenuItem
{
	[SerializeField]
	private MenuPage pageToDisableOnSubmit;

	public override void OnSubmit()
	{
		InteractionEffect();
		EventsManager.Instance.OpenOptionsMenu.Invoke();
		if ((Object)(object)pageToDisableOnSubmit != (Object)null)
		{
			pageToDisableOnSubmit.Disable();
		}
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
