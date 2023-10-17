using TileEnums;
using Utils;

public class DashBackwardAttack : DashAttack
{
	public override AttackEnum AttackEnum => AttackEnum.dashBackward;

	public override string LocalizationTableKey { get; } = "Backoff";


	public override int InitialValue => 1;

	public override int InitialCooldown => 3;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override RelativeDir RelativeDashDirection => RelativeDir.backward;
}
