using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombaUtilsNameSpace;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class BlazingBarrage : MonoBehaviour, ICombatTask
{
	private int damage;

	private Cell cell;

	private Agent attacker;

	private int distance;

	private int maxTurns;

	private List<GameObject> cellWarnings = new List<GameObject>();

	bool ICombatTask.IsFinished => GetCellsAtDistance(distance).Count == 0;

	public void Initialize(int damage, Cell cell, Agent attacker)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		this.cell = cell;
		this.damage = damage;
		this.attacker = attacker;
		distance = 1;
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(EndOfCombat));
		CombatManager.Instance.EndOfTurnTasks.Add(this);
	}

	private void RemoveListeners()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(EndOfCombat));
	}

	public void EndOfCombat()
	{
		RemoveListeners();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	IEnumerator ICombatTask.Execute()
	{
		DestroyCellWarning();
		foreach (Cell item in GetCellsAtDistance(distance))
		{
			SoundEffectsManager.Instance.Play("Spawn");
			EffectsManager.Instance.CreateInGameEffect("BlazingBarrageEffect", ((Component)item).transform.position);
			Agent agent = item.Agent;
			if ((Object)(object)agent != (Object)null)
			{
				agent.ReceiveAttack(new Hit(damage, isDirectional: false), attacker);
				SoundEffectsManager.Instance.Play("CombatHit");
			}
		}
		yield return (object)new WaitForSeconds(0.3f);
		distance++;
		foreach (Cell item2 in GetCellsAtDistance(distance))
		{
			cellWarnings.Add(EffectsManager.Instance.CreateInGameEffect("CellWarningEffect", ((Component)this).transform));
			cellWarnings.Last().transform.position = ((Component)item2).transform.position;
		}
		yield return null;
	}

	private void DestroyCellWarning()
	{
		foreach (GameObject cellWarning in cellWarnings)
		{
			Object.Destroy((Object)(object)cellWarning);
		}
	}

	void ICombatTask.FinalizeTask()
	{
		DestroyCellWarning();
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private List<Cell> GetCellsAtDistance(int d)
	{
		List<Cell> list = new List<Cell>();
		Dir[] array = new Dir[2]
		{
			Dir.left,
			Dir.right
		};
		foreach (Dir dir in array)
		{
			Cell cell = this.cell.Neighbour(dir, d);
			if ((Object)(object)cell != (Object)null)
			{
				list.Add(cell);
			}
		}
		return list;
	}
}
