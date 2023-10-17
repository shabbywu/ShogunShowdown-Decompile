using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
	[SerializeField]
	private List<Quest> quests;

	public List<Quest> Quests => quests;

	public static QuestsManager Instance { get; private set; }

	public int NumberOfCompletedQuests
	{
		get
		{
			int num = 0;
			foreach (Quest quest in Quests)
			{
				if (quest.IsCompleted)
				{
					num++;
				}
			}
			return num;
		}
	}

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
		Initialize();
	}

	private void Initialize()
	{
		foreach (Quest quest in Quests)
		{
			if (UnlocksManager.Instance.Unlocked(quest.unlockID))
			{
				quest.SetSteamAchievementIfNotAlreadyObtained();
			}
			else
			{
				quest.Initialize();
			}
		}
	}
}
