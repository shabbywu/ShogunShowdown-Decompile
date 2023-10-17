using System.Collections;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;
using Utils;

public class EarthImpaleAttack : Attack
{
	private List<Cell> targetCells;

	public override AttackEnum AttackEnum => AttackEnum.earthImpale;

	public override string LocalizationTableKey { get; } = "EarthImpale";


	public override int InitialValue => 2;

	public override int InitialCooldown => 4;

	public override int[] Range { get; protected set; } = new int[2] { -2, 2 };


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	protected override bool IsDirectional { get; set; } = true;


	protected override bool ClosestTargetOnly { get; set; }

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		Agent[] targets = AgentsInRange(attacker);
		attacker.AttackInProgress = true;
		targetCells = new List<Cell>();
		int[] range = Range;
		foreach (int distance in range)
		{
			Cell cell = attacker.Cell.Neighbour(Dir.right, distance);
			if ((Object)(object)cell != (Object)null)
			{
				targetCells.Add(cell);
			}
		}
		foreach (Cell targetCell in targetCells)
		{
			GameObject val = EffectsManager.Instance.CreateInGameEffect("EarthImpaleEffect", ((Component)targetCell).transform);
			if (val.transform.position.x < ((Component)attacker).transform.position.x)
			{
				val.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		SoundEffectsManager.Instance.Play("EarthImpalePreAttack");
		yield return (object)new WaitForSeconds(0.3f);
		SoundEffectsManager.Instance.Play("EarthImpaleAttack");
		yield return (object)new WaitForSeconds(0.1f);
		Agent[] array = targets;
		foreach (Agent target in array)
		{
			HitTarget(target);
		}
		yield return (object)new WaitForSeconds(0.3f);
		attacker.AttackInProgress = false;
	}
}
