using UnityEngine;

public class AppearingAnimationUponStart : MonoBehaviour
{
	private void Awake()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localScale = new Vector3(0f, 0f, 0f);
	}

	private void Start()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		LeanTween.scale(((Component)this).gameObject, new Vector3(1f, 1f, 1f), 0.25f).setEase((LeanTweenType)27).setIgnoreTimeScale(true);
	}
}
