using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;

public class QuestsPage : MetaProgressionUIPage
{
	[SerializeField]
	private GameObject questUIPrefab;

	[SerializeField]
	private Transform gridLayout;

	private List<QuestUI> questUIs = new List<QuestUI>();

	protected override int NUnlocks => QuestsManager.Instance.Quests.Count;

	protected override int NUnlocked => QuestsManager.Instance.NumberOfCompletedQuests;

	public override List<INavigationTarget> Targets => questUIs.Cast<INavigationTarget>().ToList();

	public override int NumberOfElementsPerPage => 12;

	public override void UpdateForPage()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_ = base.SelectedTarget;
		foreach (Transform item in ((Component)gridLayout).transform)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		List<Quest> elementsOnPage = GetElementsOnPage(QuestsManager.Instance.Quests);
		InstantiateQuestsUIForPage(elementsOnPage);
	}

	protected override void OnEnable()
	{
		UpdateForPage();
		base.OnEnable();
	}

	private void InstantiateQuestsUIForPage(List<Quest> questsOnThisPage)
	{
		questUIs.Clear();
		foreach (Quest item in questsOnThisPage)
		{
			QuestUI component = Object.Instantiate<GameObject>(questUIPrefab, gridLayout).GetComponent<QuestUI>();
			component.Initialize(item);
			questUIs.Add(component);
		}
	}
}
