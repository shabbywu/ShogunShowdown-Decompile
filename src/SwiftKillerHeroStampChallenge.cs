using ProgressionEnums;
using UnlocksID;

public class SwiftKillerHeroStampChallenge : HeroStampChallenge
{
	private static int targetMinutes = 30;

	private static int targetMinutesUltimate = 20;

	public override HeroStamp HeroStampEnum { get; } = HeroStamp.swiftKiller;


	public override string Description => string.Format(base.UnformattedDescription, targetMinutes);

	public override string UltimateDescription => string.Format(base.UnformattedUltimateDescription, targetMinutesUltimate);

	public override UnlockID QuestUnlockID { get; } = UnlockID.q_swift_killer_hero_stamp;


	protected override string LocalizationTableKey { get; } = "Stamp_SwiftKiller";


	public override HeroStampRank GetRankForRun(RunMetrics runMetric, Hero hero)
	{
		if (CombatSceneManager.Instance.RunTime <= (float)(targetMinutesUltimate * 60))
		{
			return HeroStampRank.ultimate;
		}
		if (CombatSceneManager.Instance.RunTime <= (float)(targetMinutes * 60))
		{
			return HeroStampRank.regular;
		}
		return HeroStampRank.noRank;
	}
}
