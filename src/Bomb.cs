using System.Collections;
using System.Collections.Generic;
using CombaUtilsNameSpace;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Bomb : MonoBehaviour, ICombatTask
{
	public static float placingTime = 0.2f;

	private static int nTurnsBeforeExploding = 2;

	private Cell cell;

	private int damage;

	private int nTurnsLeft;

	private Animator animator;

	private AttackEffectEnum attackEffect;

	private Agent agentWhoPlacedTheBomb;

	bool ICombatTask.IsFinished => nTurnsLeft < 0;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void Initialize(int damage, AttackEffectEnum attackEffect, Agent agent, Cell cell)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		this.cell = cell;
		this.damage = damage;
		this.attackEffect = attackEffect;
		nTurnsLeft = nTurnsBeforeExploding;
		agentWhoPlacedTheBomb = agent;
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(EndOfCombat));
		((MonoBehaviour)this).StartCoroutine(PlaceBomb(((Component)agentWhoPlacedTheBomb).transform.position, ((Component)cell).transform.position));
	}

	private IEnumerator PlaceBomb(Vector3 initialPosition, Vector3 finalPosition)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		CombatManager.Instance.EndOfTurnTasks.Add(this);
		float currentTime = 0f;
		while (currentTime < placingTime)
		{
			((Component)this).transform.position = Vector3.Lerp(initialPosition, finalPosition, currentTime / placingTime);
			currentTime += Time.deltaTime;
			yield return null;
		}
		((Component)this).transform.position = finalPosition;
	}

	public void EndOfCombat()
	{
		RemoveListeners();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private void Explode()
	{
		RemoveListeners();
		List<Cell> list = new List<Cell>();
		list.Add(cell);
		if ((Object)(object)cell.Neighbour(Dir.left, 1) != (Object)null)
		{
			list.Add(cell.Neighbour(Dir.left, 1));
		}
		if ((Object)(object)cell.Neighbour(Dir.right, 1) != (Object)null)
		{
			list.Add(cell.Neighbour(Dir.right, 1));
		}
		foreach (Cell item in list)
		{
			if ((Object)(object)item.Agent != (Object)null)
			{
				Agent agent = item.Agent;
				agent.ReceiveAttack(new Hit(damage, isDirectional: false), agentWhoPlacedTheBomb);
				Attack.ProcessAttackEffects(agent, attackEffect);
				SoundEffectsManager.Instance.Play("CombatHit");
			}
			EffectsManager.Instance.CreateInGameEffect("BombExplosionEffect", ((Component)item).transform);
		}
		SoundEffectsManager.Instance.Play("Explosion");
		EffectsManager.Instance.ScreenShake();
	}

	private void RemoveListeners()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(EndOfCombat));
	}

	IEnumerator ICombatTask.Execute()
	{
		nTurnsLeft--;
		if (nTurnsLeft == 0)
		{
			animator.SetTrigger("AboutToExplode");
		}
		else if (nTurnsLeft < 0)
		{
			Explode();
		}
		yield return null;
	}

	void ICombatTask.FinalizeTask()
	{
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
