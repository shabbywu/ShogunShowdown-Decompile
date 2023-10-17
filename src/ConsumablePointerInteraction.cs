using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumablePointerInteraction : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private Potion potion;

	private void Awake()
	{
		potion = ((Component)this).GetComponentInParent<Potion>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		potion.Select();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		potion.Deselect();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		potion.Submit();
	}
}
