using System.Collections;
using TileEnums;
using UnityEngine;

public class PhantomLeap : Attack
{
	private float dashSpeed = 20f;

	public override AttackEnum AttackEnum => AttackEnum.phantomLeap;

	public override string LocalizationTableKey => "PhantomLeap";

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
		Cell cell = attacker.Cell.LastFreeCellInDirection(attacker.FacingDir);
		if ((Object)(object)cell == (Object)(object)attacker.Cell)
		{
			return false;
		}
		((MonoBehaviour)this).StartCoroutine(Dash(cell));
		return true;
	}

	private IEnumerator Dash(Cell targetMoveCell)
	{
		attacker.AttackInProgress = true;
		attacker.SetIdleAnimation(value: false);
		SoundEffectsManager.Instance.Play("PhantomLeap");
		attacker.AgentGraphics.TrailRenderer.emitting = true;
		Vector3 position = ((Component)attacker).transform.position;
		Vector3 position2 = ((Component)targetMoveCell).transform.position;
		float time = Vector3.Distance(position, position2) / dashSpeed;
		yield return ((MonoBehaviour)this).StartCoroutine(attacker.MoveToCoroutine(position, position2, time, 0f, createDustEffect: true, createDashEffect: true));
		attacker.AgentGraphics.TrailRenderer.emitting = false;
		attacker.Cell = targetMoveCell;
		if ((Object)(object)attacker == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
		}
		attacker.SetIdleAnimation(value: true);
		attacker.AttackInProgress = false;
	}
}
