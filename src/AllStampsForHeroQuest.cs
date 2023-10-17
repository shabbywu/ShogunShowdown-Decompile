using ProgressionEnums;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AllStampsForHeroQuest", menuName = "SO/Quests/AllStampsForHeroQuest", order = 1)]
public class AllStampsForHeroQuest : Quest
{
	public override void Initialize()
	{
		EventsManager.Instance.HeroStampObtained.AddListener((UnityAction<HeroStamp>)HeroStampObtained);
	}

	public override void FinalizeQuest()
	{
		EventsManager.Instance.HeroStampObtained.RemoveListener((UnityAction<HeroStamp>)HeroStampObtained);
	}

	private void HeroStampObtained(HeroStamp heroStamp)
	{
		if (Globals.Hero.CharacterSaveData.stamps.Count == 5)
		{
			QuestCompleted();
		}
	}
}
