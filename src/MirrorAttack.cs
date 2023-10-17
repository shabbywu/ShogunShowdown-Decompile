using System.Collections;
using TileEnums;
using UnityEngine;

public class MirrorAttack : Attack
{
	private Cell targetCell;

	private Cell initialCell;

	private Agent targetAgent;

	public override AttackEnum AttackEnum => AttackEnum.mirror;

	public override string LocalizationTableKey => "Mirror";

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
		initialCell = attacker.Cell;
		targetCell = MirrorCell(initialCell);
		targetAgent = null;
		if ((Object)(object)targetCell != (Object)(object)initialCell && (Object)(object)targetCell.Agent != (Object)null)
		{
			targetAgent = targetCell.Agent;
		}
		((MonoBehaviour)this).StartCoroutine(PerformAttack());
		return true;
	}

	private IEnumerator PerformAttack()
	{
		attacker.AttackInProgress = true;
		SoundEffectsManager.Instance.Play("Spawn");
		EffectsManager.Instance.CreateInGameEffect("MirrorEffect", ((Component)initialCell).transform);
		if ((Object)(object)initialCell != (Object)(object)targetCell)
		{
			EffectsManager.Instance.CreateInGameEffect("MirrorEffect", ((Component)targetCell).transform);
		}
		yield return (object)new WaitForSeconds(0.25f);
		PerformMirrorSwapping();
		yield return (object)new WaitForSeconds(0.3f);
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.AttackInProgress = false;
	}

	private void PerformMirrorSwapping()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		attacker.Cell = targetCell;
		((Component)attacker).transform.position = ((Component)attacker.Cell).transform.position;
		attacker.Flip();
		if ((Object)(object)targetAgent != (Object)null)
		{
			targetAgent.Cell = initialCell;
			((Component)targetAgent).transform.position = ((Component)targetAgent.Cell).transform.position;
			targetAgent.Flip();
		}
	}

	private Cell MirrorCell(Cell cell)
	{
		int nCells = CombatSceneManager.Instance.Room.Grid.NCells;
		int indexInGrid = attacker.Cell.IndexInGrid;
		int num = Mathf.FloorToInt((float)nCells / 2f);
		if (nCells % 2 == 0)
		{
			num--;
		}
		int num2 = indexInGrid + 2 * (num - indexInGrid);
		return CombatSceneManager.Instance.Room.Grid.Cells[num2];
	}
}
