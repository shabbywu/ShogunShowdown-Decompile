using System;

namespace Utils;

internal static class MyTime
{
	public static string ToMinAndSecFormat(int time)
	{
		if (time < 0)
		{
			return "--:--";
		}
		int hours = TimeSpan.FromSeconds(time).Hours;
		int minutes = TimeSpan.FromSeconds(time).Minutes;
		int seconds = TimeSpan.FromSeconds(time).Seconds;
		string obj = ((hours > 0) ? $"{hours}:" : "");
		string text = ((hours > 0 && minutes < 10) ? $"0{minutes}:" : $"{minutes}:");
		string text2 = ((seconds < 10) ? $"0{seconds}" : $"{seconds}");
		return obj + text + text2;
	}
}
