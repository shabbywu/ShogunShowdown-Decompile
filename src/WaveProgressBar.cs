using TMPro;
using UnityEngine;
using Utils;

public class WaveProgressBar : MonoBehaviour
{
	public TextMeshProUGUI text;

	private Animator animator;

	private int nWaves;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void Initialize(int iWave, int nWaves)
	{
		this.nWaves = nWaves;
		UpdateText(iWave, nWaves);
	}

	public void WaveAboutToSpawn()
	{
		animator.SetTrigger("AboutToSpawn");
	}

	public void WaveBegins(int iWave)
	{
		SoundEffectsManager.Instance.Play("WaveBegins");
		UpdateText(iWave, nWaves);
		animator.SetTrigger("UpdateWave");
	}

	private void UpdateText(int i, int n)
	{
		((TMP_Text)text).text = string.Format("{0} {1}/{2}", LocalizationUtils.LocalizedString("Terms", "Wave"), i + 1, n);
	}
}
