using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnlocksID;

public class ControlsSummary : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup canvasGroup;

	[SerializeField]
	private float visibleAlpha;

	[SerializeField]
	private GameObject swapSummary;

	[SerializeField]
	private GameObject pushSummary;

	[SerializeField]
	private GameObject phaseSummary;

	private bool isEnabled;

	private bool hasVisibilityEverBeenToggoled;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(UponBeginningOfCombat));
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(UponEndOfCombat));
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)UponGameOver);
		EventsManager.Instance.MapOpened.AddListener(new UnityAction(UponMapOpen));
		EventsManager.Instance.NewHeroSelected.AddListener(new UnityAction(UponNewHeroSelected));
		InputManager.Instance?.RegisterActionToPlayerInput("Combat/ControlSummaryToggle", InputManager.InputActionEnum.performed, ToggleVisibility);
		UpdateSpecialMovePanelBasedOnHero(Globals.Hero);
		isEnabled = false;
		canvasGroup.alpha = 0f;
	}

	private void OnDestroy()
	{
		InputManager.Instance?.DeregisterActionToPlayerInput("Combat/ControlSummaryToggle", InputManager.InputActionEnum.performed, ToggleVisibility);
	}

	private void UponBeginningOfCombat()
	{
		if (!Globals.Tutorial && !hasVisibilityEverBeenToggoled && !UnlocksManager.Instance.Unlocked(UnlockID.q_first_island_cleared))
		{
			Show(value: true);
		}
	}

	private void UponEndOfCombat()
	{
		Show(value: false);
	}

	private void UponGameOver(bool victory)
	{
		Show(value: false);
	}

	private void UponMapOpen()
	{
		Show(value: false);
	}

	private void UponNewHeroSelected()
	{
		UpdateSpecialMovePanelBasedOnHero(Globals.Hero);
	}

	private void UpdateSpecialMovePanelBasedOnHero(Hero hero)
	{
		swapSummary.SetActive(hero is WandererHero && hero.Unlocked);
		pushSummary.SetActive(hero is RoninHero && hero.Unlocked);
		phaseSummary.SetActive(hero is ShadowHero && hero.Unlocked);
	}

	private void Show(bool value)
	{
		isEnabled = value;
		if (value)
		{
			LeanTween.alphaCanvas(canvasGroup, visibleAlpha, 0.1f);
		}
		else
		{
			LeanTween.alphaCanvas(canvasGroup, 0f, 0.1f);
		}
	}

	public void ToggleVisibility(CallbackContext context)
	{
		ToggleVisibility();
	}

	public void ToggleVisibility()
	{
		Show(!isEnabled);
		hasVisibilityEverBeenToggoled = true;
	}
}
