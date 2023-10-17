using System;
using AgentEnums;
using CombatEnums;
using TileEnums;

[Serializable]
public class EnemyCombatSaveData : AgentCombatSaveData
{
	public EnemyEnum enemy;

	public ActionEnum action;

	public ActionEnum previousAction;

	public AttackEnum tileToPlay;

	public AttackEffectEnum attackEffectForTileToPlay;

	public bool firstTurn;

	public EliteTypeEnum eliteType;
}
