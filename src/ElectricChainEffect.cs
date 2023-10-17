using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ElectricChainEffect : MonoBehaviour
{
	public GameObject LightningArcEffect;

	public int damage = 1;

	public void Initialize(Agent[] targets)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		List<Agent> list = new List<Agent>();
		List<Vector3> list2 = new List<Vector3>();
		foreach (Agent agent in targets)
		{
			Dir[] array = new Dir[2]
			{
				Dir.left,
				Dir.right
			};
			foreach (Dir dir in array)
			{
				Agent agent2 = agent;
				while (true)
				{
					Cell cell = agent2.Cell.Neighbour(dir, 1);
					if ((Object)(object)cell == (Object)null || (Object)(object)cell.Agent == (Object)null || cell.Agent.IsOpponent(agent))
					{
						break;
					}
					if (!list.Contains(agent))
					{
						list.Add(agent);
					}
					if (!list.Contains(cell.Agent))
					{
						list2.Add(0.5f * (((Component)agent2).transform.position + ((Component)cell.Agent).transform.position));
						list.Add(cell.Agent);
					}
					agent2 = cell.Agent;
				}
			}
		}
		if (list.Count > 0)
		{
			SoundEffectsManager.Instance.Play("ElectricChainEffect");
		}
		foreach (Agent item in list)
		{
			item.ReceiveAttack(new Hit(damage, isDirectional: false), null);
		}
		foreach (Vector3 item2 in list2)
		{
			Object.Instantiate<GameObject>(LightningArcEffect, item2, Quaternion.identity);
		}
		((MonoBehaviour)this).StartCoroutine(WaitAndDestroy(2f));
	}

	private IEnumerator WaitAndDestroy(float wait)
	{
		yield return (object)new WaitForSeconds(wait);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
