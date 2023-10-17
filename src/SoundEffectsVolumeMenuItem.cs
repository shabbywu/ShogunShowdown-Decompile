public class SoundEffectsVolumeMenuItem : Base_SliderMenuItem
{
	protected override void OnSliderValueChange()
	{
		Globals.Options.soundEffectsVolume = (int)base.Slider.value;
		SoundEffectsManager.Instance.SetVolume(Globals.Options.soundEffectsVolume);
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
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
			base.Slider.value = options.soundEffectsVolume;
		}
		OnSliderValueChange();
		InteractionEffect();
		UpdateState();
	}

	public override void UpdateState()
	{
		base.Slider.value = Globals.Options.soundEffectsVolume;
	}
}
