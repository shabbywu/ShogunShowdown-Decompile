using InfoBoxUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

public class InfoBox : MonoBehaviour
{
	public enum PositioningEnum
	{
		center,
		left,
		right,
		above,
		below
	}

	[Header("Public stuff:")]
	public PositioningEnum positioning;

	public Transform targetTransform;

	[Header("Private stuff:")]
	public TextMeshProUGUI textTMPro;

	public RectTransform container;

	public Image leftArrow;

	public Image rightArrow;

	public Image belowArrow;

	public Image aboveArrow;

	public static string titleColor = Colors.midCobaltHex;

	private readonly float deltaArrow = 0.0625f;

	private Animator animator;

	public bool IsOpen { get; private set; }

	public static string StandardInfoBoxFormatting(string header, string comment)
	{
		return TextUitls.ReplaceTags("[header_color]" + TextUitls.SingleLineHeader(header.ToUpper()) + "[end_color]\n[vspace]" + comment);
	}

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
		SetPosition(positioning, targetTransform);
	}

	private void Start()
	{
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.GameOver.RemoveListener((UnityAction<bool>)GameOver);
		}
	}

	public void SetPosition(PositioningEnum p, Transform t)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)t != (Object)null)
		{
			((Component)this).transform.SetParent(t);
		}
		positioning = p;
		switch (positioning)
		{
		case PositioningEnum.left:
			container.pivot = new Vector2(1f, 0.5f);
			((Component)this).transform.localPosition = Vector3.left * deltaArrow;
			break;
		case PositioningEnum.right:
			container.pivot = new Vector2(0f, 0.5f);
			((Component)this).transform.localPosition = Vector3.right * deltaArrow;
			break;
		case PositioningEnum.below:
			container.pivot = new Vector2(0.5f, 1f);
			((Component)this).transform.localPosition = Vector3.down * deltaArrow;
			break;
		case PositioningEnum.above:
			container.pivot = new Vector2(0.5f, 0f);
			((Component)this).transform.localPosition = Vector3.up * deltaArrow;
			break;
		case PositioningEnum.center:
			container.pivot = new Vector2(0.5f, 0.5f);
			((Component)this).transform.localPosition = Vector3.zero;
			break;
		}
		((Component)leftArrow).gameObject.SetActive(positioning == PositioningEnum.left);
		((Component)rightArrow).gameObject.SetActive(positioning == PositioningEnum.right);
		((Component)aboveArrow).gameObject.SetActive(positioning == PositioningEnum.above);
		((Component)belowArrow).gameObject.SetActive(positioning == PositioningEnum.below);
	}

	public void SetText(string text)
	{
		((TMP_Text)textTMPro).text = text;
	}

	public void SetBoxWidth(BoxWidth boxWidth)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		HorizontalLayoutGroup componentInChildren = ((Component)this).GetComponentInChildren<HorizontalLayoutGroup>();
		((HorizontalOrVerticalLayoutGroup)componentInChildren).childControlWidth = boxWidth == BoxWidth.auto;
		((HorizontalOrVerticalLayoutGroup)componentInChildren).childForceExpandWidth = boxWidth == BoxWidth.auto;
		switch (boxWidth)
		{
		case BoxWidth.medium:
			((TMP_Text)textTMPro).rectTransform.sizeDelta = new Vector2(138f, 100f);
			break;
		case BoxWidth.small:
			((TMP_Text)textTMPro).rectTransform.sizeDelta = new Vector2(96f, 100f);
			break;
		}
	}

	public void AdjustIfOutOfScreen()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		if (positioning == PositioningEnum.above || positioning == PositioningEnum.below)
		{
			float x = ((Component)this).transform.localScale.x;
			Rect rect = ((TMP_Text)textTMPro).rectTransform.rect;
			float num = x * ((Rect)(ref rect)).width / 2f;
			float num2 = ((Component)this).transform.position.x - num;
			float num3 = ((Component)this).transform.position.x + num;
			Vector3 val = Camera.main.WorldToViewportPoint(new Vector3(num3, ((Component)this).transform.position.y, ((Component)this).transform.position.z));
			Vector3 val2 = Camera.main.WorldToViewportPoint(new Vector3(num2, ((Component)this).transform.position.y, ((Component)this).transform.position.z));
			if (val.x > 1f)
			{
				container.pivot = new Vector2(0.75f, container.pivot.y);
				Vector3 val3 = Vector3.right * num / (2f * ((Component)this).transform.localScale.x);
				RectTransform rectTransform = ((Graphic)aboveArrow).rectTransform;
				((Transform)rectTransform).localPosition = ((Transform)rectTransform).localPosition + val3;
				RectTransform rectTransform2 = ((Graphic)belowArrow).rectTransform;
				((Transform)rectTransform2).localPosition = ((Transform)rectTransform2).localPosition + val3;
			}
			else if (val2.x < 0f)
			{
				container.pivot = new Vector2(0.25f, container.pivot.y);
				Vector3 val4 = Vector3.left * num / (2f * ((Component)this).transform.localScale.x);
				RectTransform rectTransform3 = ((Graphic)aboveArrow).rectTransform;
				((Transform)rectTransform3).localPosition = ((Transform)rectTransform3).localPosition + val4;
				RectTransform rectTransform4 = ((Graphic)belowArrow).rectTransform;
				((Transform)rectTransform4).localPosition = ((Transform)rectTransform4).localPosition + val4;
			}
		}
	}

	public void SetMaxWidth(int value)
	{
		((Component)this).GetComponentInChildren<LayoutMaxSize>().maxWidth = value;
	}

	public void Open()
	{
		((Component)container).gameObject.SetActive(true);
		animator.SetTrigger("Open");
		SoundEffectsManager.Instance.Play("InfoBoxOpen");
		IsOpen = true;
	}

	public void Close()
	{
		animator.SetTrigger("Close");
		IsOpen = false;
	}

	private void GameOver(bool win)
	{
		Close();
	}
}
