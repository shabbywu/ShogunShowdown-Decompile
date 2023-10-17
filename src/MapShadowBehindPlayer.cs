using UnityEngine;

public class MapShadowBehindPlayer : MonoBehaviour
{
	public Transform mapPlayerTransform;

	public float distance;

	public float followSmoothTime;

	private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = new Vector3(mapPlayerTransform.position.x - distance, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
	}

	private void Update()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(mapPlayerTransform.position.x - distance, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
		((Component)this).transform.position = Vector3.SmoothDamp(((Component)this).transform.position, val, ref velocity, followSmoothTime);
	}
}
