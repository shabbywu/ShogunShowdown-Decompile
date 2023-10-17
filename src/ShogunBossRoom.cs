using UnityEngine;
using Utils;

public class ShogunBossRoom : BossRoom
{
	[SerializeField]
	private Environment environmentPhase1;

	[SerializeField]
	private Environment environmentPhase2;

	[SerializeField]
	private Environment environmentPhase3;

	public override string BannerTextBegin => LocalizationUtils.LocalizedString("Locations", "ShogunShowdown");

	public void SwitchToPhase2()
	{
		((Component)environmentPhase1).gameObject.SetActive(false);
		((Component)environmentPhase2).gameObject.SetActive(true);
		environmentPhase2.Initialize();
	}

	public void SwitchToPhase3()
	{
		((Component)environmentPhase2).gameObject.SetActive(false);
		((Component)environmentPhase3).gameObject.SetActive(true);
		environmentPhase3.Initialize();
	}
}
