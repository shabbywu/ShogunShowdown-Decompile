using InfoBoxUtils;
using PickupEnums;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Pickup : MonoBehaviour, IInfoBoxable
{
	private Rigidbody2D rb;

	private float minX;

	private float maxX;

	private bool playerOverlapping;

	private Vector2 v0;

	private bool physicsEnabled = true;

	public abstract PickupEnum PickupEnum { get; }

	protected abstract bool CanPickUp { get; }

	public Cell Cell { get; private set; }

	public bool PhysicsEnabled
	{
		get
		{
			return physicsEnabled;
		}
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (physicsEnabled)
			{
				v0 = rb.velocity;
			}
			rb.isKinematic = !value;
			rb.velocity = (value ? v0 : Vector2.zero);
			physicsEnabled = value;
		}
	}

	public abstract string InfoBoxText { get; }

	public bool InfoBoxEnabled => Globals.FullInfoMode;

	public BoxWidth BoxWidth => BoxWidth.auto;

	protected abstract void PickUpEffect();

	public void Initialize(Cell cell, float mininumX, float maximumX, Vector2 initialVelocity)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Cell = cell;
		minX = mininumX;
		maxX = maximumX;
		v0 = initialVelocity;
		rb = ((Component)this).GetComponent<Rigidbody2D>();
		rb.velocity = v0;
	}

	private void PickUp()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		EventsManager.Instance.PickupPickedUp.Invoke(this);
		if (CanPickUp)
		{
			PickUpEffect();
		}
		EffectsManager.Instance.CreateInGameEffect("PickUpEffect", ((Component)this).transform.position);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private void Update()
	{
		if (playerOverlapping && CanPickUp)
		{
			PickUp();
			SoundEffectsManager.Instance.Play("PickupPickUp");
		}
	}

	private void LateUpdate()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (PhysicsEnabled && (((Component)this).transform.position.x < minX || ((Component)this).transform.position.x > maxX))
		{
			((Component)this).transform.position = new Vector3(Mathf.Clamp(((Component)this).transform.position.x, minX, maxX), ((Component)this).transform.position.y, ((Component)this).transform.position.z);
			rb.velocity = Vector2.op_Implicit(Vector3.Scale(Vector2.op_Implicit(rb.velocity), new Vector3(0f, 1f, 1f)));
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (((Component)other).gameObject.CompareTag("Player"))
		{
			playerOverlapping = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (((Component)other).gameObject.CompareTag("Player"))
		{
			playerOverlapping = false;
		}
	}

	public virtual void ForcePickUp()
	{
		PickUp();
	}
}
