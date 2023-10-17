using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class CurrencyCounter : MonoBehaviour
{
	private TextMeshProUGUI valueText;

	protected UpdateValueEvent UpdateValueEvent { get; set; }

	protected abstract void SetUpdateValueEvent();

	private void Start()
	{
		if (Globals.Tutorial)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		valueText = ((Component)this).GetComponentInChildren<TextMeshProUGUI>();
		SetUpdateValueEvent();
		((UnityEvent<int>)UpdateValueEvent).AddListener((UnityAction<int>)UpdateValue);
	}

	private void UpdateValue(int value)
	{
		((TMP_Text)valueText).text = $"{value}";
	}
}
