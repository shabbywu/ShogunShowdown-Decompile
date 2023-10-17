using UnityEngine;

public class AgentOnCellShopTrigger : MonoBehaviour
{
	private Shop shop;

	private void Awake()
	{
		shop = ((Component)this).GetComponent<Shop>();
	}

	public void AgentEnters()
	{
		shop.Begin();
		EventsManager.Instance.UnlocksShopBegin.Invoke();
	}

	public void AgentExits()
	{
		EventsManager.Instance.ShopEnd.Invoke();
		shop.End();
	}
}
