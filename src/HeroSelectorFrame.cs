using UINavigation;
using UnityEngine;

public class HeroSelectorFrame : MonoBehaviour, INavigationTarget
{
	[SerializeField]
	private GenericSelectorUI genericSelectorUI;

	public Transform Transform => ((Component)this).transform;

	public virtual void Select()
	{
		genericSelectorUI.Enable();
	}

	public virtual void Deselect()
	{
		genericSelectorUI.Disable();
	}

	public virtual void Submit()
	{
	}
}
