using ProgressionEnums;
using UnityEngine;
using UnlocksID;
using Utils;

public abstract class HeroStampChallenge : MonoBehaviour
{
	public abstract HeroStamp HeroStampEnum { get; }

	public string StampName => LocalizationUtils.LocalizedString("Metaprogression", LocalizationTableKey + "_Name");

	public abstract string Description { get; }

	public abstract string UltimateDescription { get; }

	public abstract UnlockID QuestUnlockID { get; }

	protected abstract string LocalizationTableKey { get; }

	protected string UnformattedDescription => LocalizationUtils.LocalizedString("Metaprogression", LocalizationTableKey + "_Description");

	protected string UnformattedUltimateDescription => LocalizationUtils.LocalizedString("Metaprogression", LocalizationTableKey + "_UltimateDescription");

	public bool ShowUltimateDescription => UnlocksManager.Instance.Unlocked(QuestUnlockID);

	public abstract HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero);
}
