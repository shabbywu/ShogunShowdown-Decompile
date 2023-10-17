using ProgressionEnums;
using UnlocksID;

public class ShogunSlayerHeroStampChallenge : HeroStampChallenge
{
	public override HeroStamp HeroStampEnum { get; }

	public override string Description => base.UnformattedDescription;

	public override string UltimateDescription => string.Format(base.UnformattedUltimateDescription, Globals.CurrentlyImplementedMaxDay);

	public override UnlockID QuestUnlockID { get; } = UnlockID.q_shogun_defeated;


	protected override string LocalizationTableKey { get; } = "Stamp_ShogunSlayer";


	public override HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero)
	{
		if (Globals.Day >= Globals.CurrentlyImplementedMaxDay)
		{
			return HeroStampRank.ultimate;
		}
		return HeroStampRank.regular;
	}
}
