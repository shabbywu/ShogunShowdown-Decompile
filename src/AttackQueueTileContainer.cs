using UnityEngine;

public class AttackQueueTileContainer : TileContainer
{
	private Animator animator;

	private AttackQueue attackQueue;

	public bool isHero;

	private bool Highlight { get; set; }

	protected override void Awake()
	{
		base.Awake();
		animator = ((Component)this).GetComponent<Animator>();
		attackQueue = ((Component)this).GetComponentInParent<AttackQueue>();
	}

	public void DropTargetHighlight(bool value)
	{
		if (!((Object)(object)animator == (Object)null))
		{
			animator.SetBool("DropTargetHighlight", value);
			Highlight = value;
		}
	}

	public override void AddTile(Tile tile)
	{
		base.AddTile(tile);
		animator.SetBool("TileAboutToBePlayed", false);
		animator.SetBool("DropTargetHighlight", false);
	}

	public void AboutToPlayTile()
	{
		animator.SetBool("TileAboutToBePlayed", true);
	}

	public override void UponTileSubmit()
	{
		if (base.Tile.TileIsEnabled)
		{
			base.Tile.GoToContainer(TilesManager.Instance.hand.TCC.FirstEmptyContainer);
			attackQueue.DefragmentAndResize();
			SoundEffectsManager.Instance.Play("TileSubmit");
		}
	}

	public override bool HandleTileDrop(Tile tile)
	{
		if (isHero && Globals.Hero.AttackQueue.CanAddTile)
		{
			if (Globals.Hero.AttackQueue.RearrangeTilesToFreeUpContainer(this))
			{
				AddTile(tile);
				SoundEffectsManager.Instance.Play("TileDrag");
			}
			else
			{
				TilesManager.Instance.TakeTile(tile);
			}
		}
		else
		{
			TilesManager.Instance.TakeTile(tile);
		}
		return true;
	}
}
