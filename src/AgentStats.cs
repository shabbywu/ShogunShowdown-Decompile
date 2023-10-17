using System;
using UnityEngine;

[Serializable]
public class AgentStats
{
	public int maxHP;

	public bool shield;

	public int ice;

	public int poison;

	public bool mark;

	[SerializeField]
	private int hp;

	public int HP
	{
		get
		{
			return hp;
		}
		set
		{
			hp = Mathf.Clamp(value, 0, maxHP);
		}
	}

	public void ResetStatusEffects()
	{
		ice = 0;
		poison = 0;
		mark = false;
	}
}
