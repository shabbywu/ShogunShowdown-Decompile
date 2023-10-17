using ProgressionEnums;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HeroStampQuest", menuName = "SO/Quests/HeroStampQuest", order = 1)]
public class HeroStampQuest : Quest
{
	[SerializeField]
	private HeroStamp heroStamp;

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
		if (this.heroStamp == heroStamp)
		{
			QuestCompleted();
		}
	}
}
