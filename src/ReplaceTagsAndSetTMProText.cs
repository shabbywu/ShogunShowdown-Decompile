using TMPro;
using UnityEngine;
using Utils;

public class ReplaceTagsAndSetTMProText : MonoBehaviour
{
	public void SetText(string value)
	{
		((TMP_Text)((Component)this).GetComponent<TextMeshProUGUI>()).text = TextUitls.ReplaceTags(value);
	}
}
