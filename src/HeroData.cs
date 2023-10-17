using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "SO/HeroData", order = 1)]
public class HeroData : ScriptableObject
{
	public string name;

	public Sprite familyCrestSelected;

	public Sprite familyCrestNotSelected;
}
