using UnityEngine;

public class AttackQueueKeysControlsInfo : MonoBehaviour
{
	public SpriteRenderer upSprite;

	public SpriteRenderer takeSprite;

	public SpriteRenderer downSprite;

	private Tile Tile { get; set; }

	public void Initialize(Tile tile)
	{
		Tile = tile;
		UpdateState();
	}

	private void Update()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		if (!(Tile.TileContainer is AttackQueueTileContainer))
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		int num = Globals.Hero.AttackQueue.TCC.Containers.IndexOf(Tile.TileContainer);
		((Renderer)takeSprite).enabled = true;
		((Renderer)upSprite).enabled = num < Globals.Hero.AttackQueue.NTiles - 1;
		((Renderer)downSprite).enabled = num > 0;
	}
}
