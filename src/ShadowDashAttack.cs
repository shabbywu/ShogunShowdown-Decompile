using System.Collections;
using TileEnums;
using UnityEngine;

public class ShadowDashAttack : Attack
{
	private float dashSpeed = 25f;

	public override AttackEnum AttackEnum => AttackEnum.shadowDash;

	public override string LocalizationTableKey => "ShadowDash";

	public override int InitialValue => 1;

	public override int InitialCooldown => 5;

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


	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		Agent[] array = AgentsInRange(attacker);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		Cell cell = attacker.Cell;
		Cell cell2 = attacker.Cell;
		Cell cell4;
		while (true)
		{
			Cell cell3 = cell2.Neighbour(attacker.FacingDir, 1);
			if ((Object)(object)cell3 == (Object)null)
			{
				return false;
			}
			if ((Object)(object)cell3.Agent != (Object)null)
			{
				cell4 = cell3.Neighbour(attacker.FacingDir, 1);
				if ((Object)(object)cell4 != (Object)null && (Object)(object)cell4.Agent == (Object)null)
				{
					break;
				}
			}
			cell2 = cell3;
		}
		cell2 = cell4;
		if ((Object)(object)cell2 == (Object)(object)cell)
		{
			return false;
		}
		attacker.AttackInProgress = true;
		((MonoBehaviour)this).StartCoroutine(DashThrough(cell, cell2));
		return true;
	}

	private IEnumerator DashThrough(Cell originCell, Cell targetCell)
	{
		yield return (object)new WaitForSeconds(0.05f);
		SoundEffectsManager.Instance.Play("ShadowDash");
		Vector3 position = ((Component)originCell).transform.position;
		Vector3 position2 = ((Component)targetCell).transform.position;
		float time = Vector3.Distance(position, position2) / dashSpeed;
		yield return ((MonoBehaviour)this).StartCoroutine(attacker.MoveToCoroutine(position, position2, time, 0f, createDustEffect: true, createDashEffect: true));
		for (int i = 1; i < originCell.Distance(targetCell); i++)
		{
			Agent agent = originCell.Neighbour(attacker.FacingDir, i).Agent;
			if ((Object)(object)agent != (Object)null)
			{
				HitTarget(agent);
			}
		}
		attacker.Cell = targetCell;
		yield return (object)new WaitForSeconds(0.05f);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.AttackInProgress = false;
	}
}
