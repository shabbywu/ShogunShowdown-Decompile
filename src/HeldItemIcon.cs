using InfoBoxUtils;
using TMPro;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeldItemIcon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IInfoBoxable, INavigationTarget
{
	public TextMeshProUGUI levelCounter;

	public Image image;

	private Color semitransparent = new Color(1f, 1f, 1f, 0.5f);

	private Color opaque = new Color(1f, 1f, 1f, 1f);

	private InfoBoxActivator infoBoxActivator;

	public string InfoBoxText { get; private set; }

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.medium;

	public Transform Transform => ((Component)this).transform;

	public void UpdateInfo(Item item)
	{
		image.sprite = item.Sprite;
		((Behaviour)levelCounter).enabled = item.Level > 1;
		((TMP_Text)levelCounter).text = $"{item.Level}";
		InfoBoxText = item.GetInfoBoxText();
	}

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)image).color = semitransparent;
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Select();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Deselect();
	}

	public void Select()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)image).color = opaque;
		infoBoxActivator.Open();
	}

	public void Deselect()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)image).color = semitransparent;
		infoBoxActivator.Close();
	}

	public void Submit()
	{
	}
}
