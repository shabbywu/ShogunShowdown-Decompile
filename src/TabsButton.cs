using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabsButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, INavigationTarget
{
	public TabsGroup tabsGroup;

	private Image _image;

	public Image Background
	{
		get
		{
			if ((Object)(object)_image == (Object)null)
			{
				_image = ((Component)this).GetComponent<Image>();
			}
			return _image;
		}
	}

	public Transform Transform => ((Component)this).transform;

	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundEffectsManager.Instance.Play("MenuItemHighlight");
		tabsGroup.OnTabEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tabsGroup.OnTabExit(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		tabsGroup.OnTabSelected(this);
	}

	public void Select()
	{
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		tabsGroup.OnTabEnter(this);
		tabsGroup.OnTabSelected(this);
	}

	public void Deselect()
	{
		tabsGroup.OnTabExit(this);
	}

	public void Submit()
	{
	}
}
