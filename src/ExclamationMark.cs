using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
	public Shop shop;

	public bool Show
	{
		set
		{
			((Component)this).GetComponent<Animator>().SetBool("Visible", value);
		}
	}

	private void Start()
	{
		Show = shop.CanBuyAnything();
	}

	public void AgentEnters()
	{
		Show = false;
	}
}
