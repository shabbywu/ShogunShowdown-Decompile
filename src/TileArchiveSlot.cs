using UINavigation;
using UnityEngine;

public class TileArchiveSlot : MonoBehaviour, INavigationTarget
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite highlightedSprite;

	private Tile Tile { get; set; }

	public Transform Transform => ((Component)this).transform;

	public void AttachTile(Tile tile)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Tile = tile;
		((Component)Tile).transform.parent = ((Component)this).transform;
		((Component)Tile).transform.localPosition = Vector3.zero;
		Tile.Interactable = false;
		Tile.TrailEmitting = false;
	}

	public virtual void Select()
	{
		Tile?.InfoBoxActivator.Open();
		spriteRenderer.sprite = highlightedSprite;
	}

	public virtual void Deselect()
	{
		Tile?.InfoBoxActivator.Close();
		spriteRenderer.sprite = normalSprite;
	}

	public virtual void Submit()
	{
	}
}
