using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	[SerializeField]
	private Transform[] cameraTransforms;

	[SerializeField]
	private float shakeDuration;

	[SerializeField]
	private float shakeAmount;

	[SerializeField]
	private float deltaTimeBetweenShakes;

	private float timeSinceBeginningOfShake;

	private float duration;

	private float amount;

	private float timeSinceLastShake;

	public void Enable(float durationMultiplier = 1f, float amountMultiplier = 1f)
	{
		if (!((Component)this).gameObject.activeSelf)
		{
			((Component)this).gameObject.SetActive(true);
			duration = shakeDuration * durationMultiplier;
			amount = shakeAmount * amountMultiplier;
			timeSinceBeginningOfShake = 0f;
			timeSinceLastShake = 0f;
		}
	}

	private void Update()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (timeSinceBeginningOfShake < duration)
		{
			timeSinceBeginningOfShake += Time.deltaTime;
			timeSinceLastShake += Time.deltaTime;
			if (!(timeSinceLastShake < deltaTimeBetweenShakes))
			{
				timeSinceLastShake = 0f;
				Vector3 localPosition = Random.insideUnitSphere * amount;
				Transform[] array = cameraTransforms;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].localPosition = localPosition;
				}
			}
		}
		else
		{
			Transform[] array = cameraTransforms;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].localPosition = Vector3.zero;
			}
			((Component)this).gameObject.SetActive(false);
		}
	}
}
