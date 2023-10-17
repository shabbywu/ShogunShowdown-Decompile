using System.Collections;
using TMPro;
using UnityEngine;

public class FightBanner : MonoBehaviour
{
	public TextMeshProUGUI mainText;

	public GameObject container;

	private bool isAnimationOver;

	private string soundEffect;

	public void AnimationOver()
	{
		isAnimationOver = true;
	}

	public void CommentAppears()
	{
		EffectsManager.Instance.ScreenShake();
		if (soundEffect != "")
		{
			SoundEffectsManager.Instance.Play(soundEffect);
		}
	}

	private IEnumerator AppearWaitAndDisappear(float wait)
	{
		container.SetActive(true);
		isAnimationOver = false;
		((Component)this).GetComponent<Animator>().SetTrigger("Appear");
		yield return (object)new WaitForSeconds(wait);
		((Component)this).GetComponent<Animator>().SetTrigger("Disappear");
		while (!isAnimationOver)
		{
			yield return null;
		}
		container.SetActive(false);
	}

	public IEnumerator BeginFightTransition(Room room)
	{
		if (!(room.BannerTextBegin == ""))
		{
			soundEffect = "BeginFight";
			((TMP_Text)mainText).text = room.BannerTextBegin;
			yield return ((MonoBehaviour)this).StartCoroutine(AppearWaitAndDisappear(1.5f));
		}
	}

	public IEnumerator EndFightTransition(Room room)
	{
		if (!(room.BannerTextEnd == ""))
		{
			soundEffect = "EndFight";
			((TMP_Text)mainText).text = room.BannerTextEnd;
			yield return ((MonoBehaviour)this).StartCoroutine(AppearWaitAndDisappear(1.5f));
		}
	}
}
