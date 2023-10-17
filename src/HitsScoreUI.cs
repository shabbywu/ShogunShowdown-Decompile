public class HitsScoreUI : ScoreUI
{
	protected override string ScoreNameLocalizationTableKey { get; } = "Hits";


	protected override int Value => MetricsManager.Instance.runMetrics.runStats.hits;

	protected override int BestValue => Globals.Hero.CharacterSaveData.bestHits;

	protected override string BestLocalizationTableKey { get; } = "Best";


	protected override string NewBestLocalizationTableKey { get; } = "NewBestScore";


	protected override string FormatValue(int value)
	{
		return $"{value}";
	}
}
