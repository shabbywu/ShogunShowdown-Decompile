using System.Collections;
using UnityEngine;

public class ScrollingCredits : MonoBehaviour
{
	[SerializeField]
	private RectTransform canvas;

	[SerializeField]
	private float scrollSpeed;

	[SerializeField]
	private CreditsTextBlock mirko;

	[SerializeField]
	private CreditsTextBlock art;

	[SerializeField]
	private CreditsTextBlock music;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CreditsRollSequence());
	}

	private IEnumerator CreditsRollSequence()
	{
		MusicManager.Instance.Play("Credits");
		CanvasGroup canvasGroup = ((Component)canvas).GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		((Component)canvas).gameObject.SetActive(false);
		yield return (object)new WaitForFixedUpdate();
		yield return (object)new WaitForFixedUpdate();
		((Component)canvas).gameObject.SetActive(true);
		yield return (object)new WaitForFixedUpdate();
		yield return (object)new WaitForFixedUpdate();
		((Component)canvas).gameObject.SetActive(false);
		yield return (object)new WaitForFixedUpdate();
		yield return (object)new WaitForFixedUpdate();
		((Component)canvas).gameObject.SetActive(true);
		yield return (object)new WaitForFixedUpdate();
		yield return (object)new WaitForFixedUpdate();
		mirko.Hide();
		art.Hide();
		music.Hide();
		LeanTween.alphaCanvas(canvasGroup, 1f, 0.2f);
		yield return (object)new WaitForSeconds(0.1f);
		yield return (object)new WaitForSeconds(2.5f);
		mirko.Show();
		yield return (object)new WaitForSeconds(2.5f);
		art.Show();
		yield return (object)new WaitForSeconds(2.5f);
		music.Show();
		yield return (object)new WaitForSeconds(4f);
		float num = canvas.sizeDelta.y * ((Transform)canvas).localScale.y;
		LeanTween.moveLocalY(((Component)canvas).gameObject, num, num / scrollSpeed);
	}
}
