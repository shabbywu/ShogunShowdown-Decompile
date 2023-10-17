using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
	public GameObject itemsContainer;

	public GameObject heldItemIconPrefab;

	[SerializeField]
	private SkillsIconsContainer skillsIconsContainer;

	private List<Item> items = new List<Item>();

	private List<HeldItemIcon> itemIcons = new List<HeldItemIcon>();

	private static string itemsResourcesPath = "Items";

	private bool skillsAlreadyLoaded;

	public static ItemsManager Instance { get; private set; }

	public SkillsIconsContainer SkillsIconsContainer => skillsIconsContainer;

	public List<Item> Items => items;

	public int NItems => Items.Count;

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		if (!skillsAlreadyLoaded)
		{
			Item[] componentsInChildren = itemsContainer.GetComponentsInChildren<Item>();
			foreach (Item item in componentsInChildren)
			{
				PickUpItem(item);
			}
		}
	}

	public bool HasThisItemType(Item item)
	{
		foreach (Item item2 in Items)
		{
			if (((object)item).GetType() == ((object)item2).GetType())
			{
				return true;
			}
		}
		return false;
	}

	public Item GetItemOfThisItemType(Item item)
	{
		foreach (Item item2 in Items)
		{
			if (((object)item).GetType() == ((object)item2).GetType())
			{
				return item2;
			}
		}
		return null;
	}

	public void PickUpItem(Item item)
	{
		bool flag = false;
		foreach (Item item2 in Items)
		{
			if (((object)item2).GetType() == ((object)item).GetType())
			{
				flag = true;
				item2.LevelUp();
			}
		}
		if (!flag)
		{
			items.Add(item);
			((Component)item).transform.SetParent(itemsContainer.transform);
			item.PickUp();
			GameObject val = Object.Instantiate<GameObject>(heldItemIconPrefab, ((Component)SkillsIconsContainer).transform);
			itemIcons.Add(val.GetComponent<HeldItemIcon>());
		}
		UpdateItemIcons();
	}

	public void UpdateItemIcons()
	{
		for (int i = 0; i < NItems; i++)
		{
			itemIcons[i].UpdateInfo(items[i]);
		}
	}

	public void PopulateSaveData(RunSaveData runInProgressSaveData)
	{
		foreach (Item item in Items)
		{
			runInProgressSaveData.skills.Add(item.SkillEnum);
			runInProgressSaveData.skillsLevel.Add(item.Level);
		}
	}

	public void LoadFromSaveData(RunSaveData runInProgressSaveData)
	{
		List<Item> list = (from go in Array.ConvertAll(Resources.LoadAll(itemsResourcesPath), (Converter<Object, GameObject>)((Object item) => (GameObject)item))
			select go.GetComponent<Item>()).ToList();
		int iSkill;
		for (iSkill = 0; iSkill < runInProgressSaveData.skills.Count; iSkill++)
		{
			Item item2 = list.Find((Item skill) => skill.SkillEnum == runInProgressSaveData.skills[iSkill]);
			for (int i = 0; i < runInProgressSaveData.skillsLevel[iSkill]; i++)
			{
				PickUpItem(((Component)Object.Instantiate<Item>(item2, ((Component)this).transform)).GetComponent<Item>());
			}
		}
		skillsAlreadyLoaded = true;
	}
}
