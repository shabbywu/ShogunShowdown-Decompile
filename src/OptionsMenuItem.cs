using System;
using InfoBoxUtils;
using TMPro;
using UINavigation;
using UnityEngine;

public abstract class OptionsMenuItem : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	protected TextMeshProUGUI text;

	private GameObject leftHighlight;

	private GameObject rightHighlight;

	private GameObject textGO;

	private bool initialized;

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.small;

	public virtual string InfoBoxText
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public Transform Transform => ((Component)this).transform;

	public abstract void OnSubmit();

	public abstract void UpdateState();

	public abstract void OnRight();

	public abstract void OnLeft();

	protected virtual void Awake()
	{
		leftHighlight = ((Component)((Component)this).transform.Find("Button/Text/LeftHighlight")).gameObject;
		rightHighlight = ((Component)((Component)this).transform.Find("Button/Text/RightHighlight")).gameObject;
		textGO = ((Component)((Component)this).transform.Find("Button/Text")).gameObject;
		text = ((Component)this).GetComponentInChildren<TextMeshProUGUI>();
		initialized = true;
		UpdateGraphicsHighlightState(value: false, 0f);
	}

	private void OnEnable()
	{
		UpdateState();
	}

	private void OnDisable()
	{
		UpdateGraphicsHighlightState(value: false);
	}

	private void Start()
	{
		UpdateState();
	}

	public void OnFocus()
	{
		if (initialized)
		{
			SoundEffectsManager.Instance.Play("MenuItemHighlight");
			UpdateGraphicsHighlightState(value: true);
		}
	}

	public void OnLoseFocus()
	{
		if (initialized)
		{
			UpdateGraphicsHighlightState(value: false);
		}
	}

	protected void InteractionEffect()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		LeanTween.moveX(textGO, textGO.transform.position.x - 0.05f, 0.2f).setEaseShake().setIgnoreTimeScale(true);
	}

	private void UpdateGraphicsHighlightState(bool value, float animationTime = 0.1f)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (value)
		{
			LeanTween.alphaCanvas(((Component)this).GetComponent<CanvasGroup>(), 1f, animationTime).setIgnoreTimeScale(true);
			LeanTween.scaleX(leftHighlight, 1f, animationTime).setEase((LeanTweenType)4).setIgnoreTimeScale(true);
			LeanTween.scaleX(rightHighlight, 1f, animationTime).setEase((LeanTweenType)4).setIgnoreTimeScale(true);
			LeanTween.scale(textGO, 1.2f * Vector3.one, 0.75f * animationTime).setIgnoreTimeScale(true);
		}
		else
		{
			LeanTween.alphaCanvas(((Component)this).GetComponent<CanvasGroup>(), 0.75f, animationTime).setIgnoreTimeScale(true);
			LeanTween.scaleX(leftHighlight, 0f, animationTime).setEase((LeanTweenType)4).setIgnoreTimeScale(true);
			LeanTween.scaleX(rightHighlight, 0f, animationTime).setEase((LeanTweenType)4).setIgnoreTimeScale(true);
			LeanTween.scale(textGO, Vector3.one, 0.75f * animationTime).setIgnoreTimeScale(true);
		}
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			if (value)
			{
				infoBoxActivator.Open();
			}
			else
			{
				infoBoxActivator.Close();
			}
		}
	}

	public void Select()
	{
		OnFocus();
	}

	public void Deselect()
	{
		OnLoseFocus();
	}

	public void Submit()
	{
		OnSubmit();
	}
}
