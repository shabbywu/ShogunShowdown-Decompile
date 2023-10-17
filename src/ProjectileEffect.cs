using System.Collections;
using Parameters;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
	[SerializeField]
	private Transform projectile;

	[SerializeField]
	private Transform projectileSprite;

	[SerializeField]
	private SpriteRenderer throwEffectSpriteRenderer;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float rotationSpeed;

	public bool MovingTowardsTargetPosition { get; private set; }

	public void Throw(Vector3 initialPosition, Vector3 targetPosition, int randomDeltaYMagnitudeInPixels = 0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StartCoroutine(ThrowCoroutine(initialPosition, targetPosition, randomDeltaYMagnitudeInPixels));
	}

	private IEnumerator ThrowCoroutine(Vector3 initialPosition, Vector3 targetPosition, int randomDeltaYMagnitudeInPixels)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		MovingTowardsTargetPosition = true;
		if (targetPosition.x < initialPosition.x)
		{
			throwEffectSpriteRenderer.flipX = true;
		}
		Vector3 initial = initialPosition;
		Vector3 target = targetPosition;
		if (randomDeltaYMagnitudeInPixels != 0)
		{
			Vector3 val = Vector3.up * TechParams.pixelSize * (float)Random.Range(-randomDeltaYMagnitudeInPixels, randomDeltaYMagnitudeInPixels + 1);
			initial += val;
			target += val;
		}
		((Component)this).transform.position = initial;
		float flighTime = Vector3.Distance(initialPosition, targetPosition) / speed;
		float currentTime = 0f;
		SoundEffectsManager.Instance.Play("ShurikenThrow");
		while (currentTime < flighTime)
		{
			projectile.position = Vector3.Lerp(initial, target, currentTime / flighTime);
			projectileSprite.Rotate(0f, 0f, (0f - rotationSpeed) * Time.deltaTime);
			currentTime += Time.deltaTime;
			yield return null;
		}
		MovingTowardsTargetPosition = false;
		((Component)projectileSprite).gameObject.SetActive(false);
		Object.Destroy((Object)(object)((Component)this).gameObject, 1f);
	}
}
