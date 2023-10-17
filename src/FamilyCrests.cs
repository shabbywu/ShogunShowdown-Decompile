using System.Collections.Generic;
using UnityEngine;

public class FamilyCrests : MonoBehaviour
{
	public GameObject familyCrestTemplate;

	public RectTransform container;

	public HeroData lockedHeroData;

	private List<GameObject> familyCrests = new List<GameObject>();

	public void Initialize(Hero[] heroes)
	{
		foreach (GameObject familyCrest in familyCrests)
		{
			Object.Destroy((Object)(object)familyCrest);
		}
		familyCrests = new List<GameObject>();
		for (int i = 0; i < heroes.Length; i++)
		{
			_ = heroes[i];
			familyCrests.Add(Object.Instantiate<GameObject>(familyCrestTemplate, (Transform)(object)container));
		}
		UpdateForSelectedHero(heroes, null);
	}

	public void UpdateForSelectedHero(Hero[] heroes, Hero hero)
	{
		for (int i = 0; i < heroes.Length; i++)
		{
			familyCrests[i].gameObject.SetActive(true);
			HeroData heroData = ((!heroes[i].Unlocked) ? lockedHeroData : heroes[i].heroData);
			if ((Object)(object)heroes[i] == (Object)(object)hero)
			{
				familyCrests[i].GetComponent<SpriteRenderer>().sprite = heroData.familyCrestSelected;
			}
			else
			{
				familyCrests[i].GetComponent<SpriteRenderer>().sprite = heroData.familyCrestNotSelected;
			}
		}
	}
}
