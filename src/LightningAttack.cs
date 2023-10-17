using TileEnums;
using UnityEngine;

public class LightningAttack : Attack
{
	private int strikeDistance;

	public override AttackEnum AttackEnum => AttackEnum.lightning;

	public override string LocalizationTableKey { get; } = "Lightning";


	public override int InitialValue => 2;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "LightningAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override bool IsDirectional { get; set; }

	protected override bool ClosestTargetOnly { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			AttackDeclared(attacker);
		}
		if (strikeDistance == 0)
		{
			return false;
		}
		Cell cell = attacker.Cell.Neighbour(attacker.FacingDir, strikeDistance);
		if ((Object)(object)cell != (Object)null)
		{
			EffectsManager.Instance.CreateInGameEffect("LightningEffect", ((Component)cell).transform);
		}
		return true;
	}

	public override void ApplyEffect()
	{
		if (strikeDistance != 0)
		{
			Cell cell = attacker.Cell.Neighbour(attacker.FacingDir, strikeDistance);
			if ((Object)(object)cell != (Object)null && (Object)(object)cell.Agent != (Object)null)
			{
				HitTarget(cell.Agent);
			}
		}
	}

	public override void AttackDeclared(Agent attackingAgent)
	{
		Agent[] array = AgentsInRange(attackingAgent);
		strikeDistance = 0;
		for (int num = array.Length - 1; num >= 0; num--)
		{
			if (attackingAgent.IsOpponent(array[num]))
			{
				strikeDistance = attackingAgent.Cell.Distance(array[num].Cell);
				break;
			}
		}
	}
}
