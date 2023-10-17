using TileEnums;
using UnityEngine;

public class BarricadeAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.barricade;

	public override string LocalizationTableKey { get; } = "Barricade";


	public override int InitialValue => -1;

	public override int InitialCooldown => 5;

	public override int[] Range { get; protected set; } = new int[1] { 1 };


	public override string AnimationTrigger { get; protected set; } = "Summon";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		if ((Object)(object)attacker.CellInFront != (Object)null)
		{
			return (Object)(object)attacker.CellInFront.Agent == (Object)null;
		}
		return false;
	}

	public override void ApplyEffect()
	{
		GameObject enemyPrefab = Resources.Load<GameObject>("Agents/Enemies/BarricadeEnemy");
		EffectsManager.Instance.GenericSpawningEffect();
		SoundEffectsManager.Instance.Play("ShopUpgrade");
		Spawner.SpawnEnemy(attacker.CellInFront, enemyPrefab).Summoned = true;
	}
}
