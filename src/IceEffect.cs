using UnityEngine;

public class IceEffect : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		SoundEffectsManager.Instance.Play("IceEffectBegin");
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void Defrost()
	{
		SoundEffectsManager.Instance.Play("IceEffectEnd");
		animator.SetTrigger("Defrost");
	}
}
