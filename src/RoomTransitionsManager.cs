using System.Collections;
using UnityEngine;
using Utils;

public class RoomTransitionsManager : MonoBehaviour
{
	public static float roomsDeltaX = 20f;

	public static Vector3 roomsDelta = roomsDeltaX * Vector3.right;

	public static RoomTransitionsManager Instance { get; private set; }

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public IEnumerator RoomToRoomTransition(Room initial, Room final, SimpleCameraFollow cameraFollow)
	{
		((MonoBehaviour)this).StartCoroutine(cameraFollow.EnableScreenScroll(((Component)cameraFollow).transform.position, ((Component)final).transform.position));
		while (((Component)cameraFollow).transform.position.x < (((Component)initial).transform.position.x + ((Component)final).transform.position.x) / 2f)
		{
			yield return null;
		}
		((Component)Globals.Hero).transform.position = ((Component)Globals.Hero.Cell).transform.position;
		Globals.Hero.FacingDir = Dir.right;
		while (((Component)cameraFollow).transform.position.x < ((Component)final).transform.position.x - 0.1f)
		{
			yield return null;
		}
		Object.Destroy((Object)(object)((Component)initial).gameObject);
	}
}
