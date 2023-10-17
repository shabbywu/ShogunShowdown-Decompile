using System.Collections;
using InfoBoxUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBoxActivator : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public InfoBox.PositioningEnum positioning;

	public GameObject infoBoxPrefab;

	public Transform infoBoxTarget;

	protected InfoBox infoBox;

	protected IInfoBoxable infoBoxable;

	private bool pointerOnMe;

	private readonly float openTimeDelay;

	private BoxCollider boxCollider;

	public bool ForceOpen { get; set; }

	protected virtual bool IsEnabled => infoBoxable.InfoBoxEnabled;

	public bool ColliderEnabled
	{
		get
		{
			return ((Collider)boxCollider).enabled;
		}
		set
		{
			((Collider)boxCollider).enabled = value;
		}
	}

	public bool InfoBoxIsOpen
	{
		get
		{
			if ((Object)(object)infoBox != (Object)null)
			{
				return infoBox.IsOpen;
			}
			return false;
		}
	}

	protected virtual void Awake()
	{
		infoBoxable = ((Component)this).GetComponentInParent<IInfoBoxable>();
		boxCollider = ((Component)this).GetComponent<BoxCollider>();
	}

	public void Open()
	{
		if ((Object)(object)infoBox != (Object)null && !ForceOpen)
		{
			infoBox.Close();
		}
		SpawnAndOpen();
	}

	public void Close()
	{
		if ((Object)(object)infoBox != (Object)null && !ForceOpen)
		{
			infoBox.Close();
		}
	}

	public virtual void OnPointerEnter(PointerEventData pointerEventData)
	{
		if ((Object)(object)infoBox != (Object)null)
		{
			infoBox.Close();
		}
		pointerOnMe = true;
		if (IsEnabled)
		{
			((MonoBehaviour)this).StartCoroutine(ActivateInfoBoxCoroutine());
		}
	}

	public virtual void OnPointerExit(PointerEventData pointerEventData)
	{
		if ((Object)(object)infoBox != (Object)null && !ForceOpen)
		{
			infoBox.Close();
		}
		pointerOnMe = false;
	}

	private void SpawnAndOpen()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(infoBoxPrefab, ((Component)this).transform);
		infoBox = val.GetComponent<InfoBox>();
		infoBox.SetPosition(positioning, ((Component)infoBoxTarget).transform);
		infoBox.SetText(infoBoxable.InfoBoxText);
		infoBox.SetBoxWidth(infoBoxable.BoxWidth);
		if (infoBoxable.MaxWidth > 0)
		{
			infoBox.SetMaxWidth(infoBoxable.MaxWidth);
		}
		infoBox.AdjustIfOutOfScreen();
		((Component)infoBox).transform.localScale = new Vector3(0f, 0f, 1f);
		infoBox.Open();
	}

	private IEnumerator ActivateInfoBoxCoroutine()
	{
		float countdownTime = openTimeDelay;
		while (pointerOnMe)
		{
			countdownTime -= Time.deltaTime;
			if (countdownTime < 0f)
			{
				if (IsEnabled)
				{
					SpawnAndOpen();
				}
				break;
			}
			yield return null;
		}
	}
}
