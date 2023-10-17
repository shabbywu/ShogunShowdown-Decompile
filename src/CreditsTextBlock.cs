using TMPro;
using UnityEngine;
using Utils;

public class CreditsTextBlock : MonoBehaviour
{
	[SerializeField]
	private string headerLocTableKey;

	[SerializeField]
	private string textLocTableKey;

	[SerializeField]
	private TextMeshProUGUI header;

	[SerializeField]
	private TextMeshProUGUI text;

	private void Start()
	{
		((TMP_Text)header).text = ((headerLocTableKey == "") ? "" : TextUitls.ReplaceTags(LocalizationUtils.LocalizedString("Credits", headerLocTableKey)));
		((TMP_Text)text).text = ((textLocTableKey == "") ? "" : TextUitls.ReplaceTags(LocalizationUtils.LocalizedString("Credits", textLocTableKey)));
	}

	public void Show()
	{
		((Component)this).GetComponent<CanvasGroup>().alpha = 1f;
	}

	public void Hide()
	{
		((Component)this).GetComponent<CanvasGroup>().alpha = 0f;
	}
}
