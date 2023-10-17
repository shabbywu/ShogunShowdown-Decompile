using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UniformSpriteSequenceAnimation : MonoBehaviour
{
	[SerializeField]
	private Sprite[] frames;

	[SerializeField]
	private float frameDuration;

	[SerializeField]
	private bool destroyAfterFirstLoopIsOver;

	[SerializeField]
	private bool loop = true;

	private SpriteRenderer spriteRenderer;

	private int iFrame;

	private int nFrames;

	private Coroutine animationLoopCoroutine;

	private void Awake()
	{
		spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		nFrames = frames.Length;
	}

	private void OnEnable()
	{
		animationLoopCoroutine = ((MonoBehaviour)this).StartCoroutine(AnimationLoop());
	}

	private void OnDisable()
	{
		if (animationLoopCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(animationLoopCoroutine);
			animationLoopCoroutine = null;
		}
	}

	private IEnumerator AnimationLoop()
	{
		iFrame = 0;
		spriteRenderer.sprite = frames[0];
		while (true)
		{
			yield return (object)new WaitForSeconds(frameDuration);
			NextFrame();
		}
	}

	private void NextFrame()
	{
		iFrame++;
		if (iFrame >= nFrames)
		{
			if (destroyAfterFirstLoopIsOver)
			{
				Object.Destroy((Object)(object)((Component)this).gameObject);
				return;
			}
			if (!loop)
			{
				iFrame = 0;
				((Component)this).gameObject.SetActive(false);
				return;
			}
			iFrame = 0;
		}
		spriteRenderer.sprite = frames[iFrame];
	}
}
