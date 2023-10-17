using System;
using UnityEngine;

public class CellWarningEffect : MonoBehaviour
{
	private void Start()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localScale = Vector3.zero;
		LeanTween.scale(((Component)this).gameObject, Vector3.one, 0.5f).setEase((LeanTweenType)27).setOnComplete((Action)delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			LeanTween.moveY(((Component)this).gameObject, ((Component)this).transform.position.y + 0.1f, 0.3f).setEase((LeanTweenType)7).setLoopPingPong();
		});
	}
}
