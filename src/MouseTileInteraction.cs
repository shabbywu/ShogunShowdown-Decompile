using TilesUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;

public class MouseTileInteraction : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
	private Tile tile;

	private Vector3 dragDelta;

	private Vector3 previousPosition;

	private Camera mainCamera;

	private bool previousUpdatePointerOnMe;

	private readonly float dragDeltaMaxComponent = 0.35f;

	private readonly float pointerOnMeMaxDistanceFromContainer = 0.3f;

	public bool Dragging { get; private set; }

	public bool PointerOnMe { get; set; }

	public float Speed { get; private set; }

	private Vector3 DraggingPointerPosition => mainCamera.ScreenToWorldPoint(new Vector3(((InputControl<float>)(object)((Pointer)Mouse.current).position.x).ReadValue(), ((InputControl<float>)(object)((Pointer)Mouse.current).position.y).ReadValue(), 0f));

	private void Awake()
	{
		tile = ((Component)this).GetComponent<Tile>();
	}

	private void Start()
	{
		mainCamera = Camera.main;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (!Dragging && TilesManager.Instance.CanInteractWithTiles && (int)eventData.button == 0)
		{
			tile.LeftClicked();
		}
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (TilesManager.Instance.CanInteractWithTiles && (int)eventData.button == 0)
		{
			dragDelta = ((Component)this).transform.position - DraggingPointerPosition + TilesParameters.forwardHighlight * Vector3.back;
			dragDelta = new Vector3(Mathf.Clamp(dragDelta.x, 0f - dragDeltaMaxComponent, dragDeltaMaxComponent), Mathf.Clamp(dragDelta.y, 0f - dragDeltaMaxComponent, dragDeltaMaxComponent), dragDelta.z);
			tile.BeginDragging();
			Dragging = true;
		}
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Dragging && (int)eventData.button == 0)
		{
			Dragging = false;
			tile.Dropped();
		}
	}

	private void OnDisable()
	{
		previousUpdatePointerOnMe = false;
		PointerOnMe = false;
		Dragging = false;
	}

	private void LateUpdate()
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		if (Dragging)
		{
			PointerOnMe = true;
		}
		if (PointerOnMe)
		{
			if (TilesManager.Instance.DraggingTile && (Object)(object)TilesManager.Instance.TileBeingDragged.mouseTileInteraction != (Object)(object)this)
			{
				PointerOnMe = false;
			}
			if ((Object)(object)tile.TileContainer != (Object)null && Vector3.Distance(((Component)tile).transform.position, ((Component)tile.TileContainer).transform.position) > pointerOnMeMaxDistanceFromContainer)
			{
				PointerOnMe = false;
			}
		}
		if (Dragging && TilesManager.Instance.CanInteractWithTiles)
		{
			((Component)this).transform.position = DraggingPointerPosition + dragDelta;
			((Component)this).transform.position = PixelUtils.PixelPerfectClamp(((Component)this).transform.position);
		}
		Vector3 val = ((Component)this).transform.position - previousPosition;
		Speed = ((Vector3)(ref val)).magnitude / Time.deltaTime;
		if (!previousUpdatePointerOnMe && PointerOnMe)
		{
			tile.OnPointerEnter();
		}
		else if (previousUpdatePointerOnMe && !PointerOnMe)
		{
			tile.OnPointerExit();
		}
		previousUpdatePointerOnMe = PointerOnMe;
		PointerOnMe = false;
		previousPosition = ((Component)this).transform.position;
	}
}
