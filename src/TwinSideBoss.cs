using AgentEnums;
using UnityEngine;

public class TwinSideBoss : TwinMainBoss
{
	public override string TechnicalName { get; } = "TwinsBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.twin_side_boss;


	protected override int InitialMaxHP { get; } = 100;


	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		if ((Object)(object)otherTwin != (Object)null && otherTwin.IsAlive)
		{
			otherTwin.AddToHealth(actualDeltaHealth);
		}
	}
}
