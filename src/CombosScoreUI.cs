public class CombosScoreUI : ScoreUI
{
	protected override string ScoreNameLocalizationTableKey { get; } = "Combos";


	protected override int Value => MetricsManager.Instance.runMetrics.runStats.combos;

	protected override int BestValue => Globals.Hero.CharacterSaveData.bestCombos;

	protected override string BestLocalizationTableKey { get; } = "Best";


	protected override string NewBestLocalizationTableKey { get; } = "NewBestScore";


	protected override string FormatValue(int value)
	{
		return $"{value}";
	}
}
