using System;

[Serializable]
public class RunStats
{
	public int day;

	public int coins;

	public int time;

	public int hits;

	public int turns;

	public int combos;

	public int nTurnArounds;

	public int friendlyKills;

	public int numberOfCombatRoomsCleared;

	public int nHealPickupDrops;

	public int nPotionsPickupDrops;

	public int nScrollsPickupDrops;

	public int nConsumablesUsed;

	public int nNewTilesPicked;

	public void Initialize()
	{
		day = Globals.Day;
		coins = 0;
		time = 0;
		hits = 0;
		turns = 0;
		combos = 0;
		nTurnArounds = 0;
		friendlyKills = 0;
		numberOfCombatRoomsCleared = 0;
		nHealPickupDrops = 0;
		nPotionsPickupDrops = 0;
		nScrollsPickupDrops = 0;
		nConsumablesUsed = 0;
		nNewTilesPicked = 0;
	}
}
