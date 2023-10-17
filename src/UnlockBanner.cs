using TMPro;
using UnityEngine;

public class UnlockBanner : MonoBehaviour
{
	public TextMeshProUGUI textTMPro;

	private string soundEffectName;

	public void Initialize(string text, string soundEffectName)
	{
		((TMP_Text)textTMPro).text = text;
		this.soundEffectName = soundEffectName;
	}

	public void PlaySound()
	{
		SoundEffectsManager.Instance.Play(soundEffectName);
	}

	public void ScreenShake()
	{
		EffectsManager.Instance.ScreenShake();
	}
}
