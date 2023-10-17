using TileEnums;

public class ShieldAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.shield;

	public override string LocalizationTableKey => "Shield";

	public override int InitialValue => -1;

	public override int InitialCooldown => 0;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "ShieldAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void ApplyEffect()
	{
		attacker.AddShield();
	}
}
