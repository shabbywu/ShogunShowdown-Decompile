using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererFading : MonoBehaviour
{
	[SerializeField]
	private float fadeInOutRate = 1f;

	[SerializeField]
	private bool visible;

	private SpriteRenderer spriteRenderer;

	private float alpha;

	private float TargetAlpha
	{
		get
		{
			if (visible)
			{
				return 1f;
			}
			return 0f;
		}
	}

	public void SetVisible(bool value)
	{
		visible = value;
		((Behaviour)this).enabled = true;
		((Renderer)spriteRenderer).enabled = true;
	}

	private void Awake()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		((Renderer)spriteRenderer).enabled = visible;
		spriteRenderer.color = new Color(1f, 1f, 1f, TargetAlpha);
	}

	private void Update()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		alpha = Mathf.MoveTowards(alpha, TargetAlpha, fadeInOutRate * Time.deltaTime);
		spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
		if (alpha == TargetAlpha)
		{
			if (TargetAlpha == 0f)
			{
				((Renderer)spriteRenderer).enabled = false;
			}
			((Behaviour)this).enabled = false;
		}
	}
}
