public class EnemyAttackQueue : AttackQueue
{
	protected override void Awake()
	{
		base.Awake();
		foreach (TileContainer container in base.TCC.Containers)
		{
			container.Interactable = false;
		}
	}

	public override void TilesWerePlayed()
	{
		Clear();
	}

	public void Clear()
	{
		DestroyAllTiles();
		Hide();
		base.Graphics.Idle();
	}

	public void AboutToPlayTile()
	{
		base.NActiveContainers++;
		((AttackQueueTileContainer)base.TCC.FirstEmptyContainer).AboutToPlayTile();
	}
}
