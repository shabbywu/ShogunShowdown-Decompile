using System.Collections;
using Parameters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public GameObject healthBarUnitPrefab;

	private HealthBarUnit[] units = new HealthBarUnit[0];

	private int MaxHp => units.Length;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.ColorblindModeUpdated.AddListener(new UnityAction(ColorblindModeUpdated));
	}

	public void Initialize(int maxHp, int hp)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).gameObject.activeSelf)
		{
			LayoutGroup componentInChildren = ((Component)this).GetComponentInChildren<LayoutGroup>();
			HealthBarUnit[] componentsInChildren = ((Component)componentInChildren).GetComponentsInChildren<HealthBarUnit>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.Destroy((Object)(object)((Component)componentsInChildren[i]).gameObject);
			}
			units = new HealthBarUnit[maxHp];
			for (int j = 0; j < maxHp; j++)
			{
				GameObject val = Object.Instantiate<GameObject>(healthBarUnitPrefab, ((Component)componentInChildren).transform);
				units[j] = val.GetComponent<HealthBarUnit>();
			}
			HealthUpdate(hp);
			componentsInChildren = units;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Initialize();
			}
			if (maxHp == 0 || maxHp > 5 || maxHp % 2 == 1)
			{
				((Component)componentInChildren).transform.localPosition = Vector3.zero;
			}
			else
			{
				((Component)componentInChildren).transform.localPosition = 0.5f * TechParams.pixelSize * Vector3.right;
			}
		}
	}

	public void HealthUpdate(int hp)
	{
		if (((Component)this).gameObject.activeInHierarchy)
		{
			if (hp < 0)
			{
				Debug.LogError((object)$"HealthBar:HealthUpdate: hp smaller than zero: {hp}");
			}
			if (hp > MaxHp)
			{
				Debug.LogError((object)$"HealthBar:HealthUpdate: hp larger than MaxHp. hp: {hp}, max hp: {MaxHp}");
			}
			for (int i = 0; i < MaxHp; i++)
			{
				units[i].Full = i < hp;
			}
			if (hp == 0)
			{
				((MonoBehaviour)this).StartCoroutine(FadeOut(0.2f));
			}
		}
	}

	private IEnumerator FadeOut(float wait)
	{
		yield return (object)new WaitForSeconds(wait);
		HealthBarUnit[] array = units;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FadeOut();
		}
	}

	private void OnDestroy()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.ColorblindModeUpdated.RemoveListener(new UnityAction(ColorblindModeUpdated));
	}

	private void ColorblindModeUpdated()
	{
		HealthBarUnit[] array = units;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
	}
}
