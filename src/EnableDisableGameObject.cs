using UnityEngine;

public class EnableDisableGameObject : MonoBehaviour
{
	public void Enable()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Disable()
	{
		((Component)this).gameObject.SetActive(false);
	}

	public void FlipEnableState()
	{
		((Component)this).gameObject.SetActive(!((Component)this).gameObject.activeSelf);
	}
}
