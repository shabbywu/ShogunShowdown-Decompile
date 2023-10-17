using System;
using System.Collections.Generic;

[Serializable]
public class CombatRoomSaveData
{
	public List<EnemyCombatSaveData> enemies;

	public int iWave;

	public int nTurnsBeforeNextWave;

	public CombatRoomSaveData()
	{
		enemies = new List<EnemyCombatSaveData>();
		iWave = 0;
		nTurnsBeforeNextWave = 0;
	}
}
