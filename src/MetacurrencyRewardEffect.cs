using TMPro;
using UnityEngine;

public class MetacurrencyRewardEffect : MonoBehaviour
{
	public TextMeshProUGUI text;

	public void SetValue(int value)
	{
		((TMP_Text)text).text = $"+{value}";
	}
}
