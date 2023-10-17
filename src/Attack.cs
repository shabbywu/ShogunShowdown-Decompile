using System;
using System.Collections.Generic;
using Parameters;
using TileEnums;
using UnityEngine;
using Utils;

public abstract class Attack : MonoBehaviour
{
	private int cooldown;

	public static int maxCooldown = 8;

	public static int maxMaxLevel = 8;

	public static int maxValue = 9;

	protected Agent attacker;

	public abstract AttackEnum AttackEnum { get; }

	public abstract int InitialValue { get; }

	public abstract int InitialCooldown { get; }

	public abstract int[] Range { get; protected set; }

	public abstract string AnimationTrigger { get; protected set; }

	public abstract AttackEffectEnum[] CompatibleEffects { get; protected set; }

	public abstract string LocalizationTableKey { get; }

	public bool HasValue => Value >= 0;

	public bool HasAnimation => AnimationTrigger != "";

	protected virtual bool ClosestTargetOnly { get; set; } = true;


	protected virtual bool IsDirectional { get; set; } = true;


	public int Value { get; set; }

	public int BaseValue { get; set; }

	public string Name => LocalizationUtils.LocalizedString("TileAttacks", LocalizationTableKey + "_Name");

	public string Description => LocalizationUtils.LocalizedString("TileAttacks", LocalizationTableKey + "_Description");

	public int Cooldown
	{
		get
		{
			return cooldown;
		}
		set
		{
			cooldown = Mathf.Clamp(value, 0, maxCooldown);
		}
	}

	public int Level { get; set; }

	public int MaxLevel { get; set; }

	public AttackEffectEnum AttackEffect { get; set; }

	public TileEffectEnum TileEffect { get; set; }

	public string TechName => LocalizationTableKey;

	public string TechNameAndStats
	{
		get
		{
			string text = $"{TechName}_lvl:{Level}_dmg:{Value}_cd:{Cooldown}";
			if (AttackEffect != 0)
			{
				text = text + "_" + Enum.GetName(typeof(AttackEffectEnum), AttackEffect);
			}
			return text;
		}
	}

	protected static int[] InfiniteForwardRange
	{
		get
		{
			int[] array = new int[Globals.MaxRoomSize - 1];
			for (int i = 0; i < Globals.MaxRoomSize - 1; i++)
			{
				array[i] = i + 1;
			}
			return array;
		}
	}

	public virtual void Initialize(int maxLevel)
	{
		Value = InitialValue;
		BaseValue = InitialValue;
		Cooldown = InitialCooldown;
		Level = 0;
		MaxLevel = maxLevel;
	}

	public virtual void ApplyEffect()
	{
	}

	public virtual bool Begin(Agent attackingAgent)
	{
		attacker = attackingAgent;
		return true;
	}

	public virtual void AttackDeclared(Agent attackingAgent)
	{
	}

	protected void HitTarget(Agent target, string hitSoundEffect = "CombatHit")
	{
		target.ReceiveAttack(new Hit(Value, IsDirectional, isCollision: false, TechNameAndStats), attacker);
		ProcessAttackEffects(target, AttackEffect);
		EffectsManager.Instance.ScreenShake();
		if (hitSoundEffect != null)
		{
			SoundEffectsManager.Instance.Play(hitSoundEffect);
		}
	}

	public virtual bool WaitingForSomethingToFinish()
	{
		return false;
	}

	public virtual Agent[] AgentsInRange(Agent attackingAgent)
	{
		List<Agent> list = new List<Agent>();
		int[] range = Range;
		foreach (int distance in range)
		{
			Cell cell = attackingAgent.Cell.Neighbour(attackingAgent.FacingDir, distance);
			if ((Object)(object)cell != (Object)null && (Object)(object)cell.Agent != (Object)null)
			{
				list.Add(cell.Agent);
			}
			if (ClosestTargetOnly && list.Count > 0)
			{
				break;
			}
		}
		return list.ToArray();
	}

	protected Agent AgentInRange(Agent attackingAgent)
	{
		Agent[] array = AgentsInRange(attackingAgent);
		if (array.Length == 0)
		{
			return null;
		}
		return array[0];
	}

	public static void ProcessAttackEffects(Agent target, AttackEffectEnum attackEffect)
	{
		ProcessAttackEffects(new Agent[1] { target }, attackEffect);
	}

	private static void ProcessAttackEffects(Agent[] targets, AttackEffectEnum attackEffect)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (targets.Length == 0)
		{
			return;
		}
		Agent[] array;
		switch (attackEffect)
		{
		case AttackEffectEnum.none:
		case AttackEffectEnum.replay:
			return;
		case AttackEffectEnum.ice:
			array = targets;
			foreach (Agent agent in array)
			{
				if ((Object)(object)agent != (Object)null && agent.AgentStats.HP > 0)
				{
					agent.Freeze(GameParams.iceEffectTurnsDuration);
				}
			}
			break;
		}
		if (attackEffect == AttackEffectEnum.electric)
		{
			EffectsManager.Instance.CreateInGameEffect("ElectricChainEffect", Vector3.zero).GetComponent<ElectricChainEffect>().Initialize(targets);
		}
		if (attackEffect != AttackEffectEnum.poison)
		{
			return;
		}
		array = targets;
		foreach (Agent agent2 in array)
		{
			if ((Object)(object)agent2 != (Object)null && agent2.AgentStats.HP > 0)
			{
				agent2.ApplyPoisonEffect(GameParams.poisonEffectTurnsDuration);
			}
		}
	}
}
