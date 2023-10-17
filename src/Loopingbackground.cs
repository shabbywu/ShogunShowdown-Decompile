using UnityEngine;

public class Loopingbackground : MonoBehaviour
{
	private enum LoopingAxis
	{
		X,
		Y
	}

	[SerializeField]
	private SpriteRenderer sprite_A;

	[SerializeField]
	private SpriteRenderer sprite_B;

	[SerializeField]
	private float speed;

	[SerializeField]
	private LoopingAxis loopingAxis;

	private void Start()
	{
		switch (loopingAxis)
		{
		case LoopingAxis.X:
			StartHorizontalLooping();
			break;
		case LoopingAxis.Y:
			StartVerticalLooping();
			break;
		}
	}

	private void StartHorizontalLooping()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Sign(speed);
		Bounds bounds = sprite_A.sprite.bounds;
		float num2 = 2f * ((Bounds)(ref bounds)).extents.x;
		float num3 = Mathf.Abs(num2 / speed);
		((Component)sprite_A).transform.localPosition = Vector3.zero;
		((Component)sprite_B).transform.localPosition = num * num2 * Vector3.left;
		LeanTween.moveLocalX(((Component)sprite_A).gameObject, num * num2, num3).setLoopCount(-1);
		LeanTween.moveLocalX(((Component)sprite_B).gameObject, 0f, num3).setLoopCount(-1);
	}

	private void StartVerticalLooping()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Sign(speed);
		Bounds bounds = sprite_A.sprite.bounds;
		float num2 = 2f * ((Bounds)(ref bounds)).extents.y;
		float num3 = Mathf.Abs(num2 / speed);
		((Component)sprite_A).transform.localPosition = Vector3.zero;
		((Component)sprite_B).transform.localPosition = num * num2 * Vector3.down;
		LeanTween.moveLocalY(((Component)sprite_A).gameObject, num * num2, num3).setLoopCount(-1);
		LeanTween.moveLocalY(((Component)sprite_B).gameObject, 0f, num3).setLoopCount(-1);
	}
}
