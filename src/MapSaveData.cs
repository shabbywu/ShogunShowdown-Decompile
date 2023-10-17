using System;
using System.Collections.Generic;

[Serializable]
public class MapSaveData
{
	public string currentLocationName;

	public string currentMapLocationID;

	public List<string> uncoveredMapLocationIDs;

	public List<string> shopComponent;

	public MapSaveData()
	{
		uncoveredMapLocationIDs = new List<string>();
		shopComponent = new List<string>();
	}
}
