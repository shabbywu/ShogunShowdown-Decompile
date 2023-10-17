using TilesUtils;
using UINavigation;
using UnityEngine;

public abstract class TileContainer : MonoBehaviour, INavigationTarget
{
	public SpriteRenderer spriteRenderer;

	private Vector3 _currentScaleVelocity;

	private readonly float smoothTimeFast = TilesParameters.translationSmoothTime;

	private readonly float smoothTimeSlow = TilesParameters.translationSmoothTime * 1.4f;

	private float tile_vx;

	private float tile_vy;

	private float tile_vz;

	private float tile_x;

	private float tile_y;

	private float tile_z;

	private bool _interactable = true;

	private ITilesChanged _notifyTileChanged;

	private ITileAdded _notifyTileAdded;

	public Tile Tile { get; private set; }

	public bool HasTile => (Object)(object)Tile != (Object)null;

	public BoxCollider BoxCollider { get; private set; }

	public ITilesChanged NotifyTileChanged
	{
		get
		{
			return _notifyTileChanged;
		}
		set
		{
			_notifyTileChanged = value;
		}
	}

	public ITileAdded NotifyTileAdded
	{
		get
		{
			return _notifyTileAdded;
		}
		set
		{
			_notifyTileAdded = value;
		}
	}

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			_interactable = value;
		}
	}

	public Transform Transform => ((Component)this).transform;

	public abstract bool HandleTileDrop(Tile tile);

	public abstract void UponTileSubmit();

	protected virtual void Awake()
	{
		BoxCollider = ((Component)this).GetComponent<BoxCollider>();
	}

	public void LateUpdate()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		if (!HasTile)
		{
			return;
		}
		((Component)Tile).transform.localScale = Vector3.SmoothDamp(((Component)Tile).transform.localScale, ((Component)this).transform.localScale, ref _currentScaleVelocity, smoothTimeFast);
		if (((Component)Tile).transform.position.y > ((Component)this).transform.position.y)
		{
			tile_x = Mathf.SmoothDamp(((Component)Tile).transform.position.x, ((Component)this).transform.position.x, ref tile_vx, smoothTimeFast);
		}
		else
		{
			tile_x = Mathf.SmoothDamp(((Component)Tile).transform.position.x, ((Component)this).transform.position.x, ref tile_vx, smoothTimeSlow);
		}
		tile_y = Mathf.SmoothDamp(((Component)Tile).transform.position.y, ((Component)this).transform.position.y, ref tile_vy, smoothTimeFast);
		tile_z = Mathf.SmoothDamp(((Component)Tile).transform.position.z, ((Component)this).transform.position.z, ref tile_vz, smoothTimeFast);
		((Component)Tile).transform.position = new Vector3(tile_x, tile_y, tile_z);
		Vector3 localPosition = ((Component)Tile).transform.localPosition;
		if ((double)((Vector3)(ref localPosition)).sqrMagnitude < 0.0001)
		{
			((Component)Tile).transform.localPosition = Vector3.zero;
			if (Tile.TrailEmitting)
			{
				Tile.TrailEmitting = false;
			}
		}
	}

	public virtual void AddTile(Tile tile)
	{
		Tile = tile;
		((Component)Tile).transform.SetParent(((Component)this).transform, true);
		Tile.TileContainer = this;
		if (NotifyTileChanged != null)
		{
			NotifyTileChanged.TilesChanged();
		}
		if (NotifyTileAdded != null)
		{
			NotifyTileAdded.TileAdded(Tile);
		}
		tile_vx = 0f;
		tile_vy = 0f;
		tile_vz = 0f;
	}

	public virtual Tile RemoveTile()
	{
		if ((Object)(object)Tile != (Object)null)
		{
			((Component)Tile).transform.SetParent((Transform)null, true);
			Tile.TileContainer = null;
			Tile.TileContainerOrigin = this;
			Tile.TrailEmitting = true;
		}
		Tile tile = Tile;
		Tile = null;
		if (NotifyTileChanged != null)
		{
			NotifyTileChanged.TilesChanged();
		}
		return tile;
	}

	public virtual void MoveTileTo(TileContainer other)
	{
		if ((Object)(object)Tile == (Object)null)
		{
			Debug.LogError((object)"Nothing to move!");
		}
		Tile tile = Tile;
		RemoveTile();
		other.AddTile(tile);
	}

	public void SwapTiles(TileContainer other)
	{
		if ((Object)(object)Tile == (Object)null || (Object)(object)other.Tile == (Object)null)
		{
			Debug.LogError((object)"SwapTiles error!!");
		}
		Tile tile = Tile;
		Tile tile2 = other.RemoveTile();
		other.AddTile(tile);
		AddTile(tile2);
	}

	public void TeleportTileInContainer()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (HasTile)
		{
			((Component)Tile).transform.position = ((Component)this).transform.position;
			((Component)Tile).transform.rotation = ((Component)this).transform.rotation;
		}
	}

	public virtual void Select()
	{
		if (HasTile)
		{
			Tile.Highlight(value: true);
		}
	}

	public virtual void Deselect()
	{
		if (HasTile)
		{
			Tile.Highlight(value: false);
		}
	}

	public virtual void Submit()
	{
		if (HasTile)
		{
			Tile.Highlight(value: false);
			UponTileSubmit();
		}
	}
}
