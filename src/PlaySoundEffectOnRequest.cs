using UnityEngine;

public class PlaySoundEffectOnRequest : MonoBehaviour
{
	public string soundEffectName;

	public void Play()
	{
		SoundEffectsManager.Instance.Play(soundEffectName);
	}
}
