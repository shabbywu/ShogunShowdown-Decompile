using SkillEnums;
using UnityEngine;
using Utils;

public abstract class Item : MonoBehaviour
{
	public Sprite defaultSprite;

	private int _level = 1;

	public int Level
	{
		get
		{
			return _level;
		}
		set
		{
			if (value > MaxLevel)
			{
				Debug.LogWarning((object)$"Trying to set level {value} for '{Name}' item, but it's larger than max level of {MaxLevel}.");
			}
			else
			{
				_level = value;
			}
		}
	}

	public bool CurrentlyHeld { get; set; }

	public Sprite Sprite => defaultSprite;

	public abstract SkillEnum SkillEnum { get; }

	public abstract int MaxLevel { get; protected set; }

	public abstract string LocalizationTableKey { get; }

	public string Name => LocalizationUtils.LocalizedString("Skills", LocalizationTableKey + "_Name");

	public string Description => ProcessDescription(LocalizationUtils.LocalizedString("Skills", LocalizationTableKey + "_Description"));

	protected virtual string ProcessDescription(string description)
	{
		return description;
	}

	public virtual void LevelUp()
	{
		Level++;
	}

	public virtual void PickUp()
	{
		CurrentlyHeld = true;
	}

	public virtual void Remove()
	{
		CurrentlyHeld = false;
	}

	public string GetInfoBoxText()
	{
		string text = "";
		text = text + "[header_color]" + TextUitls.SingleLineHeader(Name) + "[end_color]\n";
		text = ((Level <= 1) ? (text + "[vspace]") : (text + $"level {Level}\n[vspace]"));
		text += Description;
		return TextUitls.ReplaceTags(text);
	}
}
