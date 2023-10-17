using Parameters;
using UnityEngine;

public class OscillationXY : MonoBehaviour
{
	[SerializeField]
	private float nPixelsY;

	[SerializeField]
	private float yPeriod;

	[SerializeField]
	private float nPixelsX;

	[SerializeField]
	private float xPeriod;

	[SerializeField]
	private LeanTweenType ease;

	private void Start()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		if (nPixelsX != 0f)
		{
			LeanTween.moveLocal(((Component)this).gameObject, ((Component)this).transform.localPosition + Vector3.right * TechParams.pixelSize * nPixelsX, xPeriod).setEase(ease).setLoopPingPong();
		}
		if (nPixelsY != 0f)
		{
			LeanTween.moveLocal(((Component)this).gameObject, ((Component)this).transform.localPosition + Vector3.up * TechParams.pixelSize * nPixelsY, yPeriod).setEase(ease).setLoopPingPong();
		}
	}
}
