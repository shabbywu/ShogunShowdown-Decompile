using System.Collections;
using UnityEngine;
using Utils;

public class CampRoom : Room
{
	[SerializeField]
	private Shop unlocksShop;

	[SerializeField]
	private HeroSelection heroSelection;

	[SerializeField]
	private MetaProgressionUI metaProgressionUI;

	public override string BannerTextEnd
	{
		get
		{
			string text = Globals.Hero.Name ?? "";
			if (UnlocksManager.Instance.ShogunDefeated)
			{
				text = text + "\n" + string.Format(LocalizationUtils.LocalizedString("Terms", "Day"), Globals.Day);
			}
			return text;
		}
	}

	public Shop UnlocksShop => unlocksShop;

	public HeroSelection HeroSelection => heroSelection;

	public MetaProgressionUI MetaProgressionUI => metaProgressionUI;

	public override void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		base.Initialize(name, id, loadRoomStateFromSaveData);
		unlocksShop.Initialize();
	}

	public override void Begin()
	{
		base.Name = "camp";
		Globals.InCamp = true;
		Globals.TilesInfoMode = true;
		Globals.GamepadTilesInfoMode = true;
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.combat;
		TilesManager.Instance.ShowTilesLevel(value: true);
		TilesManager.Instance.CanInteractWithTiles = false;
		CombatManager.Instance.AllowTileInteraction = false;
		CombatManager.Instance.BeginCombat(campMode: true);
		Globals.Hero.SetCombatUIActive(value: false);
	}

	public override void End()
	{
		Globals.TilesInfoMode = false;
		Globals.GamepadTilesInfoMode = false;
		Globals.InCamp = false;
		EventsManager.Instance.BeginRun.Invoke();
	}

	public override IEnumerator ProcessTurn()
	{
		yield return null;
	}
}
