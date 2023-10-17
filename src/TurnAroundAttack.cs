using TileEnums;
using UnityEngine;

public class TurnAroundAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.turnAround;

	public override string LocalizationTableKey => "TurnAround";

	public override int InitialValue => -1;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void Initialize(int maxLevel)
	{
		base.Initialize(maxLevel);
		base.TileEffect = TileEffectEnum.freePlay;
	}

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		attacker.TurnAround();
		attacker.RegisterAttackInProgress(Agent.turnAroundTime);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		return true;
	}
}
