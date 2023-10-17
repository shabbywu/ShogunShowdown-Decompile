using TMPro;
using UnityEngine.Localization.Settings;
using Utils;

public class LanguageMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		SelectNext(1);
	}

	public override void OnLeft()
	{
		SelectNext(-1);
	}

	public override void OnRight()
	{
		SelectNext(1);
	}

	public override void UpdateState()
	{
		((TMP_Text)text).text = "Language: " + LocalizationUtils.ShortLocaleName(LocalizationSettings.SelectedLocale);
	}

	private void SelectNext(int delta)
	{
		int num = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale) + delta;
		while (true)
		{
			if (num >= LocalizationSettings.AvailableLocales.Locales.Count)
			{
				num = 0;
			}
			else if (num < 0)
			{
				num = LocalizationSettings.AvailableLocales.Locales.Count - 1;
			}
			if (LocalizationUtils.IsLocaleCurrentlyAvailable(LocalizationSettings.AvailableLocales.Locales[num]))
			{
				break;
			}
			num += delta;
		}
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[num];
		InteractionEffect();
		UpdateState();
	}
}
