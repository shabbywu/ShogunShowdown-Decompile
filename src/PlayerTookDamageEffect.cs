using UnityEngine;
using UnityEngine.Events;

public class PlayerTookDamageEffect : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		EventsManager.Instance.HeroTookDamage.AddListener((UnityAction<int>)PlayerTookDamage);
		animator = ((Component)this).GetComponent<Animator>();
	}

	private void PlayerTookDamage(int damage)
	{
		SoundEffectsManager.Instance.Play("SpecialHit");
		EffectsManager.Instance.MediumGamepadRumble();
		animator.SetTrigger("Enable");
	}
}
