using TileEnums;
using UnityEngine;
using UnityEngine.Events;

public class CrossbowAttack : Attack
{
	private static int maxNumberOfTargets = 2;

	private Tile tile;

	public override AttackEnum AttackEnum => AttackEnum.crossbow;

	public override string LocalizationTableKey { get; } = "Crossbow";


	public override int InitialValue => 3;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	protected override bool ClosestTargetOnly { get; set; }

	public override string AnimationTrigger
	{
		get
		{
			if (base.Value == 0)
			{
				return "LightningAttack";
			}
			return "RangedAttack";
		}
		protected set
		{
		}
	}

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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		base.Initialize(maxLevel);
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(UponEndOfCombat));
		tile = ((Component)this).GetComponent<Tile>();
	}

	public override void ApplyEffect()
	{
		if (base.Value == 0)
		{
			SoundEffectsManager.Instance.Play("CrossbowReloading");
			base.Value = base.BaseValue;
			tile.Graphics.UpdateValueGraphics();
			return;
		}
		Agent[] array = AgentsInRange(attacker);
		if (array.Length == 0)
		{
			SoundEffectsManager.Instance.Play("MissHit");
		}
		else
		{
			for (int i = 0; i < Mathf.Min(array.Length, maxNumberOfTargets); i++)
			{
				HitTarget(array[i]);
			}
		}
		base.Value = 0;
		tile.Graphics.UpdateValueGraphics();
	}

	private void UponEndOfCombat()
	{
		base.Value = base.BaseValue;
		tile.Graphics.UpdateValueGraphics();
	}
}
