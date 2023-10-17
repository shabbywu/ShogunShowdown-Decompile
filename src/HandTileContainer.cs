using System.Collections;
using Parameters;
using UnityEngine;

public class HandTileContainer : TileContainer
{
	public GameObject inactiveTileSelector;

	public override void UponTileSubmit()
	{
		if (!TilesManager.Instance.CanInteractWithTiles || !base.Tile.TileIsEnabled)
		{
			return;
		}
		if (CombatSceneManager.Instance.CurrentMode == CombatSceneManager.Mode.combat)
		{
			if (CombatManager.Instance.TurnInProgress)
			{
				((MonoBehaviour)this).StartCoroutine(StickyTryMovingTileToHeroAttackQueue());
			}
			else
			{
				TryMovingTileToHeroAttackQueue();
			}
		}
		else if (CombatSceneManager.Instance.CurrentMode == CombatSceneManager.Mode.reward)
		{
			TryMovingTileToEventTargetContainer();
		}
	}

	public override bool HandleTileDrop(Tile tile)
	{
		if (TilesManager.Instance.hand.CanAddTile)
		{
			TilesManager.Instance.TakeTile(tile);
		}
		else
		{
			tile.TileContainerOrigin.AddTile(tile);
		}
		return false;
	}

	private IEnumerator StickyTryMovingTileToHeroAttackQueue()
	{
		float t = GameParams.stickyInputTime;
		while (t > 0f)
		{
			if (CombatManager.Instance.TurnInProgress)
			{
				t -= Time.deltaTime;
				yield return null;
				continue;
			}
			TryMovingTileToHeroAttackQueue();
			break;
		}
	}

	private void TryMovingTileToEventTargetContainer()
	{
		TileContainer eventTargetContainer = TilesManager.Instance.EventTargetContainer;
		if ((Object)(object)eventTargetContainer != (Object)null)
		{
			if (eventTargetContainer.HasTile)
			{
				TilesManager.Instance.TakeTile(eventTargetContainer.RemoveTile());
			}
			eventTargetContainer.AddTile(RemoveTile());
		}
	}

	private void TryMovingTileToHeroAttackQueue()
	{
		if (!CombatManager.Instance.CombatInProgress || (Object)(object)base.Tile == (Object)null)
		{
			return;
		}
		AttackQueue attackQueue = Globals.Hero.AttackQueue;
		if (attackQueue.CanAddTile)
		{
			if (attackQueue.NTiles == attackQueue.NActiveContainers)
			{
				attackQueue.NActiveContainers++;
			}
			base.Tile.GoToContainer(attackQueue.TCC.FirstEmptyContainer);
			SoundEffectsManager.Instance.Play("TileSubmit");
		}
	}

	public override void Select()
	{
		if (base.HasTile && base.Tile.Interactable)
		{
			base.Select();
			inactiveTileSelector.SetActive(false);
		}
		else
		{
			inactiveTileSelector.SetActive(true);
		}
		if (base.HasTile && Globals.GamepadTilesInfoMode)
		{
			base.Tile.InfoBoxActivator.Open();
		}
	}

	public override void Deselect()
	{
		inactiveTileSelector.SetActive(false);
		if (base.HasTile)
		{
			base.Deselect();
		}
		if (base.HasTile && Globals.GamepadTilesInfoMode)
		{
			base.Tile.InfoBoxActivator.Close();
		}
	}
}
