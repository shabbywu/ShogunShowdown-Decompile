using ProgressionEnums;
using UnlocksID;

public class TotalObliterationHeroStampChallenge : HeroStampChallenge
{
	private static int target = 7;

	private static int targetUltimate = 0;

	public override HeroStamp HeroStampEnum { get; } = HeroStamp.totalOblitaration;


	public override string Description => string.Format(base.UnformattedDescription, target);

	public override string UltimateDescription => base.UnformattedUltimateDescription;

	public override UnlockID QuestUnlockID { get; } = UnlockID.q_obliteration_hero_stamp;


	protected override string LocalizationTableKey { get; } = "Stamp_Obliteration";


	public override HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero)
	{
		if (MetricsManager.Instance.runMetrics.runStats.hits <= targetUltimate)
		{
			return HeroStampRank.ultimate;
		}
		if (MetricsManager.Instance.runMetrics.runStats.hits <= target)
		{
			return HeroStampRank.regular;
		}
		return HeroStampRank.noRank;
	}
}
