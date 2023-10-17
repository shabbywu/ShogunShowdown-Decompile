using System.Collections;
using UnityEngine;

public class FastBulletEffect : MonoBehaviour
{
	[SerializeField]
	private TrailRenderer trailRenderer;

	public void Initialize(Vector3 from, Vector3 to, float speed = 50f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StartCoroutine(Move(from, to, speed));
	}

	private IEnumerator Move(Vector3 from, Vector3 to, float speed)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float moveTime = Vector3.Distance(from, to) / speed;
		float t = 0f;
		while (t < moveTime)
		{
			t += Time.deltaTime;
			((Component)this).transform.position = Vector3.Lerp(from, to, t / moveTime);
			yield return null;
		}
		while (trailRenderer.emitting)
		{
			yield return null;
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
