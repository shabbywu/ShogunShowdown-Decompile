using UnityEngine;

public class HeroSelectionSideBars : MonoBehaviour
{
	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private float xShow;

	[SerializeField]
	private float xHide;

	private bool _show;

	private Vector3 leftVelocity;

	private Vector3 rightVelocity;

	private Vector3 leftTargetLocalPosition;

	private Vector3 rightTargetLocalPosition;

	private static float smoothTime = 0.1f;

	private static float positionReachedThreshold = 0.01f;

	public bool Show
	{
		get
		{
			return _show;
		}
		set
		{
			_show = value;
			if (_show)
			{
				((Component)this).gameObject.SetActive(true);
			}
		}
	}

	private float XTarget
	{
		get
		{
			if (Show)
			{
				return xShow;
			}
			return xHide;
		}
	}

	private void OnEnable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		left.transform.localPosition = xHide * Vector3.left;
		right.transform.localPosition = xHide * Vector3.right;
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		leftTargetLocalPosition = XTarget * Vector3.left;
		rightTargetLocalPosition = XTarget * Vector3.right;
		left.transform.localPosition = Vector3.SmoothDamp(left.transform.localPosition, leftTargetLocalPosition, ref leftVelocity, smoothTime);
		right.transform.localPosition = Vector3.SmoothDamp(right.transform.localPosition, rightTargetLocalPosition, ref rightVelocity, smoothTime);
		if (!Show && TargetPositionsReached())
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	private bool TargetPositionsReached()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (Vector3.SqrMagnitude(left.transform.localPosition - leftTargetLocalPosition) < positionReachedThreshold)
		{
			return Vector3.SqrMagnitude(right.transform.localPosition - rightTargetLocalPosition) < positionReachedThreshold;
		}
		return false;
	}
}
