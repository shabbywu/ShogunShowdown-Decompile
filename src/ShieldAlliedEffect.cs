using System.Collections;
using UnityEngine;

public class ShieldAlliedEffect : MonoBehaviour
{
	private static float speed = 18f;

	private bool appearAnimationCompleted;

	public IEnumerator Perform(Agent shieldGiver, Agent shieldReceiver)
	{
		SoundEffectsManager.Instance.Play("ShieldAlliedAppear");
		while (!appearAnimationCompleted)
		{
			yield return null;
		}
		SoundEffectsManager.Instance.Play("ShurikenThrow");
		float flighTime = Vector3.Distance(((Component)shieldGiver).transform.position, ((Component)shieldReceiver).transform.position) / speed;
		float currentTime = 0f;
		while (currentTime < flighTime)
		{
			((Component)this).transform.position = Vector3.Lerp(((Component)shieldGiver).transform.position, ((Component)shieldReceiver).transform.position, currentTime / flighTime);
			currentTime += Time.deltaTime;
			yield return null;
		}
		shieldReceiver.AddShield();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	public void OnAppearAnimationCompleted()
	{
		appearAnimationCompleted = true;
	}
}
