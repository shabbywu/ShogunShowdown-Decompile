using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class CombatGrid : MonoBehaviour
{
	[SerializeField]
	private Transform cellsContainer;

	public Cell initialPlayerCell;

	private Cell[] _cells;

	public Cell[] Cells
	{
		get
		{
			if (_cells == null)
			{
				_cells = GetCellsPreservingOrderInHierarchy();
			}
			return _cells;
		}
	}

	public int NCells => Cells.Length;

	private void Awake()
	{
		ForceLayoutUpdate();
		ResetAgentPositionOnCells();
	}

	public void Initialize()
	{
		if (Cells.Length > Globals.MaxRoomSize)
		{
			Debug.LogError((object)$"Number of cells in grid {Cells.Length}, but Globals.MaxRoomSize is {Globals.MaxRoomSize}");
		}
		InitializeCellsNeighboursAndIndex();
	}

	public Cell LeftmostFreeCell()
	{
		for (int i = 0; i < NCells; i++)
		{
			if (Cells[i].IsFree)
			{
				return Cells[i];
			}
		}
		return null;
	}

	public Cell RightmostFreeCell()
	{
		for (int num = NCells - 1; num >= 0; num--)
		{
			if (Cells[num].IsFree)
			{
				return Cells[num];
			}
		}
		return null;
	}

	public Cell RightMostCell()
	{
		return Cells[NCells - 1];
	}

	public Cell LeftMostCell()
	{
		return Cells[0];
	}

	public Cell CentralCell()
	{
		return Cells[(NCells - 1) / 2];
	}

	private void InitializeCellsNeighboursAndIndex()
	{
		for (int i = 0; i < NCells; i++)
		{
			Cells[i].IndexInGrid = i;
			if (i > 0)
			{
				Cells[i].Neighbours.Add(Dir.left, Cells[i - 1]);
			}
			if (i < NCells - 1)
			{
				Cells[i].Neighbours.Add(Dir.right, Cells[i + 1]);
			}
		}
	}

	private void ForceLayoutUpdate()
	{
		HorizontalLayoutGroup componentInChildren = ((Component)this).GetComponentInChildren<HorizontalLayoutGroup>();
		((LayoutGroup)componentInChildren).CalculateLayoutInputHorizontal();
		((LayoutGroup)componentInChildren).SetLayoutHorizontal();
	}

	private void ResetAgentPositionOnCells()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Cell[] cells = Cells;
		foreach (Cell cell in cells)
		{
			if ((Object)(object)cell.Agent != (Object)null)
			{
				((Component)cell.Agent).transform.position = ((Component)cell).transform.position;
			}
		}
	}

	private Cell[] GetCellsPreservingOrderInHierarchy()
	{
		List<Cell> list = new List<Cell>();
		for (int i = 0; i < cellsContainer.childCount; i++)
		{
			list.Add(((Component)cellsContainer.GetChild(i)).gameObject.GetComponent<Cell>());
		}
		return list.ToArray();
	}
}
