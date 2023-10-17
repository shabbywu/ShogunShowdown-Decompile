using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils;

public static class EnumUtils
{
	public static void AssertNoDuplicateValuesInEnum<T>()
	{
		List<int> list = new List<int>();
		foreach (int value in Enum.GetValues(typeof(T)))
		{
			list.Add(value);
		}
	}

	public static List<T> GetAllEnums<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>().ToList();
	}
}
