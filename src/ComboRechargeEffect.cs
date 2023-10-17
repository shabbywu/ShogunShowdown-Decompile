using UnityEngine;

public class ComboRechargeEffect : MonoBehaviour
{
	private float initialVelocity = 5f;

	private float smoothTime = 0.1f;

	public SpriteRenderer spriteRenderer;

	private float tExpand = 0.2f;

	private Vector3 currentVelocity;

	private Transform targetTransform;

	public bool TargetReached { get; private set; }

	public void Initialize(Transform target)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		targetTransform = target;
		currentVelocity = initialVelocity * Vector3.Normalize(Vector2.op_Implicit(Random.insideUnitCircle));
	}

	private void Update()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (tExpand > 0f)
		{
			((Component)this).transform.Translate(currentVelocity * Time.deltaTime);
			tExpand -= Time.deltaTime;
			return;
		}
		((Component)this).transform.position = Vector3.SmoothDamp(((Component)this).transform.position, targetTransform.position, ref currentVelocity, smoothTime);
		if (!TargetReached && Vector3.SqrMagnitude(((Component)this).transform.position - targetTransform.position) < 0.1f)
		{
			TargetReached = true;
			((Renderer)spriteRenderer).enabled = false;
			Object.Destroy((Object)(object)((Component)this).gameObject, 0.2f);
		}
	}
}
