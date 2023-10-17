using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupRepulsion : MonoBehaviour
{
	private Rigidbody2D rb;

	private Vector2 repulsiveForce = 3f * Vector2.left;

	private void Awake()
	{
		rb = ((Component)this).GetComponentInParent<Rigidbody2D>();
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)collider.attachedRigidbody == (Object)null) && !((Object)(object)collider.attachedRigidbody == (Object)(object)rb) && !(((Component)collider).gameObject.tag != "PickUp"))
		{
			if (((Component)collider.attachedRigidbody).transform.position.x > ((Component)rb).transform.position.x)
			{
				rb.AddForce(repulsiveForce);
			}
			else
			{
				rb.AddForce(-repulsiveForce);
			}
		}
	}
}
