public class GiveUpMenuItem : OptionsMenuItem
{
	public PauseMenu pauseMenu;

	public override void OnSubmit()
	{
		pauseMenu.GiveUp();
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
