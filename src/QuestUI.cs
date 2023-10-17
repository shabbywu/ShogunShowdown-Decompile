using InfoBoxUtils;
using TMPro;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IInfoBoxable, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, INavigationTarget
{
	public Image frameImage;

	public Image questSymbolImage;

	public Sprite frameSpriteIdle;

	public Sprite frameSpriteHighlighted;

	public InfoBoxActivator infoBoxActivator;

	private TextMeshProUGUI text;

	private Quest quest;

	private bool _highlighted;

	private bool Highlighted
	{
		get
		{
			return _highlighted;
		}
		set
		{
			_highlighted = value;
			if (_highlighted)
			{
				frameImage.sprite = frameSpriteHighlighted;
				infoBoxActivator.Open();
			}
			else
			{
				frameImage.sprite = frameSpriteIdle;
				infoBoxActivator.Close();
			}
		}
	}

	public string InfoBoxText
	{
		get
		{
			if (quest.Unveiled)
			{
				return quest.Description;
			}
			return "???";
		}
	}

	int IInfoBoxable.MaxWidth { get; } = 85;


	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public Transform Transform => ((Component)this).transform;

	public void Initialize(Quest quest)
	{
		this.quest = quest;
		TextMeshProUGUI componentInChildren = ((Component)this).GetComponentInChildren<TextMeshProUGUI>();
		if (quest.Unveiled)
		{
			((TMP_Text)componentInChildren).text = quest.Name;
		}
		else
		{
			((TMP_Text)componentInChildren).text = "???";
		}
		Highlighted = false;
		if (quest.IsCompleted)
		{
			questSymbolImage.sprite = quest.symbolCompleted;
		}
		else
		{
			questSymbolImage.sprite = quest.symbolNotCompleted;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Highlighted = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Highlighted = false;
	}

	public virtual void Select()
	{
		Highlighted = true;
	}

	public virtual void Deselect()
	{
		Highlighted = false;
	}

	public virtual void Submit()
	{
	}
}
