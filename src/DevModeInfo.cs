using TMPro;
using UnityEngine;

public class DevModeInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI quickModeTMPro;

	[SerializeField]
	private TextMeshProUGUI autoSaveTMPro;

	[SerializeField]
	private TextMeshProUGUI shortLocTMPro;

	private void Awake()
	{
		((Component)this).gameObject.SetActive(Globals.Developer);
		UpdateInfo();
	}

	public void UpdateInfo()
	{
		((TMP_Text)quickModeTMPro).text = $"Q quick: {Globals.Quick}";
		((TMP_Text)autoSaveTMPro).text = $"O autosave: {Globals.AutoSave}";
		((TMP_Text)shortLocTMPro).text = $"L short loc: {Globals.ShortLocations}";
	}
}
