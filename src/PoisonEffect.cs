using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
		SoundEffectsManager.Instance.Play("PoisonEffect");
	}

	public void PoisonDamageEffect()
	{
		SoundEffectsManager.Instance.Play("PoisonEffect");
		animator.SetTrigger("PoisonDamage");
	}

	public void EndPoison()
	{
		animator.SetTrigger("EndPoison");
	}
}
