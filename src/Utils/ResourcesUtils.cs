using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils;

internal static class ResourcesUtils
{
	public static List<GameObject> LoadGameObjects(string resoucesPath)
	{
		return Array.ConvertAll(Resources.LoadAll(resoucesPath), (Converter<Object, GameObject>)((Object item) => (GameObject)item)).ToList();
	}
}
