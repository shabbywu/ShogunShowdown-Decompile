using UnityEngine;
using Utils;

public class Parallax : MonoBehaviour
{
	private Transform cam;

	public float parallaxFactor;

	private float initialShift;

	private float initialX;

	public float ParallaxDisplacement => (((Component)cam).transform.position.x - initialShift) * parallaxFactor;

	private void Awake()
	{
		cam = ((Component)Camera.main).transform;
	}

	private void LateUpdate()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = PixelUtils.PixelPerfectClamp(new Vector3(initialX + ParallaxDisplacement, ((Component)this).transform.position.y, ((Component)this).transform.position.z));
	}

	public void Initialize(float shift)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		initialShift = shift;
		initialX = ((Component)this).transform.position.x;
	}
}
