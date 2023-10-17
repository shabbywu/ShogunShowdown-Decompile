using UnityEngine;

public class TileInteractionEffect : MonoBehaviour
{
	private Animator animator;

	private Vector3 prevTargetPosition;

	private bool closeFollow;

	private Transform Target { get; set; }

	private Rigidbody2D Rigidbody2D { get; set; }

	public void InitializeOutwards(Transform target)
	{
		Initialize(target);
		animator.SetTrigger("Outwards");
		closeFollow = false;
	}

	public void InitializeInwards(Transform target)
	{
		Initialize(target);
		animator.SetTrigger("Inwards");
		closeFollow = true;
	}

	private void Initialize(Transform target)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		animator = ((Component)this).GetComponent<Animator>();
		Rigidbody2D = ((Component)this).GetComponent<Rigidbody2D>();
		Target = target;
		prevTargetPosition = Target.position;
	}

	private void Update()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Target != (Object)null)
		{
			if (closeFollow)
			{
				((Component)this).transform.position = Target.position;
			}
			else
			{
				Vector3 val = (Target.position - prevTargetPosition) / Time.deltaTime;
				float num = Vector3.SqrMagnitude(Target.position - ((Component)this).transform.position);
				Rigidbody2D.AddForce(Vector2.op_Implicit(val / Mathf.Clamp(num, 1f, 10f)));
			}
			prevTargetPosition = Target.position;
		}
	}
}
