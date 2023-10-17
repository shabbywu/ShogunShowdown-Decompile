public class DayScoreUI : ScoreUI
{
	protected override string ScoreNameLocalizationTableKey { get; } = "DayScore";


	protected override int Value => Globals.Day;

	protected override int BestValue => Globals.Hero.CharacterSaveData.bestDay;

	protected override string BestLocalizationTableKey { get; } = "Highest";


	protected override string NewBestLocalizationTableKey { get; } = "NewHighestScore";


	protected override string FormatValue(int value)
	{
		return $"{value}";
	}
}
