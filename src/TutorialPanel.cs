using TMPro;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI pageNumberText;

	protected TutorialRoom TutorialRoom { get; set; }

	public virtual bool IsVolatilePanel { get; }

	public virtual bool CanGoToNextPanel { get; } = true;


	public virtual void ProcessTurn()
	{
	}

	public void Initialize(int iPanel, int nPanels, TutorialRoom room = null)
	{
		TutorialRoom = room;
		((TMP_Text)pageNumberText).text = (IsVolatilePanel ? "" : $"{iPanel}/{nPanels}");
	}

	public virtual void EnablePanel()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void DisablePanel()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
