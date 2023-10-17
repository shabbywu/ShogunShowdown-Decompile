using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InfoBoxActivatorWithFrame : InfoBoxActivator, INavigationTarget
{
	public GameObject graphicsGO;

	private bool hasListeners;

	private Animator animator;

	public Transform Transform => ((Component)this).transform;

	protected override void Awake()
	{
		base.Awake();
		animator = ((Component)this).GetComponent<Animator>();
		InfoBoxManager.Instance?.Register(this);
	}

	private void Start()
	{
		AddListeners();
	}

	private void OnDestroy()
	{
		RemoveListeners();
		InfoBoxManager.Instance?.Deregister(this);
	}

	public void InfoModeEnabled()
	{
		graphicsGO.SetActive(true);
		animator.SetTrigger("Open");
	}

	public void InfoModeDisabled()
	{
		animator.SetTrigger("Close");
		if ((Object)(object)infoBox != (Object)null)
		{
			infoBox.Close();
		}
	}

	public override void OnPointerEnter(PointerEventData pointerEventData)
	{
		base.OnPointerEnter(pointerEventData);
		if (IsEnabled)
		{
			animator.SetBool("Highlight", true);
		}
	}

	public override void OnPointerExit(PointerEventData pointerEventData)
	{
		base.OnPointerExit(pointerEventData);
		if (IsEnabled)
		{
			animator.SetBool("Highlight", false);
		}
	}

	public void ResetListeners()
	{
		RemoveListeners();
		AddListeners();
	}

	private void AddListeners()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.InfoModeEnabled.AddListener(new UnityAction(InfoModeEnabled));
			EventsManager.Instance.InfoModeDisabled.AddListener(new UnityAction(InfoModeDisabled));
			hasListeners = true;
		}
	}

	private void RemoveListeners()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		if (hasListeners)
		{
			EventsManager.Instance.InfoModeEnabled.RemoveListener(new UnityAction(InfoModeEnabled));
			EventsManager.Instance.InfoModeDisabled.RemoveListener(new UnityAction(InfoModeDisabled));
			hasListeners = false;
		}
	}

	public virtual void Select()
	{
		Open();
	}

	public virtual void Deselect()
	{
		Close();
	}

	public void Submit()
	{
	}
}
