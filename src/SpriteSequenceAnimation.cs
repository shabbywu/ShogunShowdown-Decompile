using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSequenceAnimation : MonoBehaviour
{
	[SerializeField]
	private SpriteSequenceFrame[] frames;

	private SpriteRenderer spriteRenderer;

	private int iFrame;

	private int nFrames;

	private void Start()
	{
		spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		nFrames = frames.Length;
		((MonoBehaviour)this).StartCoroutine(AnimationLoop());
	}

	private IEnumerator AnimationLoop()
	{
		while (true)
		{
			yield return (object)new WaitForSeconds(frames[iFrame].duration);
			NextFrame();
		}
	}

	private void NextFrame()
	{
		iFrame++;
		if (iFrame >= nFrames)
		{
			iFrame = 0;
		}
		spriteRenderer.sprite = frames[iFrame].sprite;
	}
}
