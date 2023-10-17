using UnityEngine;

public class MovingCloud : MonoBehaviour
{
	public float xDestroy;

	private float v;

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.Translate(v * Vector3.right * Time.deltaTime);
		if (((Component)this).transform.localPosition.x > xDestroy)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	public void Initialize(float y, float velocity, Sprite sprite, float prewarmTime, int sortingOrder)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		v = velocity;
		((Component)this).transform.Translate(prewarmTime * v * Vector3.right + y * Vector3.up);
		SpriteRenderer component = ((Component)this).GetComponent<SpriteRenderer>();
		component.sprite = sprite;
		((Renderer)component).sortingOrder = sortingOrder;
	}
}
