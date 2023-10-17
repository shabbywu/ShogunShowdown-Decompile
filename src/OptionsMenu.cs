using UnityEngine;
using UnityEngine.Events;

public class OptionsMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject menuContainer;

	[SerializeField]
	private MenuPage[] menuPages;

	[SerializeField]
	private MenuPage initialMenuPage;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		EventsManager.Instance.OpenOptionsMenu.AddListener(new UnityAction(OpenOptionsMenu));
		EventsManager.Instance.CloseOptionsMenu.AddListener(new UnityAction(CloseOptionsMenu));
		DisableAllMenuPages();
	}

	private void OpenOptionsMenu()
	{
		menuContainer.SetActive(true);
		initialMenuPage.Enable();
	}

	private void CloseOptionsMenu()
	{
		SaveDataManager.Instance.StoreOptions();
		DisableAllMenuPages();
		menuContainer.SetActive(false);
	}

	private void DisableAllMenuPages()
	{
		MenuPage[] array = menuPages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disable();
		}
	}
}
