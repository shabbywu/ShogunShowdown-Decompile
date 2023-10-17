using System.Collections;
using UnityEngine;
using Utils;

public class MapUndiscoveredCloud : MonoBehaviour
{
	public SpriteRenderer cloudSR;

	public SpriteRenderer shadowSR;

	private static float discoverAnimationTime = 1.2f;

	private static float discoverAnimationMoveSpeed = 1.2f;

	public Sprite[] possibleSprites;

	private bool discoverAlreadyCalled;

	private void Start()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.Translate(Vector3.forward * (0.001f * ((Component)this).transform.position.y + 1E-06f * ((Component)this).transform.position.x));
		cloudSR.sprite = MyRandom.NextFromArray(possibleSprites);
	}

	public void Discover()
	{
		if (!discoverAlreadyCalled)
		{
			discoverAlreadyCalled = true;
			((MonoBehaviour)this).StartCoroutine(DiscoverCoroutine());
		}
	}

	private IEnumerator DiscoverCoroutine()
	{
		SoundEffectsManager.Instance.Play("MapUncoverLocation");
		float t = 0f;
		Color color = default(Color);
		while (t < discoverAnimationTime)
		{
			((Component)this).transform.Translate(Time.deltaTime * discoverAnimationMoveSpeed * Vector3.right);
			((Color)(ref color))._002Ector(1f, 1f, 1f, 1f - t / discoverAnimationTime);
			cloudSR.color = color;
			shadowSR.color = color;
			t += Time.deltaTime;
			yield return null;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
