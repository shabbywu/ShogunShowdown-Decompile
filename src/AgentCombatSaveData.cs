using System;
using System.Collections.Generic;
using Utils;

[Serializable]
public class AgentCombatSaveData
{
	public Dir FacingDir;

	public int iCell;

	public AgentStats agentStats;

	public List<TileSaveData> attackQueue;

	public AgentCombatSaveData()
	{
		attackQueue = new List<TileSaveData>();
		agentStats = new AgentStats();
	}
}
