public class MainMenuMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		InteractionEffect();
		Globals.SkipTitleScreen = false;
		SceneLoader.Instance.LoadScene("ResetGameState");
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
