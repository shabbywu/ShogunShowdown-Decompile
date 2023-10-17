using UnityEngine;

public class WindEffect : MonoBehaviour
{
	public static float speed = 10f;

	public float directionSign = 1f;

	private void Awake()
	{
		SoundEffectsManager.Instance.Play("WindAttack");
	}

	private void Update()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.Translate(directionSign * speed * Vector3.right * Time.deltaTime);
	}
}
