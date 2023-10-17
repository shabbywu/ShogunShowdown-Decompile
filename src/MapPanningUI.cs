using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanningUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public SpriteRenderer spriteRenderer;

	public Sprite nonHighlightedSprite;

	public Sprite highlightedSprite;

	public Vector3 panVector;

	private void OnEnable()
	{
		spriteRenderer.sprite = nonHighlightedSprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		MapManager.Instance.mapScreen.PanShift = panVector;
		spriteRenderer.sprite = highlightedSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		MapManager.Instance.mapScreen.PanShift = Vector3.zero;
		spriteRenderer.sprite = nonHighlightedSprite;
	}
}
