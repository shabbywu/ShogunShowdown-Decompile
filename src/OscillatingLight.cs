using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OscillatingLight : MonoBehaviour
{
	[SerializeField]
	private Light2D light2d;

	[SerializeField]
	private float intensityFrom;

	[SerializeField]
	private float intensityTo;

	[SerializeField]
	private float periodSeconds;

	private void Start()
	{
		LeanTween.value(((Component)this).gameObject, intensityFrom, intensityTo, periodSeconds).setOnUpdate((Action<float>)delegate(float val)
		{
			Intensity(val);
		}).setEase((LeanTweenType)4)
			.setLoopPingPong();
	}

	private void Intensity(float value)
	{
		light2d.intensity = value;
	}
}
