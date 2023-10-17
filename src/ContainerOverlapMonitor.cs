using System.Collections.Generic;
using UnityEngine;

public class ContainerOverlapMonitor : MonoBehaviour
{
	private Dictionary<TileContainer, float> _containersDistance = new Dictionary<TileContainer, float>();

	private BoxCollider _boxCollider;

	public TileContainer TargetContainer
	{
		get
		{
			TileContainer result = null;
			float num = 10000f;
			foreach (KeyValuePair<TileContainer, float> item in _containersDistance)
			{
				if (item.Key.Interactable && item.Value < num)
				{
					num = item.Value;
					result = item.Key;
				}
			}
			return result;
		}
	}

	private void Awake()
	{
		_boxCollider = ((Component)this).GetComponent<BoxCollider>();
	}

	public void Clear()
	{
		_containersDistance = new Dictionary<TileContainer, float>();
	}

	public void Add(TileContainer tileContainer)
	{
		_containersDistance.Add(tileContainer, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		TileContainer component = ((Component)other).GetComponent<TileContainer>();
		if ((Object)(object)component != (Object)null && !_containersDistance.ContainsKey(component))
		{
			Dictionary<TileContainer, float> containersDistance = _containersDistance;
			Bounds bounds = ((Collider)component.BoxCollider).bounds;
			containersDistance.Add(component, Distance(((Bounds)(ref bounds)).center));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		TileContainer component = ((Component)other).GetComponent<TileContainer>();
		if ((Object)(object)component != (Object)null && _containersDistance.ContainsKey(component))
		{
			_containersDistance.Remove(component);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		TileContainer component = ((Component)other).GetComponent<TileContainer>();
		if ((Object)(object)component != (Object)null)
		{
			Bounds bounds;
			if (_containersDistance.ContainsKey(component))
			{
				Dictionary<TileContainer, float> containersDistance = _containersDistance;
				bounds = ((Collider)component.BoxCollider).bounds;
				containersDistance[component] = Distance(((Bounds)(ref bounds)).center);
			}
			else
			{
				Dictionary<TileContainer, float> containersDistance2 = _containersDistance;
				bounds = ((Collider)component.BoxCollider).bounds;
				containersDistance2.Add(component, Distance(((Bounds)(ref bounds)).center));
			}
		}
	}

	private float Distance(Vector3 otherCenter)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider)_boxCollider).bounds;
		return Vector3.Distance(((Bounds)(ref bounds)).center, otherCenter);
	}
}
