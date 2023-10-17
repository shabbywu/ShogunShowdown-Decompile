using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnlocksID;

public class MapConnection : MonoBehaviour
{
	[Header("Connecting locations:")]
	public MapLocation start;

	public MapLocation end;

	public UnlockID[] requiredUnlocks;

	public Transform[] waypoints;

	[SerializeField]
	private bool forceDisabled;

	[Header("Sprites:")]
	public Sprite highlighted;

	private Sprite normal;

	private static Color inactiveColor = new Color(0.7f, 0.7f, 0.7f);

	private static Color activeColor = Color.white;

	private static float walkingSpeed = 1.5f;

	private int defaultSpriteRendererSortingOrder;

	private bool walkable;

	private bool highlight;

	private SpriteRenderer spriteRenderer;

	private Map map;

	public bool Walkable
	{
		get
		{
			return walkable;
		}
		set
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			walkable = value;
			if (walkable)
			{
				spriteRenderer.color = activeColor;
			}
			else
			{
				spriteRenderer.sprite = normal;
				spriteRenderer.color = inactiveColor;
			}
			UpdateSpriteRenderingSortingOrder();
		}
	}

	public bool IsWalkable
	{
		get
		{
			if (forceDisabled)
			{
				return false;
			}
			UnlockID[] array = requiredUnlocks;
			foreach (UnlockID id in array)
			{
				if (!UnlocksManager.Instance.Unlocked(id))
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool Highlight
	{
		get
		{
			return highlight;
		}
		set
		{
			highlight = value;
			if (highlight)
			{
				spriteRenderer.sprite = highlighted;
			}
			else
			{
				spriteRenderer.sprite = normal;
			}
			UpdateSpriteRenderingSortingOrder();
		}
	}

	private void Awake()
	{
		spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		normal = spriteRenderer.sprite;
		defaultSpriteRendererSortingOrder = ((Renderer)spriteRenderer).sortingOrder;
	}

	private void Start()
	{
		map = MapManager.Instance.map;
		if (!IsWalkable)
		{
			((Renderer)spriteRenderer).enabled = false;
		}
	}

	public IEnumerator MoveToEndLocation(MapPlayer player)
	{
		Highlight = true;
		player.WalkToLocation();
		List<Vector3> points = new List<Vector3> { ((Component)start).transform.localPosition };
		Transform[] array = waypoints;
		foreach (Transform val in array)
		{
			points.Add(((Component)this).transform.localPosition + val.localPosition);
		}
		points.Add(((Component)end).transform.localPosition);
		for (int i = 0; i < points.Count - 1; i++)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(Walk(player, points[i], points[i + 1]));
		}
		player.ArrivedAtLocation();
		yield return (object)new WaitForSeconds(0.3f);
	}

	private IEnumerator Walk(MapPlayer player, Vector3 a, Vector3 b)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float speed = walkingSpeed;
		if (Globals.QuickAnimations)
		{
			speed *= 5f;
		}
		Vector3 direction = Vector3.Normalize(b - a);
		float walkTime = Vector3.Distance(a, b) / speed;
		float t = 0f;
		while (t <= walkTime)
		{
			((Component)player).transform.Translate(speed * direction * Time.deltaTime);
			t += Time.deltaTime;
			yield return null;
		}
	}

	private void UpdateSpriteRenderingSortingOrder()
	{
		((Renderer)spriteRenderer).sortingOrder = defaultSpriteRendererSortingOrder;
		if (Walkable)
		{
			SpriteRenderer obj = spriteRenderer;
			((Renderer)obj).sortingOrder = ((Renderer)obj).sortingOrder + 1;
		}
		if (Highlight)
		{
			SpriteRenderer obj2 = spriteRenderer;
			((Renderer)obj2).sortingOrder = ((Renderer)obj2).sortingOrder + 1;
		}
	}
}
