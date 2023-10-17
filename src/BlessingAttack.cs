using TileEnums;
using UnityEngine;

public class BlessingAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.blessing;

	public override string LocalizationTableKey { get; } = "NotImplemented";


	public override int InitialValue => -1;

	public override int InitialCooldown => 6;

	public override int[] Range { get; protected set; } = new int[1];


	public override string AnimationTrigger { get; protected set; } = "NotImplemented";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void ApplyEffect()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Cell cell = attacker.Cell;
		if ((Object)(object)cell.EffectSymbol != (Object)null)
		{
			Object.Destroy((Object)(object)cell.EffectSymbol);
		}
		cell.Effect = Cell.CellEffect.blessing;
		cell.EffectSymbol = EffectsManager.Instance.CreateInGameEffect("BlessingEffect", ((Component)cell).transform.position);
	}
}
