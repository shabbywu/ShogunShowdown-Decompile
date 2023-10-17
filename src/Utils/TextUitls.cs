using System.Collections.Generic;

namespace Utils;

public static class TextUitls
{
	public static Dictionary<string, string> TagsDict = new Dictionary<string, string>
	{
		["[enemy_color]"] = "<color=#" + Colors.orangeHex + ">",
		["[bright_yellow]"] = "<color=#" + Colors.birghtYellowHex + ">",
		["[bright_red]"] = "<color=#" + Colors.brigthRedHex + ">",
		["[orange]"] = "<color=#" + Colors.orangeHex + ">",
		["[header_color]"] = "<color=#" + Colors.lightCobaltHex + ">",
		["[reward_color_good]"] = "<color=#" + Colors.darkGrayHex + ">",
		["[reward_color_bad]"] = "<color=#" + Colors.darkRedHex + ">",
		["[bad_color]"] = "<color=#" + Colors.orangeHex + ">",
		["[lightgray_color]"] = "<color=#" + Colors.lightGrayHex + ">",
		["[low_priority_color]"] = "<color=#" + Colors.grayHex + ">",
		["[end_color]"] = "</color>",
		["[vspace]"] = "<line-height=50%>\n<line-height=100%>",
		["[begin_header]"] = "<size=+4>- ",
		["[end_header]"] = " -<size=+0>",
		["[begin_large]"] = "<size=+8>",
		["[end_large]"] = "<size=+0>",
		["[begin_small]"] = "<size=-8>",
		["[end_small]"] = "<size=+0>"
	};

	public static string SingleLineHeader(string s)
	{
		int num = ((s.Length < 17) ? 4 : ((s.Length < 20) ? 3 : ((s.Length < 24) ? 2 : ((s.Length < 28) ? 1 : 0))));
		return $"<size=+{num}>- {s} -<size=+0>";
	}

	public static string ReplaceTags(string s)
	{
		foreach (KeyValuePair<string, string> item in TagsDict)
		{
			s = s.Replace(item.Key, item.Value);
		}
		return s;
	}
}
