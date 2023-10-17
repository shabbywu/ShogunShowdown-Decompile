using Utils;

public class TimeScoreUI : ScoreUI
{
	protected override string ScoreNameLocalizationTableKey { get; } = "Time";


	protected override int Value => (int)CombatSceneManager.Instance.RunTime;

	protected override int BestValue => Globals.Hero.CharacterSaveData.bestTime;

	protected override string BestLocalizationTableKey { get; } = "Best";


	protected override string NewBestLocalizationTableKey { get; } = "NewBestScore";


	protected override string FormatValue(int value)
	{
		return MyTime.ToMinAndSecFormat(value);
	}
}
