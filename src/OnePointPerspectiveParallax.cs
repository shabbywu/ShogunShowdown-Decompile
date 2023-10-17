using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OnePointPerspectiveParallax : MonoBehaviour
{
	[SerializeField]
	private float parallaxFactor = 0.5f;

	private Parallax parentParallax;

	private float stretchDirection;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		parentParallax = ((Component)this).GetComponentInParent<Parallax>();
		stretchDirection = ((((Component)this).transform.position.x > ((Component)parentParallax).transform.position.x) ? 1 : (-1));
	}

	private void LateUpdate()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		float parallaxDisplacement = parentParallax.ParallaxDisplacement;
		float num = parallaxFactor * parallaxDisplacement * stretchDirection;
		((Component)this).transform.localScale = new Vector3(1f + num, 1f, 1f);
	}
}
