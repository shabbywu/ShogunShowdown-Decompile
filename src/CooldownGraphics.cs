using System.Collections.Generic;
using Parameters;
using UnityEngine;

public class CooldownGraphics : MonoBehaviour
{
	public GameObject cooldownUnitPrefab;

	public Transform unitsContainer;

	public SpriteRenderer noCooldownSpriteRenderer;

	private List<GameObject> units = new List<GameObject>();

	private Color colorFull = new Color(0.8f, 0.8f, 0.8f);

	private Color colorEmpty = new Color(1f, 0.4f, 0.4f);

	private int cooldown;

	private int charge;

	public int Cooldown
	{
		set
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			cooldown = value;
			while (units.Count > cooldown)
			{
				Object.Destroy((Object)(object)units[units.Count - 1]);
				units.RemoveAt(units.Count - 1);
			}
			while (units.Count < cooldown)
			{
				units.Add(Object.Instantiate<GameObject>(cooldownUnitPrefab, unitsContainer));
			}
			if (cooldown == 0 || cooldown % 2 == 1)
			{
				((Component)this).transform.localPosition = Vector3.zero;
			}
			else
			{
				((Component)this).transform.localPosition = 0.5f * TechParams.pixelSize * Vector3.right;
			}
			if ((Object)(object)noCooldownSpriteRenderer != (Object)null)
			{
				((Renderer)noCooldownSpriteRenderer).enabled = cooldown == 0;
			}
		}
	}

	public int Charge
	{
		set
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			charge = Mathf.Max(0, value);
			for (int i = 0; i < cooldown; i++)
			{
				SpriteRenderer component = units[i].GetComponent<SpriteRenderer>();
				if (i < charge)
				{
					component.color = colorFull;
				}
				else
				{
					component.color = colorEmpty;
				}
			}
		}
	}
}
