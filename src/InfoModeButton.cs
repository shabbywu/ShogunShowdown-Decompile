using InfoBoxUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InfoModeButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IInfoBoxable
{
	private InfoBoxActivator infoBoxActivator;

	private Animator animator;

	private bool buttonEnabled;

	private string infoBoxText;

	public string InfoBoxText => infoBoxText;

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	private void Awake()
	{
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
		animator = ((Component)this).GetComponent<Animator>();
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
		EventsManager.Instance.InfoModeEnabled.AddListener(new UnityAction(InfoModeEnabledListener));
		EventsManager.Instance.InfoModeDisabled.AddListener(new UnityAction(InfoModeDisabledListener));
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(BeginningOfCombatListener));
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(EndOfCombatListener));
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (buttonEnabled)
		{
			infoBoxActivator.Open();
			animator.SetBool("Highlighted", true);
			SoundEffectsManager.Instance.Play("MenuItemHighlight");
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (buttonEnabled)
		{
			infoBoxActivator.Close();
			animator.SetBool("Highlighted", false);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (buttonEnabled && (int)eventData.button == 0)
		{
			if (Globals.FullInfoMode)
			{
				CombatSceneManager.Instance.DisableInfoMode();
			}
			else
			{
				CombatSceneManager.Instance.EnableInfoMode();
			}
			SoundEffectsManager.Instance.Play("MenuItemSubmit");
		}
	}

	private void InfoModeEnabledListener()
	{
		animator.SetBool("Enabled", true);
	}

	private void InfoModeDisabledListener()
	{
		animator.SetBool("Enabled", false);
	}

	private void BeginningOfCombatListener()
	{
		buttonEnabled = true;
		animator.SetBool("ButtonEnabled", buttonEnabled);
	}

	private void EndOfCombatListener()
	{
		buttonEnabled = false;
		animator.SetBool("ButtonEnabled", buttonEnabled);
	}

	public void SetInfoBoxText(string value)
	{
		infoBoxText = value;
	}
}
