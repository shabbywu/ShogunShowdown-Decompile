using System.Collections;
using UnityEngine;
using Utils;

public class PushSpecialMove : SpecialMove
{
	public override int InitialCooldown { get; } = 4;


	public override RelativeDir DefaultRelativeDir { get; }

	public override IEnumerator Perform(Hero hero, Dir dir, bool depleteSpecialMoveCooldown = true)
	{
		float num = 0.06f;
		Agent target = hero.Cell.Neighbour(dir, 1).Agent;
		if (dir == hero.FacingDir)
		{
			hero.Animator.SetTrigger("PushForward");
		}
		else
		{
			hero.Animator.SetTrigger("PushBackwards");
		}
		yield return (object)new WaitForSeconds(num);
		if (base.HasEffectOnTarget)
		{
			ApplyEffectOnTarget(hero, target);
		}
		if (depleteSpecialMoveCooldown)
		{
			base.Cooldown.SpecialMoveActionPerformed();
		}
		yield return target.Pushed(dir);
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
		if ((Object)(object)hero.Cell.Neighbour(dir, 2) == (Object)null)
		{
			return false;
		}
		return true;
	}
}
