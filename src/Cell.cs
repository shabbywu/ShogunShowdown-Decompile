using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Cell : MonoBehaviour
{
	public enum CellEffect
	{
		none,
		curse,
		blessing
	}

	public GameObject[] NofityAgentEnters;

	public GameObject[] NofityAgentExits;

	[Header("Graphics")]
	public Sprite[] alternativeEmptySprites;

	public Sprite[] alternativePlayerSprites;

	public Sprite[] alternativeEnemySprites;

	public SpriteRenderer emptyCellSpriteRenderer;

	public SpriteRenderer playerCellSpriteRenderer;

	public SpriteRenderer enemyCellSpriteRenderer;

	private Vector3 position;

	private Dictionary<Dir, Cell> neighbours = new Dictionary<Dir, Cell>();

	private Agent agent;

	private Animator animator;

	public int IndexInGrid { get; set; }

	public CellEffect Effect { get; set; }

	public GameObject EffectSymbol { get; set; }

	public bool IsFree => (Object)(object)Agent == (Object)null;

	public Dictionary<Dir, Cell> Neighbours => neighbours;

	public Agent Agent
	{
		get
		{
			return agent;
		}
		set
		{
			if ((Object)(object)value != (Object)null && (Object)(object)value != (Object)(object)agent)
			{
				GameObject[] nofityAgentEnters = NofityAgentEnters;
				for (int i = 0; i < nofityAgentEnters.Length; i++)
				{
					nofityAgentEnters[i].SendMessage("AgentEnters", (object)value);
				}
			}
			if ((Object)(object)value == (Object)null && (Object)(object)agent != (Object)null)
			{
				GameObject[] nofityAgentEnters = NofityAgentExits;
				for (int i = 0; i < nofityAgentEnters.Length; i++)
				{
					nofityAgentEnters[i].SendMessage("AgentExits", (object)agent);
				}
			}
			agent = value;
			animator.SetBool("EnemyOnCell", (Object)(object)agent != (Object)null && agent is Enemy);
			animator.SetBool("PlayerOnCell", (Object)(object)agent != (Object)null && agent is Hero);
		}
	}

	public Cell[] NeighbouringCells
	{
		get
		{
			List<Cell> list = new List<Cell>();
			if (Neighbours.ContainsKey(Dir.left))
			{
				list.Add(Neighbours[Dir.left]);
			}
			if (Neighbours.ContainsKey(Dir.right))
			{
				list.Add(Neighbours[Dir.right]);
			}
			return list.ToArray();
		}
	}

	public Vector3 Position
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return position;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			position = value;
		}
	}

	public float Size => 1f;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
		SetAlternativeSprites();
	}

	public Cell Neighbour(Dir dir, int distance)
	{
		if (distance < 0)
		{
			dir = DirUtils.Opposite(dir);
			distance = Mathf.Abs(distance);
		}
		if (neighbours.ContainsKey(dir))
		{
			if (distance == 1)
			{
				return neighbours[dir];
			}
			return neighbours[dir].Neighbour(dir, distance - 1);
		}
		return null;
	}

	public List<Cell> AllCellsInDirection(Dir dir)
	{
		List<Cell> list = new List<Cell>();
		Cell cell = this;
		while (true)
		{
			cell = cell.Neighbour(dir, 1);
			if ((Object)(object)cell == (Object)null)
			{
				break;
			}
			list.Add(cell);
		}
		return list;
	}

	public List<Cell> AllCellsInGrid()
	{
		List<Cell> list = new List<Cell>();
		list.Add(this);
		list.AddRange(AllCellsInDirection(Dir.left));
		list.AddRange(AllCellsInDirection(Dir.right));
		return list;
	}

	public int Distance(Cell other)
	{
		if ((Object)(object)other == (Object)(object)this)
		{
			return 0;
		}
		Dir dir = DirectionToOtherCell(other);
		Cell cell = this;
		int num = 1;
		while (true)
		{
			cell = cell.Neighbour(dir, 1);
			if ((Object)(object)cell == (Object)(object)other)
			{
				return num;
			}
			if ((Object)(object)cell == (Object)null)
			{
				break;
			}
			num++;
		}
		Debug.LogError((object)"Cannot find distance between cells...");
		return 0;
	}

	public static int Distance(Cell a, Cell b)
	{
		return a.Distance(b);
	}

	public Dir DirectionToOtherCell(Cell other)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)other).transform.position.x > ((Component)this).transform.position.x)
		{
			return Dir.right;
		}
		return Dir.left;
	}

	public Cell LastFreeCellInDirection(Dir dir)
	{
		Cell cell = this;
		while (true)
		{
			Cell cell2 = cell.Neighbour(dir, 1);
			if ((Object)(object)cell2 == (Object)null || (Object)(object)cell2.Agent != (Object)null)
			{
				break;
			}
			cell = cell2;
		}
		return cell;
	}

	private void SetAlternativeSprites()
	{
		if (alternativeEmptySprites.Length != 0)
		{
			int num = Random.Range(0, alternativeEmptySprites.Length);
			emptyCellSpriteRenderer.sprite = alternativeEmptySprites[num];
			playerCellSpriteRenderer.sprite = alternativePlayerSprites[num];
			enemyCellSpriteRenderer.sprite = alternativeEnemySprites[num];
		}
	}
}
