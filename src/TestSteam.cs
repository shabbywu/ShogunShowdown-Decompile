using Steamworks;
using UnityEngine;

public class TestSteam : MonoBehaviour
{
	private void Start()
	{
		if (SteamManager.Initialized)
		{
			Debug.Log((object)SteamFriends.GetPersonaName());
		}
	}
}
