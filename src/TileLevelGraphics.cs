using System.Collections.Generic;
using Parameters;
using UnityEngine;
using Utils;

public class TileLevelGraphics : MonoBehaviour
{
	public GameObject tileLevelUnitPrefab;

	public GameObject layoutGroup;

	private List<SpriteRenderer> units = new List<SpriteRenderer>();

	private int _level;

	private int _maxLevel;

	public void SetVisible(bool value)
	{
		layoutGroup.SetActive(value);
	}

	public void UpdateGraphics(int level, int maxLevel)
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		if (level != _level || _maxLevel != maxLevel)
		{
			if (maxLevel < _maxLevel)
			{
				Debug.LogError((object)$"TileLevelGraphics: UpdateGraphics: maxLevel < _maxLevel ({maxLevel},{_maxLevel})");
			}
			if (level > maxLevel)
			{
				Debug.LogError((object)$"TileLevelGraphics: UpdateGraphics: level > maxLevel ({level},{maxLevel})");
			}
			for (int i = 0; i < maxLevel - _maxLevel; i++)
			{
				GameObject val = Object.Instantiate<GameObject>(tileLevelUnitPrefab, layoutGroup.transform);
				units.Add(val.GetComponent<SpriteRenderer>());
			}
			for (int j = 0; j < maxLevel; j++)
			{
				string hexString = ((j >= level) ? Colors.lightCobaltHex : Colors.birghtYellowHex);
				units[j].color = Colors.FromHex(hexString);
			}
			if (maxLevel % 2 == 0 || maxLevel > 4)
			{
				((Component)this).transform.localPosition = 0.5f * TechParams.pixelSize * Vector3.left;
			}
			else
			{
				((Component)this).transform.localPosition = Vector3.zero;
			}
			_level = level;
			_maxLevel = maxLevel;
		}
	}
}
