public class SimpleTileContainer : TileContainer
{
	public override void UponTileSubmit()
	{
	}

	public override bool HandleTileDrop(Tile tile)
	{
		return false;
	}
}
