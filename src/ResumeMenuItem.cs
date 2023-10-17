public class ResumeMenuItem : OptionsMenuItem
{
	public PauseMenu pauseMenu;

	public override void OnSubmit()
	{
		pauseMenu.ResumeGame();
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
