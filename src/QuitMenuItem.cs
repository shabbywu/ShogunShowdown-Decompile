public class QuitMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		InteractionEffect();
		Globals.QuitGame();
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
