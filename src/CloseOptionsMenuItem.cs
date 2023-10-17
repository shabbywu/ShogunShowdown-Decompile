using UnityEngine;

public class CloseOptionsMenuItem : OptionsMenuItem
{
	[SerializeField]
	private MenuPage pageToEnableOnSubmit;

	public override void OnSubmit()
	{
		InteractionEffect();
		EventsManager.Instance.CloseOptionsMenu.Invoke();
		if ((Object)(object)pageToEnableOnSubmit != (Object)null)
		{
			pageToEnableOnSubmit.Enable();
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
