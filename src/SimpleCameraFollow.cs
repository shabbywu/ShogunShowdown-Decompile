using System.Collections;
using UnityEngine;
using Utils;

public class SimpleCameraFollow : MonoBehaviour
{
	public enum CameraMode
	{
		fixedPosition,
		followHero
	}

	private enum Mode
	{
		followHero,
		openingTransition,
		staticMode,
		screenScroll
	}

	private float smoothTime;

	private float followPrefactor;

	private float initialX;

	private float maxSpeed = 1000f;

	private Vector3 velocity = Vector3.zero;

	private float z = -10f;

	private readonly float targetReachedSqrThreshold = 0.001f;

	private Mode mode = Mode.staticMode;

	private Vector3 position;

	private Vector3 screenScrollTargetPosition;

	public bool TargetReached => Vector3.SqrMagnitude(TargetPosition() - position) < targetReachedSqrThreshold;

	private void FixedUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (mode != Mode.staticMode)
		{
			position = Vector3.SmoothDamp(position, TargetPosition(), ref velocity, smoothTime, maxSpeed);
			((Component)this).transform.position = PixelUtils.PixelPerfectClamp(position);
		}
	}

	public void EnableStaticMode()
	{
		mode = Mode.staticMode;
	}

	public void EnableSmoothDampFollow(float initialX, float followPrefactor = 0.35f, float smoothTime = 0.05f)
	{
		mode = Mode.followHero;
		this.smoothTime = smoothTime;
		this.followPrefactor = followPrefactor;
		this.initialX = initialX;
		maxSpeed = 1000f;
	}

	public IEnumerator EnableOpeningTransition()
	{
		mode = Mode.openingTransition;
		smoothTime = 0.35f;
		while (Vector3.SqrMagnitude(position - TargetPosition()) > 0.001f)
		{
			yield return null;
		}
	}

	public IEnumerator EnableScreenScroll(Vector3 from, Vector3 to)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		mode = Mode.screenScroll;
		smoothTime = 0.2f;
		maxSpeed = 40f;
		screenScrollTargetPosition = new Vector3(to.x, to.y, z);
		SetPosition(from);
		while (Vector3.SqrMagnitude(position - TargetPosition()) > 0.1f)
		{
			yield return null;
		}
	}

	public void SetInitialPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		position = new Vector3(0f, 8f, z);
		((Component)this).transform.position = position;
	}

	public void SetPosition(Vector3 p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		position = new Vector3(p.x, p.y, z);
		((Component)this).transform.position = position;
	}

	private Vector3 TargetPosition()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (mode == Mode.followHero && (Object)(object)Globals.Hero != (Object)null)
		{
			return new Vector3(initialX + followPrefactor * (((Component)Globals.Hero).transform.position.x - initialX), 0f, z);
		}
		if (mode == Mode.screenScroll)
		{
			return screenScrollTargetPosition;
		}
		if (mode == Mode.openingTransition)
		{
			return new Vector3(0f, 0f, z);
		}
		return Vector3.zero;
	}

	public IEnumerator ScrollsToX(float x)
	{
		Vector3 to = default(Vector3);
		((Vector3)(ref to))._002Ector(x, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
		yield return ((MonoBehaviour)this).StartCoroutine(EnableScreenScroll(((Component)this).transform.position, to));
	}
}
