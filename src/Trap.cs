using System.Collections;
using TileEnums;
using UnityEngine;

public class Trap : MonoBehaviour
{
	public static float placingTime = 0.2f;

	private bool armed;

	private bool alreadyTriggered;

	private int damage;

	private Animator animator;

	private AttackEffectEnum _attackEffect;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void Initialize(int trapDamage, AttackEffectEnum attackEffect, Vector3 agentPosition, Vector3 cellPosition)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		damage = trapDamage;
		_attackEffect = attackEffect;
		((MonoBehaviour)this).StartCoroutine(PlaceTrap(agentPosition, cellPosition));
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Agent component = ((Component)other).gameObject.GetComponent<Agent>();
		if (armed && !alreadyTriggered && (Object)(object)component != (Object)null && ((Component)component).CompareTag("Enemy"))
		{
			alreadyTriggered = true;
			animator.SetTrigger("GoOff");
			SoundEffectsManager.Instance.Play("TrapGoingOff");
			SoundEffectsManager.Instance.Play("CombatHit");
			EffectsManager.Instance.ScreenShake();
			component.ReceiveAttack(new Hit(damage, isDirectional: false), null);
			Attack.ProcessAttackEffects(component, _attackEffect);
		}
	}

	private IEnumerator PlaceTrap(Vector3 initialPosition, Vector3 finalPosition)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float currentTime = 0f;
		while (currentTime < placingTime)
		{
			((Component)this).transform.position = Vector3.Lerp(initialPosition, finalPosition, currentTime / placingTime);
			currentTime += Time.deltaTime;
			yield return null;
		}
		((Component)this).transform.position = finalPosition;
		armed = true;
	}
}
