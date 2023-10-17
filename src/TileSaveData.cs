using System;
using TileEnums;
using UnityEngine;

[Serializable]
public class TileSaveData
{
	public AttackEnum attackEnum;

	public int cooldown;

	public int cooldownCharge;

	public int value;

	public int baseValue;

	public int level;

	public int maxLevel;

	public AttackEffectEnum attackEffect;

	public TileEffectEnum tileEffect;

	public bool IsEquivalentTo(TileSaveData other)
	{
		return JsonUtility.ToJson((object)this) == JsonUtility.ToJson((object)other);
	}
}
