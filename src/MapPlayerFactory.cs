using UnityEngine;

public class MapPlayerFactory : MonoBehaviour
{
	public MapPlayer mapPlayerWanderer;

	public MapPlayer mapPlayerRonin;

	public MapPlayer mapPlayerShadow;

	public MapPlayer mapPlayerJujitsuka;

	public MapPlayer InstantiateMapPlayer(Hero player)
	{
		MapPlayer mapPlayer = null;
		if (player is WandererHero)
		{
			mapPlayer = mapPlayerWanderer;
		}
		else if (player is RoninHero)
		{
			mapPlayer = mapPlayerRonin;
		}
		else if (player is ShadowHero)
		{
			mapPlayer = mapPlayerShadow;
		}
		else if (player is JujitsukaHero)
		{
			mapPlayer = mapPlayerJujitsuka;
		}
		else
		{
			Debug.LogError((object)("MapPlayerFactory: the player '" + player.Name + "' does not have a corresponding MapPlayer"));
		}
		return Object.Instantiate<GameObject>(((Component)mapPlayer).gameObject, ((Component)this).transform).GetComponent<MapPlayer>();
	}
}
