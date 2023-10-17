using ProgressionEnums;
using UnlocksID;

public class StrategistHeroStampChallenge : HeroStampChallenge
{
	private static int nTargetTurns = 1000;

	private static int nTargetTurnsUltimate = 900;

	public override HeroStamp HeroStampEnum { get; } = HeroStamp.strategist;


	public override string Description => string.Format(base.UnformattedDescription, nTargetTurns);

	public override string UltimateDescription => string.Format(base.UnformattedUltimateDescription, nTargetTurnsUltimate);

	public override UnlockID QuestUnlockID { get; } = UnlockID.q_strategist_hero_stamp;


	protected override string LocalizationTableKey { get; } = "Stamp_Strategist";


	public override HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero)
	{
		if (MetricsManager.Instance.runMetrics.runStats.turns <= nTargetTurnsUltimate)
		{
			return HeroStampRank.ultimate;
		}
		if (MetricsManager.Instance.runMetrics.runStats.turns <= nTargetTurns)
		{
			return HeroStampRank.regular;
		}
		return HeroStampRank.noRank;
	}
}
