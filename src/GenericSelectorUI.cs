using System.Collections;
using Parameters;
using UnityEngine;

public class GenericSelectorUI : MonoBehaviour
{
	[SerializeField]
	private RectTransform rect;

	private static float animationTimeSeconds = 0.5f;

	private Vector2 initialSizeDelta;

	private Vector2 largerSizeDelta;

	private IEnumerator animationLoopCoroutine;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		initialSizeDelta = rect.sizeDelta;
		largerSizeDelta = initialSizeDelta + 2f * TechParams.pixelSize * Vector2.one;
		Disable();
	}

	public void Enable()
	{
		((Component)rect).gameObject.SetActive(true);
		animationLoopCoroutine = AnimationLoop();
		((MonoBehaviour)this).StartCoroutine(animationLoopCoroutine);
	}

	public void Disable()
	{
		if (animationLoopCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(animationLoopCoroutine);
		}
		((Component)rect).gameObject.SetActive(false);
	}

	private IEnumerator AnimationLoop()
	{
		while (true)
		{
			yield return (object)new WaitForSeconds(animationTimeSeconds);
			if (rect.sizeDelta == initialSizeDelta)
			{
				rect.sizeDelta = largerSizeDelta;
			}
			else
			{
				rect.sizeDelta = initialSizeDelta;
			}
		}
	}
}
