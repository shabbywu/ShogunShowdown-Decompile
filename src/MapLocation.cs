using InfoBoxUtils;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapLocation : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IInfoBoxable, INavigationTarget
{
	public Location location;

	public string ID;

	public SpriteRenderer symbolSpriteRenderer_C;

	public SpriteRenderer symbolSpriteRenderer_L;

	public SpriteRenderer symbolSpriteRenderer_R;

	public MapUndiscoveredCloud undiscoveredCloud;

	public int uncoverDepth;

	public float cameraTargetY;

	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	public Transform focusPointIn;

	public Transform focusPointOut;

	private Map map;

	private bool reachable;

	private bool cleared;

	private bool selected;

	private bool highlighted;

	private bool uncovered;

	private Animator animator;

	public bool InitiallyUncovered { get; set; }

	public bool Reachable
	{
		get
		{
			return reachable;
		}
		set
		{
			reachable = value;
			UpdateState();
		}
	}

	public bool Cleared
	{
		get
		{
			return cleared;
		}
		set
		{
			if (value)
			{
				animator.SetTrigger("Cleared");
			}
			cleared = value;
			if (cleared)
			{
				selected = false;
			}
			UpdateState();
		}
	}

	public Transform FocusPoint
	{
		get
		{
			if (cleared)
			{
				return focusPointOut;
			}
			return focusPointIn;
		}
	}

	public bool Highlight
	{
		set
		{
			highlighted = value;
			UpdateState();
		}
	}

	public bool Uncovered => uncovered;

	public string InfoBoxText => location.Name;

	public bool InfoBoxEnabled
	{
		get
		{
			if (uncovered && IsInsideVisibleArea && MapManager.Instance.Interactable && !MapManager.Instance.map.MovingInProgress)
			{
				return !MapManager.Instance.mapScreen.IsInTransition;
			}
			return false;
		}
	}

	public bool IsInsideVisibleArea => MapManager.Instance.mapScreen.IsInsideVisibleArea(((Component)this).transform.position);

	public BoxWidth BoxWidth => BoxWidth.auto;

	public Transform Transform => ((Component)this).transform;

	public void Initialize()
	{
		uncovered = InitiallyUncovered;
		if (!uncovered)
		{
			((Component)undiscoveredCloud).gameObject.SetActive(true);
		}
	}

	public void Uncover()
	{
		if (!uncovered)
		{
			undiscoveredCloud.Discover();
			uncovered = true;
			UpdateState();
		}
	}

	private void Awake()
	{
		map = ((Component)this).GetComponentInParent<Map>();
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Select();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Deselect();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Submit();
	}

	public void OnEnable()
	{
		infoBoxActivator.ForceOpen = false;
	}

	private void UpdateState()
	{
		((Component)infoBoxActivator).gameObject.SetActive(uncovered);
		animator.SetBool("Reachable", reachable);
		animator.SetBool("Highlighted", highlighted);
		if (location is ShopLocation shopLocation)
		{
			((Renderer)symbolSpriteRenderer_C).enabled = false;
			((Renderer)symbolSpriteRenderer_L).enabled = true;
			((Renderer)symbolSpriteRenderer_R).enabled = true;
			if (cleared)
			{
				symbolSpriteRenderer_L.sprite = shopLocation.leftShopComponent.mapIconCleared;
			}
			else if (!reachable)
			{
				symbolSpriteRenderer_L.sprite = shopLocation.leftShopComponent.mapIconUnreachable;
			}
			else if (highlighted || selected)
			{
				symbolSpriteRenderer_L.sprite = shopLocation.leftShopComponent.mapIconHighlighted;
			}
			else
			{
				symbolSpriteRenderer_L.sprite = shopLocation.leftShopComponent.mapIconReachable;
			}
			if (cleared)
			{
				symbolSpriteRenderer_R.sprite = shopLocation.rightShopComponent.mapIconCleared;
			}
			else if (!reachable)
			{
				symbolSpriteRenderer_R.sprite = shopLocation.rightShopComponent.mapIconUnreachable;
			}
			else if (highlighted || selected)
			{
				symbolSpriteRenderer_R.sprite = shopLocation.rightShopComponent.mapIconHighlighted;
			}
			else
			{
				symbolSpriteRenderer_R.sprite = shopLocation.rightShopComponent.mapIconReachable;
			}
		}
		else
		{
			((Renderer)symbolSpriteRenderer_C).enabled = true;
			((Renderer)symbolSpriteRenderer_L).enabled = false;
			((Renderer)symbolSpriteRenderer_R).enabled = false;
			if (cleared)
			{
				symbolSpriteRenderer_C.sprite = location.mapIconCleared;
			}
			else if (!reachable)
			{
				symbolSpriteRenderer_C.sprite = location.mapIconUnreachable;
			}
			else if (highlighted || selected)
			{
				symbolSpriteRenderer_C.sprite = location.mapIconHighlighted;
			}
			else
			{
				symbolSpriteRenderer_C.sprite = location.mapIconReachable;
			}
		}
	}

	public void Select()
	{
		if (uncovered && IsInsideVisibleArea)
		{
			if (!infoBoxActivator.InfoBoxIsOpen)
			{
				infoBoxActivator.Open();
			}
			if (Reachable)
			{
				map.HighlightLocation(this);
				SoundEffectsManager.Instance.Play("MenuItemHighlight");
			}
		}
	}

	public void Deselect()
	{
		if (uncovered && IsInsideVisibleArea)
		{
			if (infoBoxActivator.InfoBoxIsOpen)
			{
				infoBoxActivator.ForceOpen = false;
				infoBoxActivator.Close();
			}
			if (!selected)
			{
				infoBoxActivator.Close();
				map.DeHighlightLocation(this);
			}
		}
	}

	public void Submit()
	{
		if (Reachable && uncovered && !selected)
		{
			selected = map.SelectLocation(this);
			animator.SetTrigger("Selected");
			infoBoxActivator.ForceOpen = true;
		}
	}
}
