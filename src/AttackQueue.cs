using System;
using System.Collections.Generic;
using TilesUtils;
using UnityEngine;

public abstract class AttackQueue : MonoBehaviour, ITileAdded
{
	public static int maxContainers = 3;

	private int nActiveContainers;

	public AttackQueueTileContainer[] containers;

	public AttackQueueGraphics Graphics { get; private set; }

	public TileContainersCollection TCC { get; private set; }

	public string ContentDescription
	{
		get
		{
			string text = "";
			Tile[] tiles = TCC.Tiles;
			foreach (Tile tile in tiles)
			{
				text = text + tile.Attack.TechNameAndStats + " + ";
			}
			char[] trimChars = new char[2] { ' ', '+' };
			return text.TrimEnd(trimChars);
		}
	}

	public virtual bool CanAddTile => NTiles < maxContainers;

	public int NTiles => TCC.NTiles;

	public int NActiveContainers
	{
		get
		{
			return nActiveContainers;
		}
		set
		{
			value = Mathf.Clamp(value, 0, maxContainers);
			if (nActiveContainers != value)
			{
				nActiveContainers = value;
				for (int i = 0; i < maxContainers; i++)
				{
					((Component)containers[i]).gameObject.SetActive(i < nActiveContainers);
				}
				Graphics.SetNumberOfContainers(nActiveContainers);
			}
		}
	}

	public Attack[] Attacks
	{
		get
		{
			List<Attack> list = new List<Attack>();
			Tile[] tiles = TCC.Tiles;
			foreach (Tile tile in tiles)
			{
				list.Add(tile.Attack);
			}
			return list.ToArray();
		}
	}

	public bool HasOffensiveAttack
	{
		get
		{
			Tile[] tiles = TCC.Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				if (tiles[i].Attack.HasValue)
				{
					return true;
				}
			}
			return false;
		}
	}

	public abstract void TilesWerePlayed();

	protected virtual void Awake()
	{
		Graphics = ((Component)this).GetComponentInChildren<AttackQueueGraphics>();
		TCC = ((Component)this).GetComponentInChildren<TileContainersCollection>();
		AttackQueueTileContainer[] array = containers;
		foreach (AttackQueueTileContainer tc in array)
		{
			TCC.AddContainer(tc);
		}
		TCC.NotifyTilesAdded = this;
		NActiveContainers = 0;
	}

	public virtual void BeginAttack()
	{
		NActiveContainers = NTiles;
		Tile[] tiles = TCC.Tiles;
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].Interactable = false;
		}
	}

	public virtual void TileAdded(Tile tile)
	{
	}

	public void RearrangeTilesForDraggedTile(Tile tile)
	{
		if (NTiles == 0)
		{
			return;
		}
		bool flag = false;
		AttackQueueTileContainer attackQueueTileContainer = null;
		if ((Object)(object)tile.TargetTileContainer != (Object)null && tile.TargetTileContainer is AttackQueueTileContainer)
		{
			attackQueueTileContainer = (AttackQueueTileContainer)tile.TargetTileContainer;
			if (attackQueueTileContainer.isHero)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			TCC.Defragment();
		}
		else if (!(tile.Speed > 1f))
		{
			RearrangeTilesToFreeUpContainer(attackQueueTileContainer);
		}
	}

	public bool RearrangeTilesToFreeUpContainer(AttackQueueTileContainer target)
	{
		if ((Object)(object)target.Tile == (Object)null)
		{
			return true;
		}
		if (NTiles == maxContainers)
		{
			return false;
		}
		int num = Array.IndexOf(containers, target);
		int num2 = 0;
		while ((Object)(object)target.Tile != (Object)null)
		{
			num2++;
			if (num2 > 100)
			{
				Debug.LogWarning((object)"RearrangeTilesToFreeUpContainer: Max bug!");
				return false;
			}
			for (int i = 1; i <= num; i++)
			{
				if ((Object)(object)containers[i - 1].Tile == (Object)null && (Object)(object)containers[i].Tile != (Object)null)
				{
					containers[i].Tile.GoToContainer(containers[i - 1]);
				}
			}
			for (int num3 = NActiveContainers - 1; num3 > num; num3--)
			{
				if ((Object)(object)containers[num3].Tile == (Object)null && (Object)(object)containers[num3 - 1].Tile != (Object)null)
				{
					containers[num3 - 1].Tile.GoToContainer(containers[num3]);
				}
			}
		}
		return true;
	}

	public void SetNextTargetContainerActive(bool value)
	{
		if (value)
		{
			NActiveContainers = NTiles + 1;
		}
		else
		{
			NActiveContainers = NTiles;
		}
	}

	public void TileInContainer_0_Played()
	{
		Tile tile = containers[0].RemoveTile();
		((Component)tile).transform.SetParent(((Component)this).transform, true);
		((Component)tile.Graphics).gameObject.SetActive(false);
		DefragmentAndResize();
	}

	public void DefragmentAndResize()
	{
		TCC.Defragment();
		NActiveContainers = NTiles;
	}

	public virtual void Hide()
	{
		NActiveContainers = 0;
	}

	public void AddTile(Tile tile)
	{
		if (!TCC.HasEmptyContainer)
		{
			Debug.LogError((object)"Calling AddTile on AttackQueue, but there are no empty containers.");
		}
		TCC.FirstEmptyContainer.AddTile(tile);
	}

	public void DestroyAllTiles()
	{
		Tile[] componentsInChildren = ((Component)this).GetComponentsInChildren<Tile>();
		foreach (Tile tile in componentsInChildren)
		{
			if ((Object)(object)tile.TileContainer != (Object)null)
			{
				tile.TileContainer.RemoveTile();
			}
			Object.Destroy((Object)(object)((Component)tile).gameObject);
		}
	}
}
