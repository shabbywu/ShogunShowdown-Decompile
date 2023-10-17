using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

public abstract class ScoreUI : MonoBehaviour
{
	public enum BestScoreCondition
	{
		largerIsBetter,
		smallerIsBetter
	}

	public BestScoreCondition bestScoreCondition;

	public TextMeshProUGUI textTMPro;

	public TextMeshProUGUI valueTMPro;

	protected abstract string ScoreNameLocalizationTableKey { get; }

	private string ScoreName => LocalizationUtils.LocalizedString("Terms", ScoreNameLocalizationTableKey);

	protected abstract int Value { get; }

	protected abstract int BestValue { get; }

	protected abstract string BestLocalizationTableKey { get; }

	protected abstract string NewBestLocalizationTableKey { get; }

	protected abstract string FormatValue(int value);

	private void Start()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		if (!UnlocksManager.Instance.ShogunDefeated || Globals.Tutorial)
		{
			((Component)this).gameObject.SetActive(false);
			EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
			return;
		}
		DisplayBest();
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
		EventsManager.Instance.NewHeroSelected.AddListener(new UnityAction(NewHeroSelected));
		EventsManager.Instance.NewDifficultySelected.AddListener(new UnityAction(NewDifficultySelected));
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(BeginRun));
	}

	public void GameOver(bool win)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).CancelInvoke();
		if (win && UnlocksManager.Instance.ShogunDefeated && (BestValue < 0 || (bestScoreCondition == BestScoreCondition.smallerIsBetter && Value < BestValue) || (bestScoreCondition == BestScoreCondition.largerIsBetter && Value > BestValue)))
		{
			((Component)this).gameObject.SetActive(true);
			((TMP_Text)textTMPro).text = string.Format(LocalizationUtils.LocalizedString("Terms", NewBestLocalizationTableKey), ScoreName);
			((Graphic)textTMPro).color = Colors.FromHex(Colors.birghtYellowHex);
			((Graphic)valueTMPro).color = Colors.FromHex(Colors.birghtYellowHex);
			UpdateValue();
		}
	}

	private void BeginRun()
	{
		((TMP_Text)textTMPro).text = ScoreName;
		((MonoBehaviour)this).InvokeRepeating("UpdateValue", 0f, 0.1f);
	}

	private void NewHeroSelected()
	{
		((Component)textTMPro).gameObject.SetActive(Globals.Hero.Unlocked);
		((Component)valueTMPro).gameObject.SetActive(Globals.Hero.Unlocked);
		DisplayBest();
	}

	private void NewDifficultySelected()
	{
		DisplayBest();
	}

	private void UpdateValue()
	{
		((TMP_Text)valueTMPro).text = FormatValue(Value);
	}

	private void DisplayBest()
	{
		((TMP_Text)textTMPro).text = string.Format(LocalizationUtils.LocalizedString("Terms", BestLocalizationTableKey), ScoreName);
		if (BestValue >= 0)
		{
			((TMP_Text)valueTMPro).text = FormatValue(BestValue);
		}
		else
		{
			((TMP_Text)valueTMPro).text = "-";
		}
	}
}
