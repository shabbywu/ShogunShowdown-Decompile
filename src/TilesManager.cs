using System.Collections.Generic;
using System.Linq;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;

public class TilesManager : MonoBehaviour
{
	public TileContainer EventTargetContainer;

	public Hand hand;

	private bool canInteractWithTiles;

	public static TilesManager Instance { get; protected set; }

	public bool CanInteractWithTiles
	{
		get
		{
			return canInteractWithTiles;
		}
		set
		{
			bool flag = canInteractWithTiles;
			canInteractWithTiles = value;
			if (DraggingTile && !canInteractWithTiles)
			{
				TakeTile(TileBeingDragged);
			}
			foreach (Tile item in Deck)
			{
				item.Interactable = value;
			}
			if (!flag && canInteractWithTiles)
			{
				EventsManager.Instance.TileInteractionEnabled.Invoke();
			}
			else if (flag && !canInteractWithTiles)
			{
				EventsManager.Instance.TileInteractionDisabled.Invoke();
			}
		}
	}

	public List<Tile> Deck { get; set; } = new List<Tile>();


	public List<AttackEnum> DeckAttackEnums => Deck.ConvertAll((Tile t) => t.Attack.AttackEnum);

	public Tile TileBeingDragged { get; set; }

	public Tile TileBeingPointed { get; set; }

	public bool CombatTilePlayingPhase
	{
		get
		{
			if (CombatManager.Instance.CombatInProgress)
			{
				return !CombatManager.Instance.TurnInProgress;
			}
			return false;
		}
	}

	public bool DraggingTile => (Object)(object)TileBeingDragged != (Object)null;

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombat.AddListener(new UnityAction(EndOfCombat));
		EventsManager.Instance.EnterRoom.AddListener((UnityAction<Room>)EnterNewRoom);
	}

	private void Update()
	{
		if (!CanInteractWithTiles || !CombatTilePlayingPhase)
		{
			return;
		}
		if ((Object)(object)TileBeingDragged != (Object)null)
		{
			Globals.Hero.AttackQueue.RearrangeTilesForDraggedTile(TileBeingDragged);
			AttackQueueTileContainer[] containers = Globals.Hero.AttackQueue.containers;
			foreach (AttackQueueTileContainer obj in containers)
			{
				obj.DropTargetHighlight((Object)(object)obj == (Object)(object)TileBeingDragged.TargetTileContainer);
			}
		}
		if ((Object)(object)TileBeingPointed != (Object)null && (Object)(object)EventTargetContainer == (Object)null && !(TileBeingPointed.TileContainer is AttackQueueTileContainer) && Globals.Hero.AttackQueue.NActiveContainers == Globals.Hero.AttackQueue.NTiles)
		{
			Globals.Hero.AttackQueue.SetNextTargetContainerActive(value: true);
		}
	}

	public void InitializeDeck(List<Tile> tiles)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		hand.Initialize(tiles.Count);
		hand.Resize(tiles.Count);
		foreach (Tile tile in tiles)
		{
			TakeTile(tile);
			((Component)tile).transform.localPosition = new Vector3(0f, -6f, 0f);
		}
		ShowTilesLevel(value: false);
	}

	public void TakeTile(Tile tile, bool putInClosestHandContainer = false)
	{
		if (!Deck.Contains(tile))
		{
			AddTileToDeck(tile);
		}
		if ((Object)(object)tile.TileContainer != (Object)null)
		{
			tile.TileContainer.RemoveTile();
		}
		if (hand.TCC.NTiles == hand.TCC.NContainers)
		{
			hand.Resize(hand.TCC.NContainers + 1);
		}
		hand.AddTile(tile, putInClosestHandContainer);
	}

	public void TileDropped(Tile tile)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		TileBeingDragged = null;
		bool flag = false;
		if (!CanInteractWithTiles)
		{
			flag = true;
		}
		if (CombatManager.Instance.TurnInProgress && tile.TileContainerOrigin is HandTileContainer)
		{
			flag = true;
		}
		if (flag)
		{
			if (Globals.Hero.AttackQueue.containers.Contains(tile.TargetTileContainer))
			{
				Globals.Hero.AttackQueue.DefragmentAndResize();
			}
			TakeTile(tile, putInClosestHandContainer: true);
			return;
		}
		if ((Object)(object)tile.TargetTileContainer != (Object)null)
		{
			EffectsManager.Instance.CreateInGameEffect("TileInteractionEffect", Vector3.Scale(((Component)tile).transform.position, new Vector3(1f, 1f, 0.5f))).GetComponent<TileInteractionEffect>().InitializeInwards(((Component)tile).transform);
		}
		if ((Object)(object)tile.TargetTileContainer == (Object)null || !tile.TargetTileContainer.Interactable || !((Component)tile.TargetTileContainer).gameObject.activeSelf)
		{
			if (hand.CanAddTile)
			{
				TakeTile(tile, putInClosestHandContainer: true);
				SoundEffectsManager.Instance.Play("TileSubmit");
			}
			else
			{
				tile.TileContainerOrigin.AddTile(tile);
			}
		}
		else
		{
			tile.TargetTileContainer.HandleTileDrop(tile);
		}
		if (tile.TileContainerOrigin is AttackQueueTileContainer && !(tile.TileContainer is AttackQueueTileContainer))
		{
			Globals.Hero.AttackQueue.DefragmentAndResize();
		}
	}

	public void RechargeCooldownForDeck()
	{
		foreach (Tile item in Deck)
		{
			item.CooldownCharge = item.Attack.Cooldown;
			if (CanInteractWithTiles)
			{
				item.Interactable = true;
			}
		}
	}

	public void OnTilePointerEnter(Tile tile)
	{
		TileBeingPointed = tile;
	}

	public void OnTilePointerExit(Tile tile)
	{
		if ((Object)(object)TileBeingPointed == (Object)(object)tile)
		{
			TileBeingPointed = null;
		}
		if ((Object)(object)TileBeingDragged == (Object)null && CombatManager.Instance.CombatInProgress)
		{
			Globals.Hero.AttackQueue.SetNextTargetContainerActive(value: false);
		}
	}

	public void ShowTilesLevel(bool value)
	{
		foreach (Tile item in Deck)
		{
			item.Graphics.ShowLevel(value);
		}
	}

	private void EndOfCombat()
	{
		CanInteractWithTiles = false;
		RechargeCooldownForDeck();
		if ((Object)(object)TileBeingDragged != (Object)null)
		{
			TileBeingDragged.Dropped();
		}
		foreach (Tile item in Deck)
		{
			item.TileIsEnabled = true;
		}
	}

	private void AddTileToDeck(Tile tile)
	{
		Deck.Add(tile);
		tile.SetUpListenersForDeckTile();
	}

	private void EnterNewRoom(Room room)
	{
		if (room is CombatRoom)
		{
			ShowTilesLevel(value: false);
		}
	}
}
