public class MusicVolumeMenuItem : Base_SliderMenuItem
{
	protected override void OnSliderValueChange()
	{
		Globals.Options.musicVolume = (int)base.Slider.value;
		MusicManager.Instance.SetVolume(Globals.Options.musicVolume);
	}

	public override void OnSubmit()
	{
		if (base.Slider.value != 0f)
		{
			base.Slider.value = 0f;
		}
		else
		{
			Options options = new Options();
			base.Slider.value = options.musicVolume;
		}
		OnSliderValueChange();
		InteractionEffect();
		UpdateState();
	}

	public override void UpdateState()
	{
		base.Slider.value = Globals.Options.musicVolume;
	}
}
