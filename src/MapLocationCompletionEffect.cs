using TMPro;
using UnityEngine;
using Utils;

public class MapLocationCompletionEffect : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI text;

	private void Start()
	{
		int num = 0;
		foreach (RoomMetrics roomMetric in MetricsManager.Instance.runMetrics.roomMetrics)
		{
			if (roomMetric.sector == Progression.Instance.CurrentLocation.sector)
			{
				num = Mathf.Max(num, roomMetric.damageTaken);
			}
		}
		string key = ((num == 0) ? "LocationObliterated" : "LocationCleared");
		((TMP_Text)text).text = LocalizationUtils.LocalizedString("Terms", key);
	}
}
