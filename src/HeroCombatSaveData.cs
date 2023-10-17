using System;
using AgentEnums;

[Serializable]
public class HeroCombatSaveData : AgentCombatSaveData
{
	public string name;

	public HeroEnum heroEnum;

	public int specialMoveCooldownCharge;
}
