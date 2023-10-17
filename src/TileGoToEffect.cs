using UnityEngine;

public class TileGoToEffect : MonoBehaviour
{
	public void Initialize(Vector3 from, Vector3 to)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.rotation = Quaternion.FromToRotation(Vector3.up, to - from);
	}
}
