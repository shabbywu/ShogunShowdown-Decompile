using UnityEngine;
using UnityEngine.Events;

public class InfoModeTogglePrompt : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup canvasGroup;

	private bool CanDisplay
	{
		get
		{
			if (Globals.Options.controlScheme == Options.ControlScheme.Gamepad)
			{
				return CombatSceneManager.Instance.CanEnableInfoMode;
			}
			return false;
		}
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(PotentiallyShow));
		EventsManager.Instance.RewardRoomBegin.AddListener(new UnityAction(PotentiallyShow));
		EventsManager.Instance.ShopBegin.AddListener(new UnityAction(PotentiallyShow));
		EventsManager.Instance.MapDeactivated.AddListener(new UnityAction(PotentiallyShow));
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(Hide));
		EventsManager.Instance.RewardRoomEnd.AddListener(new UnityAction(Hide));
		EventsManager.Instance.ShopEnd.AddListener(new UnityAction(Hide));
		EventsManager.Instance.MapActivated.AddListener(new UnityAction(Hide));
		EventsManager.Instance.InfoModeEnabled.AddListener(new UnityAction(InfoModeEnabled));
		EventsManager.Instance.InfoModeDisabled.AddListener(new UnityAction(InfoModeDisabled));
		if (CanDisplay)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void InfoModeEnabled()
	{
		canvasGroup.alpha = 1f;
	}

	private void InfoModeDisabled()
	{
		canvasGroup.alpha = 0.4f;
	}

	private void PotentiallyShow()
	{
		if (CanDisplay)
		{
			Show();
		}
	}

	private void Show()
	{
		((Component)canvasGroup).gameObject.SetActive(true);
	}

	private void Hide()
	{
		((Component)canvasGroup).gameObject.SetActive(false);
	}
}
