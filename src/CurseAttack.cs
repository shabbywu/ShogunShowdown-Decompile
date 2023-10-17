using TileEnums;
using UnityEngine;

public class CurseAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.curse;

	public override string LocalizationTableKey { get; } = "NotImplemented";


	public override int InitialValue => -1;

	public override int InitialCooldown => 6;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "NotImplemented";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void ApplyEffect()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Cell cell = attacker.Cell.Neighbour(attacker.FacingDir, 1);
		if ((Object)(object)cell != (Object)null)
		{
			if ((Object)(object)cell.EffectSymbol != (Object)null)
			{
				Object.Destroy((Object)(object)cell.EffectSymbol);
			}
			cell.Effect = Cell.CellEffect.curse;
			cell.EffectSymbol = EffectsManager.Instance.CreateInGameEffect("CurseEffect", ((Component)cell).transform.position);
		}
	}
}
