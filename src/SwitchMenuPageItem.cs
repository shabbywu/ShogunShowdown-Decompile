using UnityEngine;

public class SwitchMenuPageItem : OptionsMenuItem
{
	[SerializeField]
	private MenuPage from;

	[SerializeField]
	private MenuPage to;

	[SerializeField]
	private bool isBackMenuItem;

	public override void OnSubmit()
	{
		InteractionEffect();
		from.Disable();
		to.Enable(isBackMenuItem);
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
