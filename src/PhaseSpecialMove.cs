using System.Collections;
using UnityEngine;
using Utils;

public class PhaseSpecialMove : SpecialMove
{
	public override int InitialCooldown { get; } = 4;


	public override RelativeDir DefaultRelativeDir { get; }

	public override IEnumerator Perform(Hero hero, Dir dir, bool depleteSpecialMoveCooldown = true)
	{
		Cell cell = PotentialPhaseCell(hero, dir);
		Coroutine val = ((MonoBehaviour)this).StartCoroutine(hero.DashToOtherCell(cell, 20f));
		SoundEffectsManager.Instance.Play("ShadowDash");
		if (base.HasEffectOnTarget)
		{
			for (int i = 1; i < hero.Cell.Distance(cell); i++)
			{
				((MonoBehaviour)this).StartCoroutine(ApplyEffectOnTargetAfterPositionCrossing(hero, hero.Cell.Neighbour(dir, i).Agent));
			}
		}
		hero.Cell = cell;
		hero.MoveActionPerformed = true;
		if (depleteSpecialMoveCooldown)
		{
			base.Cooldown.SpecialMoveActionPerformed();
		}
		yield return val;
	}

	public override bool Allowed(Hero hero, Dir dir)
	{
		Cell cell = hero.Cell.Neighbour(dir, 1);
		if (!base.IsEnabled)
		{
			return false;
		}
		if ((Object)(object)cell == (Object)null || (Object)(object)cell.Agent == (Object)null)
		{
			return false;
		}
		if (dir != hero.FacingDir && !base.CanDoBackwards)
		{
			return false;
		}
		if ((Object)(object)PotentialPhaseCell(hero, dir) == (Object)null)
		{
			return false;
		}
		return true;
	}

	private Cell PotentialPhaseCell(Hero hero, Dir dir)
	{
		Cell cell = hero.Cell.Neighbour(dir, 1);
		if ((Object)(object)cell == (Object)null || (Object)(object)cell.Agent == (Object)null)
		{
			return null;
		}
		int num = 0;
		Cell cell2;
		do
		{
			num++;
			cell2 = hero.Cell.Neighbour(dir, num);
			if ((Object)(object)cell2 == (Object)null)
			{
				return null;
			}
		}
		while (!((Object)(object)cell2.Agent == (Object)null));
		return cell2;
	}
}
