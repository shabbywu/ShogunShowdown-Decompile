using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapSprites : MonoBehaviour
{
	private static string resourcesPath = "Graphics/Map";

	private static string locationIconsFileName = "MapLocations";

	private static string connectionsHighlightedFileName = "MapConnectionsHighlighted";

	private static string connectionsNotHighlightedFileName = "MapConnectionsNotHighlighted";

	public Dictionary<string, Sprite> Locations { get; private set; }

	public Dictionary<(string, string), Sprite> ConnectionsHighlighted { get; private set; }

	public Dictionary<(string, string), Sprite> ConnectionsNotHighlighted { get; private set; }

	public void LoadSprites()
	{
		Locations = new Dictionary<string, Sprite>();
		ConnectionsHighlighted = new Dictionary<(string, string), Sprite>();
		ConnectionsNotHighlighted = new Dictionary<(string, string), Sprite>();
		Sprite[] array = Resources.LoadAll<Sprite>(Path.Combine(resourcesPath, locationIconsFileName));
		foreach (Sprite val in array)
		{
			Locations.Add(((Object)val).name, val);
		}
		LoadConnections(connectionsHighlightedFileName, ConnectionsHighlighted);
		LoadConnections(connectionsNotHighlightedFileName, ConnectionsNotHighlighted);
	}

	private void LoadConnections(string filename, Dictionary<(string, string), Sprite> dict)
	{
		new Dictionary<string, Sprite>();
		Sprite[] array = Resources.LoadAll<Sprite>(Path.Combine(resourcesPath, filename));
		foreach (Sprite val in array)
		{
			string[] array2 = ((Object)val).name.Split('_');
			string item = array2[^2];
			string item2 = array2[^1];
			dict.Add((item, item2), val);
		}
	}
}
