using UnityEngine;

public class NobunagaArmor : MonoBehaviour
{
	private Animator animator;

	public bool ArmorActive
	{
		set
		{
			if ((Object)(object)animator == (Object)null)
			{
				animator = ((Component)this).GetComponent<Animator>();
			}
			animator.SetBool("Active", value);
		}
	}

	public void Hit()
	{
		animator.SetTrigger("Hit");
	}

	public void OnArmorAppears()
	{
		SoundEffectsManager.Instance.Play("NobunagaArmorAppears");
	}

	public void OnArmorDisappears()
	{
		SoundEffectsManager.Instance.Play("NobunagaArmorDisappears");
	}
}
