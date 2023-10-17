using System.Collections.Generic;
using TilesUtils;
using UnityEngine;

public class TileContainersCollection : MonoBehaviour, ITilesChanged, ITileAdded
{
	private List<TileContainer> _tc = new List<TileContainer>();

	public List<TileContainer> Containers => _tc;

	public ITilesChanged NotifyTilesChanged { get; set; }

	public ITileAdded NotifyTilesAdded { get; set; }

	public bool HasEmptyContainer
	{
		get
		{
			foreach (TileContainer item in _tc)
			{
				if (!item.HasTile)
				{
					return true;
				}
			}
			return false;
		}
	}

	public TileContainer FirstEmptyContainer
	{
		get
		{
			foreach (TileContainer item in _tc)
			{
				if (!item.HasTile)
				{
					return item;
				}
			}
			return null;
		}
	}

	public TileContainer FirstEmptyInteractableContainer
	{
		get
		{
			foreach (TileContainer item in _tc)
			{
				if (!item.HasTile && item.Interactable)
				{
					return item;
				}
			}
			return null;
		}
	}

	public int NContainers => _tc.Count;

	public int NTiles
	{
		get
		{
			int num = 0;
			foreach (TileContainer item in _tc)
			{
				if (item.HasTile)
				{
					num++;
				}
			}
			return num;
		}
	}

	public Tile[] Tiles
	{
		get
		{
			List<Tile> list = new List<Tile>();
			foreach (TileContainer item in _tc)
			{
				if (item.HasTile)
				{
					list.Add(item.Tile);
				}
			}
			return list.ToArray();
		}
	}

	public int NumberOfInteractableTiles
	{
		get
		{
			int num = 0;
			Tile[] tiles = Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				if (tiles[i].Interactable)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool IsFragmented
	{
		get
		{
			for (int i = 0; i < NTiles; i++)
			{
				if (!Containers[i].HasTile)
				{
					return true;
				}
			}
			return false;
		}
	}

	public void InstantiateContainers(int nSlots, GameObject tileContainerPrefab)
	{
		_tc = new List<TileContainer>();
		for (int i = 0; i < nSlots; i++)
		{
			InstantiateAndAddContainer(tileContainerPrefab);
		}
	}

	public void InstantiateAndAddContainer(GameObject tileContainerPrefab)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = Object.Instantiate<GameObject>(tileContainerPrefab, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(((Component)this).transform, false);
		TileContainer component = obj.GetComponent<TileContainer>();
		AddContainer(component);
	}

	public void AddContainer(TileContainer tc)
	{
		_tc.Add(tc);
		tc.NotifyTileChanged = this;
		tc.NotifyTileAdded = this;
	}

	public void DestroyContainers()
	{
		foreach (TileContainer item in _tc)
		{
			Object.Destroy((Object)(object)((Component)item).gameObject);
		}
		_tc = new List<TileContainer>();
	}

	public void MoveAllTilesTo(TileContainersCollection tcc)
	{
		foreach (TileContainer container in Containers)
		{
			if (container.HasTile)
			{
				if (tcc.HasEmptyContainer)
				{
					container.MoveTileTo(tcc.FirstEmptyContainer);
				}
				else
				{
					Debug.LogError((object)"MoveAllTilesTo: target container does not have enough free containers");
				}
			}
		}
	}

	public void Defragment()
	{
		for (int i = 0; i < NContainers - 1; i++)
		{
			if (!Containers[i].HasTile && Containers[i + 1].HasTile)
			{
				Containers[i + 1].MoveTileTo(Containers[i]);
			}
		}
	}

	public void SortByAttack()
	{
		Tile[] array = TileSorting.SortByCombat(Tiles);
		foreach (TileContainer container in Containers)
		{
			container.RemoveTile();
		}
		Tile[] array2 = array;
		foreach (Tile tile in array2)
		{
			FirstEmptyContainer.AddTile(tile);
		}
	}

	public void SortByTurnsBeforeCharged()
	{
		_ = NTiles;
		Tile[] array = TileSorting.SortByTurnsBeforeCharged(Tiles);
		foreach (TileContainer container in Containers)
		{
			container.RemoveTile();
		}
		Tile[] array2 = array;
		foreach (Tile tile in array2)
		{
			FirstEmptyContainer.AddTile(tile);
		}
	}

	public void DestroyAllTiles()
	{
		foreach (TileContainer container in Containers)
		{
			if (container.HasTile)
			{
				Tile tile = container.Tile;
				container.RemoveTile();
				Object.Destroy((Object)(object)((Component)tile).gameObject);
			}
		}
	}

	void ITilesChanged.TilesChanged()
	{
		if (NotifyTilesChanged != null)
		{
			NotifyTilesChanged.TilesChanged();
		}
	}

	void ITileAdded.TileAdded(Tile tile)
	{
		if (NotifyTilesAdded != null)
		{
			NotifyTilesAdded.TileAdded(tile);
		}
	}
}
