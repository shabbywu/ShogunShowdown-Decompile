using System.Collections;
using UnityEngine;
using Utils;

public class SwapSpecialMove : SpecialMove
{
	public override int InitialCooldown { get; } = 4;


	public override RelativeDir DefaultRelativeDir { get; }

	public override IEnumerator Perform(Hero hero, Dir dir, bool depleteSpecialMoveCooldown = true)
	{
		Agent agent = hero.Cell.Neighbour(dir, 1).Agent;
		if (base.HasEffectOnTarget)
		{
			((MonoBehaviour)this).StartCoroutine(ApplyEffectOnTargetAfterPositionCrossing(hero, agent));
		}
		Cell cell = agent.Cell;
		Cell cell2 = hero.Cell;
		cell2.Agent = null;
		cell.Agent = null;
		agent.ImposedMovement(cell2);
		Coroutine val = ((MonoBehaviour)this).StartCoroutine(hero.DashToOtherCell(cell, 9.1f, createDustEffect: false, createDashEffect: false));
		hero.Cell = cell;
		if (depleteSpecialMoveCooldown)
		{
			base.Cooldown.SpecialMoveActionPerformed();
		}
		hero.MoveActionPerformed = true;
		yield return val;
	}

	public override bool Allowed(Hero hero, Dir dir)
	{
		Cell cell = hero.Cell.Neighbour(dir, 1);
		if (!base.IsEnabled)
		{
			return false;
		}
		if ((Object)(object)cell == (Object)null || (Object)(object)cell.Agent == (Object)null || !cell.Agent.Movable)
		{
			return false;
		}
		if (dir != hero.FacingDir && !base.CanDoBackwards)
		{
			return false;
		}
		return true;
	}
}
