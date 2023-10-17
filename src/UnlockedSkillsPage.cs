using System;
using System.Collections.Generic;
using System.Linq;
using UINavigation;
using UnityEngine;
using UnlocksID;
using Utils;

public class UnlockedSkillsPage : MetaProgressionUIPage
{
	[SerializeField]
	private GameObject unlockedSkillUIPrefab;

	[SerializeField]
	private Transform gridLayout;

	private List<UnlockedSkillUI> unlockedSkillUIs = new List<UnlockedSkillUI>();

	protected override int NUnlocks => ID.skillsID.Count;

	protected override int NUnlocked
	{
		get
		{
			int num = 0;
			foreach (UnlockID key in ID.skillsID.Keys)
			{
				if (UnlocksManager.Instance.Unlocked(key))
				{
					num++;
				}
			}
			return num;
		}
	}

	public override int NumberOfElementsPerPage => 33;

	public override List<INavigationTarget> Targets => unlockedSkillUIs.Cast<INavigationTarget>().ToList();

	public override void UpdateForPage()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)gridLayout).transform)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		Initialize();
	}

	protected override void OnEnable()
	{
		Initialize();
		base.OnEnable();
	}

	public void Initialize()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		unlockedSkillUIs.Clear();
		foreach (Transform item in ((Component)gridLayout).transform)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		IEnumerable<Item> enumerable = from itemGO in Array.ConvertAll(Resources.LoadAll("Items"), (Converter<Object, GameObject>)((Object item) => (GameObject)item))
			select itemGO.GetComponent<Item>();
		foreach (KeyValuePair<UnlockID, Type> item2 in ID.skillsID)
		{
			UnlockedSkillUI component = Object.Instantiate<GameObject>(unlockedSkillUIPrefab, gridLayout).GetComponent<UnlockedSkillUI>();
			unlockedSkillUIs.Add(component);
			if (UnlocksManager.Instance.Unlocked(item2.Key))
			{
				component.Unlocked = true;
				foreach (Item item3 in enumerable)
				{
					if (((object)item3).GetType() == item2.Value)
					{
						component.Sprite = item3.defaultSprite;
						component.Description = item3.GetInfoBoxText();
					}
				}
			}
			else
			{
				component.Description = LocalizationUtils.LocalizedString("Terms", "Locked");
			}
		}
	}
}
