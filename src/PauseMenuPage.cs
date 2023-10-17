using UnityEngine;

public class PauseMenuPage : MenuPage
{
	[SerializeField]
	private HeroStampsDisplay heroStampsDisplay;

	public HeroStampsDisplay HeroStampsDisplay => heroStampsDisplay;

	public override void InitializeNavigation()
	{
		base.InitializeNavigation();
		if (((Component)heroStampsDisplay).gameObject.activeSelf)
		{
			HeroStampUI[] stamps = heroStampsDisplay.stamps;
			foreach (HeroStampUI item in stamps)
			{
				navigationTargets.Insert(0, item);
			}
		}
	}
}
