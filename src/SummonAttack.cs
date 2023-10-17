using TileEnums;

public class SummonAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.summon;

	public override string LocalizationTableKey => "Summon";

	public override int InitialValue => -1;

	public override int InitialCooldown => 6;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "Summon";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[1] { AttackEffectEnum.replay };


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		if (CombatManager.Instance.Agents.Count >= CombatSceneManager.Instance.Room.Grid.NCells)
		{
			return false;
		}
		SoundEffectsManager.Instance.PlayAfterDeltaT("WaveBegins", 0.2f);
		return true;
	}

	public override void ApplyEffect()
	{
		Enemy getNextEnemyToSummon = ((Enemy)attacker).GetNextEnemyToSummon;
		new Wave(new Enemy[1] { getNextEnemyToSummon }, 1, 1f, summoned: true).Spawn((CombatRoom)CombatSceneManager.Instance.Room);
	}
}
