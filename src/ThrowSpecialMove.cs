using System.Collections;
using UnityEngine;
using Utils;

public class ThrowSpecialMove : SpecialMove
{
	public override int InitialCooldown { get; } = 4;


	public override RelativeDir DefaultRelativeDir { get; } = RelativeDir.backward;


	public override IEnumerator Perform(Hero hero, Dir dir, bool depleteSpecialMoveCooldown = true)
	{
		hero.Animator.SetTrigger("SwapTossAttack");
		Cell cell = hero.Cell.Neighbour(dir, 1);
		Cell cell2 = hero.Cell.Neighbour(DirUtils.Opposite(dir), 1);
		Agent agent = cell.Agent;
		cell.Agent = null;
		IEnumerator enumerator = null;
		if ((Object)(object)agent != (Object)null)
		{
			agent.Cell = cell2;
			enumerator = agent.Pushed(DirUtils.Opposite(dir));
		}
		if (base.HasEffectOnTarget)
		{
			((MonoBehaviour)this).StartCoroutine(ApplyEffectOnTargetAfterPositionCrossing(hero, agent));
		}
		if (enumerator != null)
		{
			yield return enumerator;
		}
		EventsManager.Instance.HeroSpecialMove.Invoke();
		if (depleteSpecialMoveCooldown)
		{
			base.Cooldown.SpecialMoveActionPerformed();
		}
		hero.MoveActionPerformed = true;
		yield return (object)new WaitForSeconds(0.1f);
	}

	public override bool Allowed(Hero hero, Dir dir)
	{
		if (dir == hero.FacingDir && !base.CanDoBackwards)
		{
			return false;
		}
		Cell cell = hero.Cell.Neighbour(dir, 1);
		Cell cell2 = hero.Cell.Neighbour(DirUtils.Opposite(dir), 1);
		if (!base.IsEnabled)
		{
			return false;
		}
		if ((Object)(object)cell == (Object)null || (Object)(object)cell.Agent == (Object)null || !cell.Agent.Movable)
		{
			return false;
		}
		if ((Object)(object)cell2 == (Object)null || (Object)(object)cell2.Agent != (Object)null)
		{
			return false;
		}
		return true;
	}
}
