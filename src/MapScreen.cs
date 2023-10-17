using System.Collections;
using UnityEngine;
using Utils;

public class MapScreen : MonoBehaviour
{
	public enum TransitionStateEnum
	{
		idle,
		opening,
		closing
	}

	public enum MapStateEnum
	{
		closed,
		opening,
		closing,
		openConsult,
		openSelect,
		transitioning
	}

	public Map map;

	public MapCloseButton mapCloseButton;

	public RectTransform visibleArea;

	public GameObject container;

	[SerializeField]
	private GameObject blockingLayer;

	[SerializeField]
	private SpriteRendererFading backgroundDimMask;

	private float openCloseSpeed = 20f;

	private float panSpeed = 4f;

	private Vector3 mapPosition;

	private static Vector3 forwardShift = -2f * Vector3.forward;

	private Vector3 closedMapPosition = 8f * Vector3.down + forwardShift;

	private Vector3 openMapPosition = new Vector3(0f, 0f, 0f) + forwardShift;

	private Vector3 mapPanVelocity = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	private IEnumerator currentCoroutine;

	private TransitionStateEnum transitionState;

	private Color DarkBackgroundColor = new Color(0f, 0f, 0f, 0.3f);

	public bool IsInTransition => transitionState != TransitionStateEnum.idle;

	public bool IsOpen
	{
		get
		{
			if (container.activeSelf)
			{
				return !IsInTransition;
			}
			return false;
		}
	}

	public bool CameraIsPanning => Vector3.SqrMagnitude(mapPosition - TargetMapPosition) > 0.1f;

	public Vector3 PanShift { get; set; }

	private Vector3 TargetMapPosition
	{
		get
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)map.CurrentMovingToMapLocation != (Object)null)
			{
				return -map.CurrentMovingToMapLocation.FocusPoint.localPosition;
			}
			return -map.CurrentMapLocation.FocusPoint.localPosition;
		}
	}

	private void Update()
	{
		UpdateMapPosition();
	}

	private void UpdateMapPosition()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		mapPosition = Vector3.MoveTowards(mapPosition, TargetMapPosition, panSpeed * Time.deltaTime);
		((Component)map).transform.localPosition = PixelUtils.PixelPerfectClamp(mapPosition);
	}

	public void Activate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		((Component)map).transform.localPosition = TargetMapPosition;
		mapPosition = TargetMapPosition;
		((Behaviour)this).enabled = true;
		container.SetActive(true);
		blockingLayer.SetActive(true);
		backgroundDimMask.SetVisible(value: true);
		EventsManager.Instance.MapActivated.Invoke();
	}

	private void Deactivate()
	{
		((Behaviour)this).enabled = false;
		container.SetActive(false);
		blockingLayer.SetActive(false);
		EventsManager.Instance.MapDeactivated.Invoke();
	}

	public void CloseMap()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (transitionState != TransitionStateEnum.closing)
		{
			if (currentCoroutine != null)
			{
				((MonoBehaviour)this).StopCoroutine(currentCoroutine);
			}
			currentCoroutine = CloseMapCoroutine();
			((MonoBehaviour)this).StartCoroutine(currentCoroutine);
			PanShift = Vector3.zero;
			backgroundDimMask.SetVisible(value: false);
			EventsManager.Instance.MapClosed.Invoke();
		}
	}

	public void OpenMap()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = new Vector3(((Component)Camera.main).transform.position.x, ((Component)Camera.main).transform.position.y, 0f);
		if (transitionState != TransitionStateEnum.opening)
		{
			if (currentCoroutine != null)
			{
				((MonoBehaviour)this).StopCoroutine(currentCoroutine);
			}
			currentCoroutine = OpenMapCoroutine();
			((MonoBehaviour)this).StartCoroutine(currentCoroutine);
			((Component)map).transform.localPosition = TargetMapPosition;
			((Component)mapCloseButton).gameObject.SetActive(!map.LocationSelectionMode);
			PanShift = Vector3.zero;
			EventsManager.Instance.MapOpened.Invoke();
		}
	}

	private IEnumerator OpenMapCoroutine()
	{
		transitionState = TransitionStateEnum.opening;
		SoundEffectsManager.Instance.Play("OpenCloseMap");
		container.transform.localPosition = closedMapPosition;
		while (Vector3.Distance(container.transform.localPosition, openMapPosition) > 0.001f)
		{
			container.transform.localPosition = Vector3.MoveTowards(container.transform.localPosition, openMapPosition, openCloseSpeed * Time.deltaTime);
			yield return null;
		}
		container.transform.localPosition = openMapPosition;
		transitionState = TransitionStateEnum.idle;
		currentCoroutine = null;
	}

	private IEnumerator CloseMapCoroutine()
	{
		transitionState = TransitionStateEnum.closing;
		while (Vector3.Distance(container.transform.localPosition, closedMapPosition) > 0.001f)
		{
			container.transform.localPosition = Vector3.MoveTowards(container.transform.localPosition, closedMapPosition, openCloseSpeed * Time.deltaTime);
			yield return null;
		}
		transitionState = TransitionStateEnum.idle;
		currentCoroutine = null;
		Deactivate();
	}

	public bool IsInsideVisibleArea(Vector3 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Abs(((Transform)visibleArea).position.x - position.x);
		Rect rect = visibleArea.rect;
		if (num < ((Rect)(ref rect)).width / 2f)
		{
			float num2 = Mathf.Abs(((Transform)visibleArea).position.y - position.y);
			rect = visibleArea.rect;
			return num2 < ((Rect)(ref rect)).height / 2f;
		}
		return false;
	}
}
