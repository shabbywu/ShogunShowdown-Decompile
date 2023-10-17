using UnityEngine;

public class EventTileContainer : TileContainer
{
	private Animator animator;

	public bool allowTileDrop;

	protected override void Awake()
	{
		base.Awake();
		animator = ((Component)this).GetComponent<Animator>();
	}

	public override void UponTileSubmit()
	{
		TilesManager.Instance.TakeTile(RemoveTile());
	}

	public void Highlight(bool value)
	{
		animator.SetBool("Highlighted", value);
	}

	public override bool HandleTileDrop(Tile tile)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (allowTileDrop)
		{
			if (base.HasTile)
			{
				TilesManager.Instance.TakeTile(RemoveTile());
			}
			AddTile(tile);
			EffectsManager.Instance.CreateInGameEffect("TileInteractionEffect", Vector3.Scale(((Component)tile).transform.position, new Vector3(1f, 1f, 0.5f))).GetComponent<TileInteractionEffect>().InitializeInwards(((Component)tile).transform);
			return true;
		}
		TilesManager.Instance.TakeTile(tile);
		return false;
	}
}
