using System.Collections;
using UnityEngine;
using Utils;

public abstract class DashAttack : Attack
{
	private float dashSpeed = 15f;

	private float bounceTime = 0.2f;

	private float hitAnimationTime = 0.025f;

	protected abstract RelativeDir RelativeDashDirection { get; }

	private Dir Direction
	{
		get
		{
			if (RelativeDashDirection == RelativeDir.forward)
			{
				return attacker.FacingDir;
			}
			return DirUtils.Opposite(attacker.FacingDir);
		}
	}

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		Cell cell = attacker.Cell.LastFreeCellInDirection(Direction);
		Cell cell2 = cell.Neighbour(Direction, 1);
		bool flag = (Object)(object)cell2 != (Object)null && (Object)(object)cell2.Agent != (Object)null;
		if ((Object)(object)cell == (Object)(object)attacker.Cell && !flag)
		{
			return false;
		}
		attacker.AttackInProgress = true;
		attacker.SetIdleAnimation(value: false);
		if (flag)
		{
			((MonoBehaviour)this).StartCoroutine(DashAndHit(cell, cell2));
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(DashOnly(cell));
		}
		return true;
	}

	private IEnumerator DashOnly(Cell targetMoveCell)
	{
		yield return ((MonoBehaviour)this).StartCoroutine(Dash(((Component)attacker).transform.position, ((Component)targetMoveCell).transform.position, dashSpeed));
		attacker.Cell = targetMoveCell;
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.SetIdleAnimation(value: true);
		attacker.AttackInProgress = false;
	}

	private IEnumerator DashAndHit(Cell targetMoveCell, Cell targetHitCell)
	{
		Vector3 hitPoint = (((Component)targetMoveCell).transform.position + ((Component)targetHitCell).transform.position) / 2f;
		yield return ((MonoBehaviour)this).StartCoroutine(Dash(((Component)attacker).transform.position, hitPoint, dashSpeed));
		attacker.Cell = targetMoveCell;
		Agent agent = targetHitCell.Agent;
		HitTarget(agent);
		yield return (object)new WaitForSeconds(hitAnimationTime);
		attacker.SetIdleAnimation(value: true);
		yield return ((MonoBehaviour)this).StartCoroutine(attacker.MoveToCoroutine(hitPoint, ((Component)targetMoveCell).transform.position, bounceTime));
		EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		attacker.AttackInProgress = false;
	}

	private IEnumerator Dash(Vector3 from, Vector3 to, float speed)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("Dash");
		float time = Vector3.Distance(from, to) / speed;
		yield return ((MonoBehaviour)this).StartCoroutine(attacker.MoveToCoroutine(from, to, time, 0f, createDustEffect: true, createDashEffect: true));
	}
}
