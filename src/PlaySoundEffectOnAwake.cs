using UnityEngine;

public class PlaySoundEffectOnAwake : MonoBehaviour
{
	public string soundEffectName;

	private void Awake()
	{
		SoundEffectsManager.Instance.Play(soundEffectName);
	}
}
