using Parameters;
using UnityEngine;

public class ButtonPressedTextEffect : MonoBehaviour
{
	private Vector3 initialLocalPosition;

	public int deltaYInPixels = 1;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		initialLocalPosition = ((Component)this).transform.localPosition;
	}

	public void PointerUp()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = initialLocalPosition;
	}

	public void PointerDown()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = initialLocalPosition + Vector3.down * TechParams.pixelSize * (float)deltaYInPixels;
	}
}
