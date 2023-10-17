using System;
using System.Collections.Generic;

[Serializable]
public struct RoomMetrics
{
	public string roomId;

	public int initialHP;

	public int finalHP;

	public int damageTaken;

	public int turns;

	public int coins;

	public int time;

	public int iWave;

	public int nPlayerSpecialMove;

	public int nCombos;

	public int sector;

	public List<string> deck;

	public List<string> skills;

	public List<string> attacks;

	public List<string> enemies;

	public List<string> hits;

	public List<string> consumablesUsed;
}
