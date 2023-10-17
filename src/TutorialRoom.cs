using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoom : CombatRoom
{
	public GameObject tutorialPanelCanvas;

	[SerializeField]
	private GameObject skipTutorialCanvas;

	public TutorialPanel[] panels;

	[SerializeField]
	private Button[] nextButtons;

	[SerializeField]
	private Button[] prevButtons;

	[SerializeField]
	private TutorialPanel panelExplainingCooldown;

	[SerializeField]
	private TutorialPanel panelExplainingSpecialMove;

	private int iPanel;

	private bool cooldownMechanicExplained;

	private bool tutorialEnded;

	private TutorialPanel CurrentPanel => panels[iPanel];

	private int PanelIndex
	{
		get
		{
			return iPanel;
		}
		set
		{
			CurrentPanel.DisablePanel();
			if (CurrentPanel.IsVolatilePanel && value != iPanel && CurrentPanel.CanGoToNextPanel)
			{
				panels = panels.Where((TutorialPanel p) => (Object)(object)p != (Object)(object)CurrentPanel).ToArray();
				iPanel = Mathf.Clamp(value, 0, iPanel);
			}
			else
			{
				iPanel = Mathf.Clamp(value, 0, panels.Length - 1);
			}
			CurrentPanel.EnablePanel();
			UpdatePrevAndNextButtons();
			if ((Object)(object)CurrentPanel == (Object)(object)panelExplainingCooldown)
			{
				cooldownMechanicExplained = true;
			}
			if ((Object)(object)CurrentPanel == (Object)(object)panelExplainingSpecialMove)
			{
				Globals.Hero.SpecialMove.IsEnabled = true;
			}
		}
	}

	public override void Begin()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		tutorialPanelCanvas.SetActive(true);
		tutorialPanelCanvas.transform.SetParent(CombatSceneManager.Instance.temporaryUI.transform, true);
		tutorialPanelCanvas.transform.localPosition = 3f * Vector3.up;
		LeanTween.moveLocal(tutorialPanelCanvas, Vector3.zero, 0.25f);
		skipTutorialCanvas.SetActive(true);
		skipTutorialCanvas.transform.SetParent(CombatSceneManager.Instance.temporaryUI.transform, true);
		((Component)CombatSceneManager.Instance.microMap).gameObject.SetActive(false);
		PanelIndex = 0;
		cooldownMechanicExplained = false;
		Globals.Hero.SpecialMove.IsEnabled = false;
		int nPanels = panels.Where((TutorialPanel p) => !p.IsVolatilePanel).ToArray().Length;
		int num = 0;
		TutorialPanel[] array = panels;
		foreach (TutorialPanel obj in array)
		{
			if (!obj.IsVolatilePanel)
			{
				num++;
			}
			obj.Initialize(num, nPanels, this);
		}
		Globals.Hero.SetCombatUIActive(value: true);
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.combat;
		CombatManager.Instance.BeginCombat();
	}

	public override void End()
	{
	}

	public override IEnumerator ProcessTurn()
	{
		if (!cooldownMechanicExplained)
		{
			TilesManager.Instance.RechargeCooldownForDeck();
		}
		CurrentPanel.ProcessTurn();
		UpdatePrevAndNextButtons();
		if (Globals.Hero.AgentStats.HP <= 4)
		{
			Globals.Hero.FullHeal();
			EffectsManager.Instance.CreateInGameEffect("HealEffect", ((Component)Globals.Hero.AgentGraphics).transform);
		}
		yield return null;
	}

	public override void Initialize(string name, string id, bool loadRoomStateFromSaveData)
	{
		base.Initialize(name, id, loadRoomStateFromSaveData);
		tutorialPanelCanvas.SetActive(false);
		skipTutorialCanvas.SetActive(false);
		base.Name = "how to play";
		Potion[] heldPotions = PotionsManager.Instance.HeldPotions;
		for (int i = 0; i < heldPotions.Length; i++)
		{
			Object.Destroy((Object)(object)((Component)heldPotions[i]).gameObject);
		}
	}

	public void NextPanel()
	{
		PanelIndex++;
	}

	public void PrevPanel()
	{
		PanelIndex--;
	}

	public void EndTutorial()
	{
		if (!tutorialEnded)
		{
			tutorialEnded = true;
			Globals.SkipTitleScreen = Globals.FirstEverRun;
			MusicManager.Instance.SmoothSetVolume(Globals.Options.musicVolume, 0, 1f);
			SceneLoader.Instance.LoadScene("ResetGameState");
		}
	}

	public void UpdatePrevAndNextButtons()
	{
		Button[] array = prevButtons;
		for (int i = 0; i < array.Length; i++)
		{
			((Selectable)array[i]).interactable = iPanel > 0;
		}
		array = nextButtons;
		for (int i = 0; i < array.Length; i++)
		{
			((Selectable)array[i]).interactable = CurrentPanel.CanGoToNextPanel && iPanel < panels.Length - 1;
		}
	}
}
