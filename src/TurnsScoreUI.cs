public class TurnsScoreUI : ScoreUI
{
	protected override string ScoreNameLocalizationTableKey { get; } = "Turns";


	protected override int Value => MetricsManager.Instance.runMetrics.runStats.turns;

	protected override int BestValue => Globals.Hero.CharacterSaveData.bestTurns;

	protected override string BestLocalizationTableKey { get; } = "Best";


	protected override string NewBestLocalizationTableKey { get; } = "NewBestScore";


	protected override string FormatValue(int value)
	{
		return $"{value}";
	}
}
