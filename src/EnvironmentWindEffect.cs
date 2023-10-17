using System.Collections;
using UnityEngine;

public class EnvironmentWindEffect : MonoBehaviour
{
	public Vector2 smallSwirlC;

	public Vector2 smallSwirlE;

	public Vector2 dustC;

	public Vector2 dustE;

	public float pSmallSwirl;

	public float pSmallDust;

	public float pLargeDust;

	private void Start()
	{
		if ((Object)(object)EffectsManager.Instance != (Object)null)
		{
			((MonoBehaviour)this).StartCoroutine(SpawnWindEffect());
		}
	}

	private IEnumerator SpawnWindEffect()
	{
		while (true)
		{
			yield return (object)new WaitForSeconds(0.2f);
			if (Random.Range(0f, 1f) < pSmallSwirl)
			{
				EffectsManager.Instance.CreateInGameEffect("SmallSwirl", ((Component)this).transform, RandomPosition(smallSwirlC, Vector2.op_Implicit(smallSwirlE)));
			}
			if (Random.Range(0f, 1f) < pSmallDust)
			{
				EffectsManager.Instance.CreateInGameEffect("SmallDust", ((Component)this).transform, RandomPosition(dustC, Vector2.op_Implicit(dustE)));
			}
			if (Random.Range(0f, 1f) < pLargeDust)
			{
				EffectsManager.Instance.CreateInGameEffect("LargeDust", ((Component)this).transform, RandomPosition(dustC, Vector2.op_Implicit(dustE)));
			}
		}
	}

	private Vector3 RandomPosition(Vector2 center, Vector3 extent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		float num = center.x + Random.Range(0f - extent.x, extent.x);
		float num2 = center.y + Random.Range(0f - extent.y, extent.y);
		return new Vector3(num, num2, 0f);
	}
}
