using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class SwapTossAttack : Attack
{
	private float moveSpeed = 12f;

	private Agent leftAgent;

	private Agent rightAgent;

	private Cell leftCell;

	private Cell rightCell;

	public override AttackEnum AttackEnum => AttackEnum.swapToss;

	public override string LocalizationTableKey { get; } = "SwapToss";


	public override int InitialValue => -1;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = new int[2] { -1, 1 };


	public override string AnimationTrigger { get; protected set; } = "SwapTossAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void Initialize(int maxLevel)
	{
		base.Initialize(maxLevel);
		base.TileEffect = TileEffectEnum.freePlay;
	}

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		leftCell = attackingAgent.Cell.Neighbour(Dir.left, 1);
		rightCell = attackingAgent.Cell.Neighbour(Dir.right, 1);
		if ((Object)(object)leftCell == (Object)null)
		{
			return false;
		}
		if ((Object)(object)rightCell == (Object)null)
		{
			return false;
		}
		leftAgent = leftCell.Agent;
		rightAgent = rightCell.Agent;
		if ((Object)(object)leftAgent != (Object)null && !leftAgent.Movable)
		{
			return false;
		}
		if ((Object)(object)rightAgent != (Object)null && !rightAgent.Movable)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		if ((Object)(object)leftAgent == (Object)null && (Object)(object)rightAgent == (Object)null)
		{
			SoundEffectsManager.Instance.Play("MissHit");
		}
		else
		{
			SoundEffectsManager.Instance.Play("Swap");
		}
		leftCell.Agent = null;
		rightCell.Agent = null;
		if ((Object)(object)leftAgent != (Object)null)
		{
			leftAgent.ImposedMovement(rightCell, moveSpeed, 0f, createDustEffect: true);
		}
		if ((Object)(object)rightAgent != (Object)null)
		{
			rightAgent.ImposedMovement(leftCell, moveSpeed, 0f, createDustEffect: true);
		}
		yield return (object)new WaitForSeconds(0.35f);
		attacker.AttackInProgress = false;
	}
}
