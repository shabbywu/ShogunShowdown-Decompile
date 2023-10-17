using UnityEngine;
using UnityEngine.EventSystems;

public class MapCloseButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite highlightedSprite;

	[SerializeField]
	private Sprite pressedSprite;

	private void OnEnable()
	{
		spriteRenderer.sprite = normalSprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		spriteRenderer.sprite = highlightedSprite;
		SoundEffectsManager.Instance.Play("MenuItemHighlight");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		spriteRenderer.sprite = normalSprite;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		spriteRenderer.sprite = pressedSprite;
		MapManager.Instance.CloseMap();
	}
}
