using System.Collections;
using TileEnums;
using UnityEngine;

public class OriginAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.origin;

	public override string LocalizationTableKey => "Origin";

	public override int InitialValue => -1;

	public override int InitialCooldown => 6;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void Initialize(int maxLevel)
	{
		base.Initialize(maxLevel);
		base.TileEffect = TileEffectEnum.freePlay;
	}

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		Cell initialCell = attacker.Cell;
		Cell centralCell = CombatSceneManager.Instance.Room.Grid.CentralCell();
		Agent targetAgent = (((Object)(object)initialCell == (Object)(object)centralCell) ? null : centralCell.Agent);
		SoundEffectsManager.Instance.Play("Spawn");
		EffectsManager.Instance.CreateInGameEffect("MirrorEffect", ((Component)initialCell).transform);
		if ((Object)(object)initialCell != (Object)(object)centralCell)
		{
			EffectsManager.Instance.CreateInGameEffect("MirrorEffect", ((Component)centralCell).transform);
		}
		yield return (object)new WaitForSeconds(0.25f);
		attacker.Cell = centralCell;
		((Component)attacker).transform.position = ((Component)attacker.Cell).transform.position;
		if ((Object)(object)targetAgent != (Object)null)
		{
			targetAgent.Cell = initialCell;
			((Component)targetAgent).transform.position = ((Component)targetAgent.Cell).transform.position;
		}
		if ((Object)(object)attacker == (Object)(object)Globals.Hero && (Object)(object)initialCell != (Object)(object)centralCell)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		yield return (object)new WaitForSeconds(0.3f);
		attacker.AttackInProgress = false;
	}
}
