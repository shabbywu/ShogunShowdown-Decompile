using TileEnums;
using UnityEngine;

public class WindAttack : Attack
{
	private float pushSpeed = 7f;

	public override AttackEnum AttackEnum => AttackEnum.wind;

	public override string LocalizationTableKey => "WarFan";

	public override int InitialValue => 1;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "WindAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[2]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.poison
	};


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		EffectsManager.Instance.CreateInGameEffect("WindEffect", ((Component)attacker.AgentGraphics).transform).GetComponent<WindEffect>();
		Agent[] array = AgentsInRange(attacker);
		if (array.Length == 0)
		{
			attacker.RegisterAttackInProgress(0.4f);
		}
		else
		{
			Agent agent = array[0];
			Cell other = agent.Cell.LastFreeCellInDirection(attacker.FacingDir);
			float num = (float)agent.Cell.Distance(other) / pushSpeed;
			float t = Mathf.Max(0.4f, TimeWindReachesOpponent(attacker, agent) + num);
			attacker.RegisterAttackInProgress(t);
		}
		return true;
	}

	public override void ApplyEffect()
	{
		Agent[] array = AgentsInRange(attacker);
		if (array.Length != 0)
		{
			if (array.Length > 1)
			{
				Debug.LogError((object)"Wind attack can have at most than 1 target.");
			}
			Agent agent = array[0];
			Cell cell = agent.Cell.LastFreeCellInDirection(attacker.FacingDir);
			float waitBeforeMoving = TimeWindReachesOpponent(attacker, agent);
			if (attacker.Cell.Distance(agent.Cell) == 1)
			{
				HitTarget(agent);
			}
			if (agent.Movable && (Object)(object)cell != (Object)(object)agent.Cell)
			{
				agent.ImposedMovement(cell, pushSpeed, waitBeforeMoving);
			}
		}
	}

	private float TimeWindReachesOpponent(Agent agent, Agent target)
	{
		return (float)agent.Cell.Distance(target.Cell) / WindEffect.speed;
	}
}
