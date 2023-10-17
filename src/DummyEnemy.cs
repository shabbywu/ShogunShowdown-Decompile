using AgentEnums;
using CombatEnums;
using UnityEngine;
using Utils;

public class DummyEnemy : Enemy
{
	public override string TechnicalName { get; } = "Dummy";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.dummy;


	public override bool IsPurelyRangedEnemy { get; }

	public override bool CanBeElite { get; }

	protected override int DefaultInitialHP { get; } = 4;


	protected override int HigerInitialHP { get; } = 4;


	public override void Start()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if (((Component)Globals.Hero).transform.position.x > ((Component)this).transform.position.x)
		{
			base.FacingDir = Dir.right;
		}
		else
		{
			base.FacingDir = Dir.left;
		}
	}

	protected override ActionEnum AIPickAction()
	{
		return ActionEnum.wait;
	}
}
