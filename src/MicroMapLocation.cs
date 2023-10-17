using UnityEngine;

[CreateAssetMenu(fileName = "NewMicroMapLocation", menuName = "SO/MicroMapLocation", order = 1)]
public class MicroMapLocation : ScriptableObject
{
	public bool showInMicroMap = true;

	public Sprite activeSprite;

	public Sprite inactiveSprite;
}
