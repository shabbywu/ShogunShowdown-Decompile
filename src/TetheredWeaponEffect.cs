using System;
using System.Collections;
using UnityEngine;

public class TetheredWeaponEffect : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer rope;

	[SerializeField]
	private SpriteRenderer head;

	public IEnumerator PerformWeaponMove(float initialLength, float finalLenght, float speed, float accellerationTime = 0f)
	{
		SetLenght(initialLength);
		float length = initialLength;
		bool useAccelleration = accellerationTime > 0f;
		float currentSpeed = (useAccelleration ? (speed / 2f) : speed);
		while (true)
		{
			if (useAccelleration)
			{
				currentSpeed = Mathf.MoveTowards(currentSpeed, speed, Time.deltaTime * speed / accellerationTime);
			}
			length = Mathf.MoveTowards(length, finalLenght, currentSpeed * Time.deltaTime);
			SetLenght(length);
			if ((double)Mathf.Abs(length - finalLenght) < 0.0001)
			{
				break;
			}
			yield return null;
		}
		SetLenght(finalLenght);
	}

	private void SetLenght(float length)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		((Component)head).transform.localPosition = new Vector3(length, 0f, 0f);
		rope.size = new Vector2(length, rope.size.y);
	}

	public void DisappearAndDestroy()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		rope.size = Vector2.zero;
		LeanTween.scale(((Component)head).gameObject, Vector3.zero, 0.2f).setEase((LeanTweenType)6).setOnComplete((Action)delegate
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		});
	}
}
