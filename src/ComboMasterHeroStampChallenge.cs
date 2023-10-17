using ProgressionEnums;
using UnlocksID;

public class ComboMasterHeroStampChallenge : HeroStampChallenge
{
	private static int nTargetCombo = 70;

	private static int nTargetComboUltimate = 100;

	public override HeroStamp HeroStampEnum { get; } = HeroStamp.comboMaster;


	public override string Description => string.Format(base.UnformattedDescription, nTargetCombo);

	public override string UltimateDescription => string.Format(base.UnformattedUltimateDescription, nTargetComboUltimate);

	public override UnlockID QuestUnlockID { get; } = UnlockID.q_combo_hero_stamp;


	protected override string LocalizationTableKey { get; } = "Stamp_ComboMaster";


	public override HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero)
	{
		if (MetricsManager.Instance.runMetrics.runStats.combos >= nTargetComboUltimate)
		{
			return HeroStampRank.ultimate;
		}
		if (MetricsManager.Instance.runMetrics.runStats.combos >= nTargetCombo)
		{
			return HeroStampRank.regular;
		}
		return HeroStampRank.noRank;
	}
}
