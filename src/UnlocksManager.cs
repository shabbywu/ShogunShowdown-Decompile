using System;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using UnlocksID;

public class UnlocksManager : MonoBehaviour, ISavable
{
	public UnlocksSaveData unlocksSaveData;

	public static UnlocksManager Instance { get; private set; }

	public List<UnlockID> RecentlyUnlockedTilesID => unlocksSaveData.recentlyUnlockedTiles;

	public int NumberOfDaimyosDefeated
	{
		get
		{
			int num = 0;
			if (Unlocked(UnlockID.q_daimyo_1_defeated))
			{
				num++;
			}
			if (Unlocked(UnlockID.q_daimyo_2_defeated))
			{
				num++;
			}
			if (Unlocked(UnlockID.q_daimyo_3_defeated))
			{
				num++;
			}
			return num;
		}
	}

	public bool ShogunDefeated => Unlocked(UnlockID.q_shogun_defeated);

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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(BeginRun));
	}

	private void BeginRun()
	{
		unlocksSaveData.unlockedDuringThisRun = new List<UnlockID>();
	}

	public bool Unlocked(UnlockID id)
	{
		return unlocksSaveData.unlocked.Contains(id);
	}

	public bool UnlockedDuringThisRun(UnlockID id)
	{
		return unlocksSaveData.unlockedDuringThisRun.Contains(id);
	}

	public void Unlock(UnlockID id)
	{
		if (unlocksSaveData.unlocked.Contains(id))
		{
			Debug.LogError((object)$"MetaProgressionManager: Unlock: Requested id '{id}' is already unlocked");
		}
		unlocksSaveData.unlocked.Add(id);
		unlocksSaveData.unlockedDuringThisRun.Add(id);
		if (ID.GetUnlockType(id) == UnlockType.tile)
		{
			unlocksSaveData.recentlyUnlockedTiles.Add(id);
		}
		PopulateSaveData(SaveDataManager.Instance.saveData);
		SaveDataManager.Instance.StoreSaveData();
	}

	public void UnlockEverything()
	{
		foreach (UnlockID value in Enum.GetValues(typeof(UnlockID)))
		{
			if (!Unlocked(value))
			{
				Unlock(value);
			}
		}
		PopulateSaveData(SaveDataManager.Instance.saveData);
		SaveDataManager.Instance.StoreSaveData();
	}

	public void PopulateSaveData(SaveData saveData)
	{
		saveData.unlocksSaveData = unlocksSaveData;
	}

	public void LoadFromSaveData(SaveData saveData)
	{
		unlocksSaveData = saveData.unlocksSaveData;
	}

	public List<UnlockID> GetAllUnlockables()
	{
		return unlocksSaveData.unlockables;
	}

	public List<AttackEnum> GetUnlockedTiles()
	{
		List<AttackEnum> list = new List<AttackEnum>();
		foreach (KeyValuePair<UnlockID, AttackEnum> item in ID.tilesID)
		{
			if (unlocksSaveData.unlocked.Contains(item.Key))
			{
				list.Add(item.Value);
			}
		}
		return list;
	}

	public bool TileUnlocked(AttackEnum attackEnum)
	{
		foreach (KeyValuePair<UnlockID, AttackEnum> item in ID.tilesID)
		{
			if (item.Value == attackEnum && unlocksSaveData.unlocked.Contains(item.Key))
			{
				return true;
			}
		}
		return false;
	}

	public bool SkillUnlocked(Type type)
	{
		foreach (KeyValuePair<UnlockID, Type> item in ID.skillsID)
		{
			if (item.Value == type && unlocksSaveData.unlocked.Contains(item.Key))
			{
				return true;
			}
		}
		return false;
	}
}
