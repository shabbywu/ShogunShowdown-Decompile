using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Utils;

public static class LocalizationUtils
{
	public static string LocalizedString(string table, string key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return new LocalizedString
		{
			TableReference = TableReference.op_Implicit(table),
			TableEntryReference = TableEntryReference.op_Implicit(key)
		}.GetLocalizedString();
	}

	public static bool IsLocaleCurrentlyAvailable(Locale locale)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		List<string> obj = new List<string>
		{
			"en", "fr", "de", "es", "pt", "ko", "zh-Hans", "zh-Hant", "ja", "pl",
			"ru"
		};
		LocaleIdentifier identifier = locale.Identifier;
		return obj.Contains(((LocaleIdentifier)(ref identifier)).Code);
	}

	public static string ShortLocaleName(Locale locale)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		LocaleIdentifier identifier = locale.Identifier;
		return ((LocaleIdentifier)(ref identifier)).Code switch
		{
			"en" => "english", 
			"it" => "italian", 
			"fr" => "french", 
			"de" => "german", 
			"es" => "spanish", 
			"pt" => "portuguese", 
			"ja" => "japanese", 
			"zh-Hans" => "chinese (simplified)", 
			"zh-Hant" => "chinese (traditional)", 
			"ko" => "korean", 
			"pl" => "polish", 
			"ru" => "russian", 
			_ => "not defined in ShortLocaleName", 
		};
	}
}
