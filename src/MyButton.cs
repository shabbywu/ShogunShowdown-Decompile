using System.Collections;
using InfoBoxUtils;
using TMPro;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyButton : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	[SerializeField]
	private Sprite disabledSprite;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite highlightedSprite;

	[SerializeField]
	private Sprite pressedSprite;

	[SerializeField]
	private Color normalTextColor;

	[SerializeField]
	private Color disabledTextColor;

	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	[SerializeField]
	private bool awakeClosed;

	[SerializeField]
	private GameObject[] notifyHighlighted;

	[SerializeField]
	private TextMeshProUGUI text;

	private Button button;

	private Image image;

	private Animator animator;

	private string infoBoxText;

	private bool setOpenUponAppearAnimationIsOver = true;

	public bool Interactable
	{
		get
		{
			return ((Selectable)button).interactable;
		}
		set
		{
			((Selectable)button).interactable = value;
			animator.SetBool("Interactable", ((Selectable)button).interactable);
		}
	}

	public string InfoBoxText => infoBoxText;

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public Transform Transform => ((Component)this).transform;

	private void Awake()
	{
		button = ((Component)this).GetComponent<Button>();
		image = ((Component)this).GetComponent<Image>();
		animator = ((Component)this).GetComponent<Animator>();
		if (awakeClosed)
		{
			animator.SetBool("Open", false);
		}
	}

	public void Appear()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		image.sprite = disabledSprite;
		((Graphic)text).color = disabledTextColor;
		setOpenUponAppearAnimationIsOver = true;
		animator.SetTrigger("Appear");
	}

	public void Click()
	{
		((UnityEvent)button.onClick).Invoke();
	}

	public void TriggerOnPointerEnter()
	{
		if ((Object)(object)button != (Object)null)
		{
			((Selectable)button).OnPointerEnter((PointerEventData)null);
		}
	}

	public void TriggerOnPointerExit()
	{
		if ((Object)(object)button != (Object)null)
		{
			((Selectable)button).OnPointerExit((PointerEventData)null);
		}
	}

	public void Disappear()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		animator.ResetTrigger("Appear");
		setOpenUponAppearAnimationIsOver = false;
		image.sprite = disabledSprite;
		((Graphic)text).color = disabledTextColor;
		animator.SetBool("Open", false);
		Interactable = false;
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			infoBoxActivator.Close();
		}
		SendNotifyHighlightedMessages(highlighted: false);
	}

	public void SetText(string value)
	{
		((TMP_Text)text).text = value;
	}

	public void SetInfoBoxText(string value)
	{
		infoBoxText = value;
	}

	public void Highlighted()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("MenuItemHighlight");
		image.sprite = highlightedSprite;
		((Graphic)text).color = normalTextColor;
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			infoBoxActivator.Open();
		}
		SendNotifyHighlightedMessages(highlighted: true);
	}

	public void AppearAnimationOver()
	{
		if (setOpenUponAppearAnimationIsOver)
		{
			animator.SetBool("Open", true);
		}
	}

	public void Normal()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		image.sprite = normalSprite;
		((Graphic)text).color = normalTextColor;
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			infoBoxActivator.Close();
		}
		SendNotifyHighlightedMessages(highlighted: false);
	}

	public void Pressed()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		image.sprite = pressedSprite;
		((Graphic)text).color = normalTextColor;
	}

	public void Disabled()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		image.sprite = disabledSprite;
		((Graphic)text).color = disabledTextColor;
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			infoBoxActivator.Close();
		}
		SendNotifyHighlightedMessages(highlighted: false);
	}

	private void SendNotifyHighlightedMessages(bool highlighted)
	{
		if (notifyHighlighted != null)
		{
			GameObject[] array = notifyHighlighted;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SendMessage("ButtonHighlighted", (object)highlighted);
			}
		}
	}

	public void PressFromCode()
	{
		if (!Interactable)
		{
			SoundEffectsManager.Instance.Play("CannotPerformAction");
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(PressedFromCodeCoroutine());
		}
	}

	private IEnumerator PressedFromCodeCoroutine()
	{
		Pressed();
		Click();
		yield return (object)new WaitForSeconds(0.2f);
		if (Interactable)
		{
			Normal();
		}
		else
		{
			Disabled();
		}
	}

	public void Select()
	{
		TriggerOnPointerEnter();
	}

	public void Deselect()
	{
		TriggerOnPointerExit();
	}

	public void Submit()
	{
		Click();
	}
}
