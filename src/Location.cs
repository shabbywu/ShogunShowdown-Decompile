using System.Collections.Generic;
using ProgressionEnums;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "NewLocation", menuName = "SO/Location", order = 1)]
public class Location : ScriptableObject
{
	public string technicalName;

	public bool isCamp;

	public bool isLastCombatLocationInIsland;

	public int sector;

	public IslandEnum island;

	public string waveMusicTrackName;

	public Sprite mapIconCleared;

	public Sprite mapIconHighlighted;

	public Sprite mapIconReachable;

	public Sprite mapIconUnreachable;

	public SubLocation[] subLocations;

	public virtual string Name => LocalizationUtils.LocalizedString("Locations", technicalName);

	public virtual int NRooms => subLocations.Length;

	public virtual Room[] Rooms
	{
		get
		{
			List<Room> list = new List<Room>();
			SubLocation[] array = subLocations;
			foreach (SubLocation subLocation in array)
			{
				list.Add(subLocation.room);
			}
			return list.ToArray();
		}
	}

	public virtual string RoomName(int iRoom)
	{
		return Name.Replace("\n", " ") + subLocations[iRoom].nameSuffix;
	}

	public virtual string RoomId(int iRoom)
	{
		return subLocations[iRoom].id;
	}

	public virtual void Validate()
	{
		SubLocation[] array = subLocations;
		foreach (SubLocation subLocation in array)
		{
			if ((Object)(object)subLocation.room == (Object)null)
			{
				Debug.LogError((object)("Location validation error: no room in location '" + technicalName + "', sublocation '" + subLocation.nameSuffix + "'"));
			}
			if (subLocation.room is WaveRoom && WavesFactory.Instance.GetWaves(subLocation.id) == null)
			{
				Debug.LogError((object)("Location validation error: wave room '" + subLocation.nameSuffix + "' with id '" + subLocation.id + "' does not have waves defined."));
			}
		}
	}
}
