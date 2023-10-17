using UnityEngine;

public class ShootingStar : MonoBehaviour
{
	private Vector3 direction = Vector3.Normalize(new Vector3(3f, -1f, 0f));

	private float speed = 3f;

	private float lifetime = 0.5f;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		LeanTween.move(((Component)this).gameObject, ((Component)this).gameObject.transform.position + direction * speed / lifetime, lifetime);
		Object.Destroy((Object)(object)((Component)this).gameObject, lifetime + 1f);
	}
}
