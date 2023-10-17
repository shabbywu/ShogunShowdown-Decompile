using InfoBoxUtils;
using TMPro;
using UINavigation;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class DaySelection : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	[SerializeField]
	private TextMeshProUGUI dayText;

	[SerializeField]
	private MyButton prevButton;

	[SerializeField]
	private MyButton nextButton;

	[SerializeField]
	private GenericSelectorUI genericSelectorUI;

	private bool open;

	public bool Open
	{
		set
		{
			((Component)this).gameObject.SetActive(value);
			if (!open && value && Globals.Day > 1)
			{
				infoBoxActivator.Open();
			}
			else if (open && !value)
			{
				infoBoxActivator.Close();
			}
			open = value;
		}
	}

	public string InfoBoxText
	{
		get
		{
			string text = "[bad_color]<align=\"left\">";
			if (Globals.Day >= 2)
			{
				text = text + "- <indent=1em>" + Ascension.DescriptionOfBuffActivatedOnDay(2) + "</indent>";
			}
			if (Globals.Day >= 3)
			{
				text = text + "\n- <indent=1em>" + Ascension.DescriptionOfBuffActivatedOnDay(3) + "</indent>";
			}
			if (Globals.Day >= 4)
			{
				text = text + "\n- <indent=1em>" + Ascension.DescriptionOfBuffActivatedOnDay(4) + "</indent>";
			}
			if (Globals.Day >= 5)
			{
				text = text + "\n- <indent=1em>" + Ascension.DescriptionOfBuffActivatedOnDay(5) + "</indent>";
			}
			text += "[end_color]</align>";
			return TextUitls.ReplaceTags(text);
		}
	}

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.medium;

	public Transform Transform => ((Component)this).transform;

	public void CloseInfoBox()
	{
		infoBoxActivator.Close();
	}

	public void SetInitiallySelectedDayForHero(Hero hero)
	{
		int day = Mathf.Clamp(hero.CharacterSaveData.bestDay + 1, 1, Globals.CurrentlyImplementedMaxDay);
		UpdateDay(day);
	}

	public void UpdateDay(int day)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Globals.Day = day;
		((TMP_Text)dayText).text = string.Format(LocalizationUtils.LocalizedString("Terms", "Day"), day);
		EventsManager.Instance.NewDifficultySelected.Invoke();
		Globals.Hero.InitializeHP();
		bool flag = day <= Globals.Hero.CharacterSaveData.bestDay;
		((Graphic)dayText).color = (flag ? Colors.FromHex(Colors.birghtYellowHex) : Color.white);
		prevButton.Interactable = day > 1;
		nextButton.Interactable = flag && day < Globals.CurrentlyImplementedMaxDay;
		if (open)
		{
			if (day > 1)
			{
				infoBoxActivator.Open();
			}
			else
			{
				infoBoxActivator.Close();
			}
		}
	}

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
