using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
	public float baseIntensity;

	public float flickerIntensity;

	public float slowFlickerTime;

	public float fastFlickerTime;

	private Light2D light2d;

	private void Start()
	{
		light2d = ((Component)this).GetComponent<Light2D>();
	}

	private void Update()
	{
		light2d.intensity = baseIntensity + 0.5f * flickerIntensity * (Mathf.PingPong(Time.time, slowFlickerTime) + Mathf.PingPong(Time.time, fastFlickerTime));
	}
}
