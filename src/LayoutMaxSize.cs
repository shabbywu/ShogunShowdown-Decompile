using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
[RequireComponent(typeof(RectTransform))]
public class LayoutMaxSize : LayoutElement
{
	public float maxHeight = -1f;

	public float maxWidth = -1f;

	public override void CalculateLayoutInputHorizontal()
	{
		((LayoutElement)this).CalculateLayoutInputHorizontal();
		UpdateMaxSizes();
	}

	public override void CalculateLayoutInputVertical()
	{
		((LayoutElement)this).CalculateLayoutInputVertical();
		UpdateMaxSizes();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		((UIBehaviour)this).OnRectTransformDimensionsChange();
		UpdateMaxSizes();
	}

	private void UpdateMaxSizes()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		if (maxHeight != -1f)
		{
			if (((LayoutElement)this).preferredHeight == -1f && maxHeight < ((Component)this).GetComponent<RectTransform>().sizeDelta.y)
			{
				((LayoutElement)this).preferredHeight = maxHeight;
			}
			else if (((LayoutElement)this).preferredHeight != -1f && ((Component)this).transform.childCount > 0)
			{
				bool flag = true;
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < ((Component)this).transform.childCount; i++)
				{
					RectTransform component = ((Component)((Component)this).transform.GetChild(i)).GetComponent<RectTransform>();
					if (!((Object)(object)component == (Object)null))
					{
						Vector3 localPosition = ((Transform)component).localPosition;
						Vector2 sizeDelta = component.sizeDelta;
						Vector2 pivot = component.pivot;
						if (flag)
						{
							num = localPosition.y + sizeDelta.y * (1f - pivot.y);
							num2 = localPosition.y - sizeDelta.y * pivot.y;
						}
						else
						{
							num = Mathf.Max(num, localPosition.y + sizeDelta.y * (1f - pivot.y));
							num2 = Mathf.Min(num2, localPosition.y - sizeDelta.y * pivot.y);
						}
						flag = false;
					}
				}
				if (flag)
				{
					return;
				}
				float num3 = Mathf.Abs(num - num2);
				if (((LayoutElement)this).preferredHeight > num3)
				{
					((LayoutElement)this).preferredHeight = -1f;
				}
			}
		}
		if (maxWidth == -1f)
		{
			return;
		}
		if (((LayoutElement)this).preferredWidth == -1f && maxWidth < ((Component)this).GetComponent<RectTransform>().sizeDelta.x)
		{
			((LayoutElement)this).preferredWidth = maxWidth;
		}
		else
		{
			if (((LayoutElement)this).preferredWidth == -1f || ((Component)this).transform.childCount <= 0)
			{
				return;
			}
			bool flag2 = true;
			float num4 = 0f;
			float num5 = 0f;
			for (int j = 0; j < ((Component)this).transform.childCount; j++)
			{
				RectTransform component2 = ((Component)((Component)this).transform.GetChild(j)).GetComponent<RectTransform>();
				if (!((Object)(object)component2 == (Object)null))
				{
					Vector3 localPosition2 = ((Transform)component2).localPosition;
					Vector2 sizeDelta2 = component2.sizeDelta;
					Vector2 pivot2 = component2.pivot;
					if (flag2)
					{
						num4 = localPosition2.x + sizeDelta2.x * (1f - pivot2.x);
						num5 = localPosition2.x - sizeDelta2.x * pivot2.x;
					}
					else
					{
						num4 = Mathf.Max(num4, localPosition2.x + sizeDelta2.x * (1f - pivot2.x));
						num5 = Mathf.Min(num5, localPosition2.x - sizeDelta2.x * pivot2.x);
					}
					flag2 = false;
				}
			}
			if (!flag2)
			{
				float num6 = Mathf.Abs(num4 - num5);
				if (((LayoutElement)this).preferredWidth > num6)
				{
					((LayoutElement)this).preferredWidth = -1f;
				}
			}
		}
	}
}
