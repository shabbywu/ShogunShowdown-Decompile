using Steamworks;
using UnityEngine;
using UnlocksID;
using Utils;

[CreateAssetMenu(fileName = "NewQuest", menuName = "SO/Quests/", order = 1)]
public abstract class Quest : ScriptableObject
{
	[SerializeField]
	private string LocalizationTableKey;

	public UnlockID unlockID;

	public UnlockID[] additionalUnlocks;

	public UnlockID[] requiredUnlocksForUnveiled;

	public Sprite symbolNotCompleted;

	public Sprite symbolCompleted;

	public string Name => LocalizationUtils.LocalizedString("Metaprogression", LocalizationTableKey + "_Name");

	public string Description => ProcessDescription(LocalizationUtils.LocalizedString("Metaprogression", LocalizationTableKey + "_Description"));

	public bool IsCompleted => UnlocksManager.Instance.Unlocked(unlockID);

	public bool Unveiled
	{
		get
		{
			UnlockID[] array = requiredUnlocksForUnveiled;
			foreach (UnlockID id in array)
			{
				if (!UnlocksManager.Instance.Unlocked(id))
				{
					return false;
				}
			}
			return true;
		}
	}

	public abstract void Initialize();

	public abstract void FinalizeQuest();

	protected virtual string ProcessDescription(string description)
	{
		return description;
	}

	protected void QuestCompleted()
	{
		string text = TextUitls.ReplaceTags("[begin_header]" + LocalizationUtils.LocalizedString("Terms", "QuestCompleted") + "[end_header][vspace]\n" + Name + ": [low_priority_color]" + Description + "[end_color]");
		EventsManager.Instance.PushNotification.Invoke(text);
		UnlocksManager.Instance.Unlock(unlockID);
		UnlockID[] array = additionalUnlocks;
		foreach (UnlockID id in array)
		{
			if (!UnlocksManager.Instance.Unlocked(id))
			{
				UnlocksManager.Instance.Unlock(id);
			}
		}
		FinalizeQuest();
		SetSteamAchievementIfNotAlreadyObtained();
	}

	public void SetSteamAchievementIfNotAlreadyObtained()
	{
		string text = unlockID.ToString();
		bool flag = false;
		SteamUserStats.GetAchievement(text, ref flag);
		if (!flag)
		{
			SteamUserStats.SetAchievement(text);
			SteamUserStats.StoreStats();
		}
	}
}
