using TileEnums;
using Utils;

public class DashForwardAttack : DashAttack
{
	public override AttackEnum AttackEnum => AttackEnum.dashForward;

	public override string LocalizationTableKey { get; } = "Dash";


	public override int InitialValue => 1;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "DashForward";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override RelativeDir RelativeDashDirection => RelativeDir.forward;
}
