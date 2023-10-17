using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;
using UnlocksID;

public class UnlockedTilesPage : MetaProgressionUIPage
{
	[SerializeField]
	private TileArchiveSlot tileArchiveSlotPrefab;

	[SerializeField]
	private Transform gridLayout;

	private List<TileArchiveSlot> tileArchiveSlots = new List<TileArchiveSlot>();

	protected override int NUnlocks => ID.tilesID.Count;

	protected override int NUnlocked => UnlocksManager.Instance.GetUnlockedTiles().Count;

	public override List<INavigationTarget> Targets => tileArchiveSlots.Cast<INavigationTarget>().ToList();

	public override int NumberOfElementsPerPage => 14;

	public override void UpdateForPage()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)gridLayout).transform)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		List<UnlockID> elements = new List<UnlockID>(ID.tilesID.Keys);
		List<UnlockID> elementsOnPage = GetElementsOnPage(elements);
		InstantiateTileDisplayContainersForPage(elementsOnPage);
	}

	protected override void OnEnable()
	{
		UpdateForPage();
		base.OnEnable();
	}

	private void InstantiateTileDisplayContainersForPage(List<UnlockID> tileIDsOnThisPage)
	{
		tileArchiveSlots.Clear();
		foreach (UnlockID item in tileIDsOnThisPage)
		{
			TileArchiveSlot component = Object.Instantiate<GameObject>(((Component)tileArchiveSlotPrefab).gameObject, gridLayout).GetComponent<TileArchiveSlot>();
			tileArchiveSlots.Add(component);
			if (UnlocksManager.Instance.Unlocked(item))
			{
				component.AttachTile(TilesFactory.Instance.Create(ID.tilesID[item]));
			}
		}
	}
}
