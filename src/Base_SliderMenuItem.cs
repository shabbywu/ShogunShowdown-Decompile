using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Base_SliderMenuItem : OptionsMenuItem
{
	protected Slider Slider { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		Slider = ((Component)this).GetComponentInChildren<Slider>();
		((UnityEvent<float>)(object)Slider.onValueChanged).AddListener((UnityAction<float>)delegate
		{
			OnSliderValueChange();
		});
	}

	protected abstract void OnSliderValueChange();

	public override void OnLeft()
	{
		if (Slider.value > Slider.minValue)
		{
			Slider slider = Slider;
			float value = slider.value;
			slider.value = value - 1f;
			OnSliderValueChange();
		}
	}

	public override void OnRight()
	{
		if (Slider.value < Slider.maxValue)
		{
			Slider slider = Slider;
			float value = slider.value;
			slider.value = value + 1f;
			OnSliderValueChange();
		}
	}
}
