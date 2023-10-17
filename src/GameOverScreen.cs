using System.Collections;
using System.Collections.Generic;
using ProgressionEnums;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnlocksID;
using Utils;

public class GameOverScreen : MonoBehaviour
{
	public MyButton continueButton;

	public HeroStampsDisplay heroStampsDisplay;

	public Image kanjiImage;

	public Sprite victoryKanji;

	public Sprite deathKanji;

	public TextMeshProUGUI kanjiMeaningText;

	public TextMeshProUGUI mainText;

	public List<GameObject> toDisableUponGameOver;

	private Animator animator;

	private bool animationInProgress;

	private string VictoryDescription
	{
		get
		{
			string text = "";
			if (UnlocksManager.Instance.UnlockedDuringThisRun(UnlockID.h_shadow) || UnlocksManager.Instance.UnlockedDuringThisRun(UnlockID.h_ronin))
			{
				text = text + "\n" + GameOverString("Victory_NewHeroUnlocked");
			}
			if (UnlocksManager.Instance.UnlockedDuringThisRun(UnlockID.q_shogun_defeated))
			{
				return GameOverString("Victory_FirstEverShogun") + text;
			}
			if (Progression.Instance.IsShogunFight && Globals.Day > Globals.Hero.CharacterSaveData.bestDay && Globals.Day < Globals.CurrentlyImplementedMaxDay)
			{
				return string.Format(GameOverString("Victory_NewDayUnlocked"), Globals.Day + 1) + text;
			}
			if (Progression.Instance.IsShogunFight)
			{
				return GameOverString("Victory_RegularShogunKill") + text;
			}
			return GameOverString("Victory_UndefeatedDaimyos");
		}
	}

	private string DeathDescription
	{
		get
		{
			string text = "";
			Agent lastAttacker = Globals.Hero.LastAttacker;
			if ((Object)(object)lastAttacker == (Object)(object)Globals.Hero)
			{
				text = GameOverString("Death_Seppuku");
				return string.Format(text, Globals.Hero.Name);
			}
			if (lastAttacker is Boss)
			{
				text = GameOverString("Death_KilledDefinite");
				return string.Format(text, Globals.Hero.Name, lastAttacker.Name);
			}
			text = GameOverString("Death_KilledIndefinite");
			return string.Format(text, Globals.Hero.Name, lastAttacker.Name);
		}
	}

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	private void Start()
	{
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
	}

	public void GameOver(bool win)
	{
		InitializeForWinState(win);
		((MonoBehaviour)this).StartCoroutine(SequenceCoroutine(win));
	}

	private void InitializeForWinState(bool win)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Color white = Color.white;
		if (win)
		{
			kanjiImage.sprite = victoryKanji;
			((TMP_Text)kanjiMeaningText).text = "- " + LocalizationUtils.LocalizedString("Terms", "Victory") + " -";
			((TMP_Text)mainText).text = VictoryDescription;
			white = Colors.FromHex(Colors.birghtYellowHex);
		}
		else
		{
			kanjiImage.sprite = deathKanji;
			((TMP_Text)kanjiMeaningText).text = "- " + LocalizationUtils.LocalizedString("Terms", "Death") + " -";
			((TMP_Text)mainText).text = DeathDescription;
			white = Colors.FromHex(Colors.brigthRedHex);
		}
		((Graphic)kanjiImage).color = white;
		((Graphic)kanjiMeaningText).color = white;
	}

	private IEnumerator SequenceCoroutine(bool win)
	{
		foreach (GameObject item in toDisableUponGameOver)
		{
			item.SetActive(false);
		}
		foreach (Agent agent in CombatManager.Instance.Agents)
		{
			agent.ExitCombatMode();
			agent.SetCombatUIActive(value: false);
		}
		if (!win)
		{
			SoundEffectsManager.Instance.Play("GameOverDeath");
			MusicManager.Instance.Play("Death", 0.1f);
		}
		yield return ((MonoBehaviour)this).StartCoroutine(EffectsManager.Instance.SlowMotionEffect());
		((Component)continueButton).gameObject.SetActive(false);
		animator.SetTrigger("GameOver");
		animationInProgress = true;
		while (animationInProgress)
		{
			yield return null;
		}
		yield return (object)new WaitForSeconds(2f);
		if (win && Progression.Instance.IsShogunFight)
		{
			Globals.Hero.CharacterSaveData.ShogunDefeated(Globals.Day, (int)CombatSceneManager.Instance.RunTime, MetricsManager.Instance.runMetrics.runStats.turns, MetricsManager.Instance.runMetrics.runStats.combos, MetricsManager.Instance.runMetrics.runStats.hits);
			yield return ((MonoBehaviour)this).StartCoroutine(ProcessHeroStamps());
			yield return (object)new WaitForSeconds(0.25f);
		}
		animator.SetTrigger("ContextualTextAppear");
		((Component)continueButton).gameObject.SetActive(true);
		Globals.Hero.PopulateSaveData(SaveDataManager.Instance.saveData);
		SaveDataManager.Instance.runSaveData.hasRunInProgress = false;
		SaveDataManager.Instance.StoreSaveData();
	}

	private string GameOverString(string key)
	{
		return LocalizationUtils.LocalizedString("Metaprogression", key);
	}

	public void Restart()
	{
		continueButton.Disappear();
		MusicManager.Instance.SmoothSetVolume(Globals.Options.musicVolume, 0, 1f);
		SceneLoader.Instance.LoadScene("ResetGameState");
	}

	public IEnumerator ProcessHeroStamps()
	{
		((Component)heroStampsDisplay).gameObject.SetActive(true);
		heroStampsDisplay.Initialize(Globals.Hero);
		HeroStampUI[] stamps = heroStampsDisplay.stamps;
		foreach (HeroStampUI heroStampUI in stamps)
		{
			HeroStampChallenge stampChallenge = heroStampUI.stampChallenge;
			HeroStamp heroStampEnum = stampChallenge.HeroStampEnum;
			HeroStampRank rankForRun = stampChallenge.GetRankForRun(MetricsManager.Instance.runMetrics, Globals.Hero);
			if (rankForRun > Globals.Hero.CharacterSaveData.GetHeroStampRank(heroStampEnum))
			{
				Globals.Hero.CharacterSaveData.AddHeroStamp(heroStampEnum, rankForRun);
				heroStampUI.UnlockAnimation();
				heroStampsDisplay.Initialize(Globals.Hero);
				EventsManager.Instance.HeroStampObtained.Invoke(heroStampEnum);
				yield return (object)new WaitForSeconds(1.2f);
			}
		}
		EventsManager.Instance.VictoryScreenStampDisplaySequenceOver.Invoke(heroStampsDisplay);
	}

	public void ScreenShake()
	{
		EffectsManager.Instance.ScreenShake();
	}

	public void KanjiSound()
	{
		SoundEffectsManager.Instance.Play("UnlockHeroStamp");
	}

	public void AnimationOver()
	{
		animationInProgress = false;
	}
}
