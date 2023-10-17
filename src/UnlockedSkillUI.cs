using InfoBoxUtils;
using UINavigation;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedSkillUI : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	public Image image;

	public string InfoBoxText => Description;

	public string Description { get; set; }

	public bool Unlocked { get; set; }

	public Sprite Sprite
	{
		set
		{
			image.sprite = value;
		}
	}

	public bool InfoBoxEnabled => Unlocked;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public Transform Transform => ((Component)this).transform;

	public virtual void Select()
	{
		infoBoxActivator.Open();
	}

	public virtual void Deselect()
	{
		infoBoxActivator.Close();
	}

	public virtual void Submit()
	{
	}
}
