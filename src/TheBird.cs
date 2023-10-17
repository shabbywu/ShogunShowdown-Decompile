using System;
using UnityEngine;
using UnityEngine.Events;

public class TheBird : MonoBehaviour
{
	[SerializeField]
	private Transform flyTarget;

	[SerializeField]
	private float speed;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(FlyAway));
	}

	public void FlyAway()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector3.Distance(((Component)this).transform.position, ((Component)flyTarget).transform.position) / speed;
		((Component)this).GetComponent<Animator>().SetTrigger("Fly");
		LeanTween.move(((Component)this).gameObject, ((Component)flyTarget).transform.position, num).setEase((LeanTweenType)16).setOnComplete((Action)delegate
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		});
	}

	public void ButtonHighlighted(bool value)
	{
		if (value)
		{
			((Component)this).GetComponent<Animator>().SetTrigger("Sing");
		}
	}

	public void TakeOffAnimationOver()
	{
		((Renderer)((Component)this).GetComponentInChildren<SpriteRenderer>()).sortingLayerName = "Main";
	}

	public void PlayCawSound()
	{
		SoundEffectsManager.Instance.Play("TheBirdCaw");
	}

	public void PlayWingFlapSound()
	{
		SoundEffectsManager.Instance.Play("WingFlap");
	}
}
