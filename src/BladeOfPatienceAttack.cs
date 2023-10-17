using System;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;

public class BladeOfPatienceAttack : Attack
{
	private Tile tile;

	private bool wasInAttackQueue;

	public override AttackEnum AttackEnum => AttackEnum.bladeOfPatience;

	public override string LocalizationTableKey => "BladeOfPatience";

	public override int InitialValue => 0;

	public override int InitialCooldown => 6;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "MeleeAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	public override void Initialize(int maxLevel)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		base.Initialize(maxLevel);
		tile = ((Component)this).GetComponent<Tile>();
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(UponEndOfTurn));
		tile.TileContainerChanged += TileContainerChanged;
	}

	private void UponEndOfTurn()
	{
		if (tile.TileContainer is AttackQueueTileContainer)
		{
			if (wasInAttackQueue || base.TileEffect == TileEffectEnum.freePlay)
			{
				base.Value = Mathf.Clamp(base.Value + 1, 0, Attack.maxValue);
				tile.Graphics.UpdateValueGraphics();
			}
			wasInAttackQueue = true;
		}
	}

	private void TileContainerChanged(object sender, EventArgs e)
	{
		if (tile.TileContainer is HandTileContainer && wasInAttackQueue)
		{
			wasInAttackQueue = false;
			base.Value = base.BaseValue;
			tile.Graphics.UpdateValueGraphics();
		}
	}

	public override void ApplyEffect()
	{
		Agent agent = AgentInRange(attacker);
		if ((Object)(object)agent == (Object)null)
		{
			SoundEffectsManager.Instance.Play("MissHit");
		}
		else
		{
			HitTarget(agent);
		}
	}
}
