using TileEnums;

public class SpearAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.spear;

	public override string LocalizationTableKey => "Spear";

	public override int InitialValue => 2;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[2] { 1, 2 };


	public override string AnimationTrigger { get; protected set; } = "SpearAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override bool ClosestTargetOnly { get; set; }

	public override void ApplyEffect()
	{
		Agent[] array = AgentsInRange(attacker);
		if (array.Length == 0)
		{
			SoundEffectsManager.Instance.Play("MissHit");
			return;
		}
		Agent[] array2 = array;
		foreach (Agent target in array2)
		{
			HitTarget(target);
		}
	}
}
