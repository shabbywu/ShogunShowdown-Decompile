using UnityEngine;

public class FlickeringStats : MonoBehaviour
{
	[SerializeField]
	private Color defaultColor;

	[SerializeField]
	private Color blinkColor;

	[SerializeField]
	private float approximateBlinkingPeriod;

	private void Awake()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderer[] componentsInChildren = ((Component)this).GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer obj in componentsInChildren)
		{
			obj.color = defaultColor;
			LeanTween.color(((Component)obj).gameObject, blinkColor, Random.Range(0.8f, 1.2f) * approximateBlinkingPeriod).setEaseInExpo().setLoopPingPong();
		}
	}
}
