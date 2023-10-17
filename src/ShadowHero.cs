using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AgentEnums;
using TileEnums;
using UnityEngine;
using UnlocksID;
using Utils;

public class ShadowHero : Hero
{
	public override string TechnicalName { get; } = "Shadow";


	public override HeroEnum HeroEnum { get; } = HeroEnum.shadow;


	public override UnlockID UnlockID => UnlockID.h_shadow;

	public override List<AttackEnum[]> Decks
	{
		get
		{
			List<AttackEnum[]> list = new List<AttackEnum[]>();
			list.Add(new AttackEnum[2]
			{
				AttackEnum.swirl,
				AttackEnum.grapplingHook
			});
			list.Add(new AttackEnum[2]
			{
				AttackEnum.spear,
				AttackEnum.mirror
			});
			return list;
		}
	}

	public override IEnumerator PerformMoveAction(Dir dir)
	{
		base.CellBeforeExecution = base.Cell;
		if (RegularMoveAllowed(dir))
		{
			yield return ((MonoBehaviour)this).StartCoroutine(Dash(dir));
		}
		else if (base.SpecialMove.Allowed(this, dir))
		{
			yield return ((MonoBehaviour)this).StartCoroutine(base.SpecialMove.Perform(this, dir));
		}
	}

	private IEnumerator Dash(Dir dir)
	{
		Cell cell = base.Cell.LastFreeCellInDirection(dir);
		if (Globals.InCamp)
		{
			foreach (Cell item in base.Cell.AllCellsInDirection(dir))
			{
				if (item is InteractiveCell)
				{
					cell = item;
					break;
				}
			}
		}
		SoundEffectsManager.Instance.Play("ShadowDash");
		float speed = ((base.Cell.NeighbouringCells.Contains(cell) || Globals.InCamp) ? 15f : 20f);
		Coroutine val = ((MonoBehaviour)this).StartCoroutine(DashToOtherCell(cell, speed));
		base.Cell = cell;
		yield return val;
	}
}
