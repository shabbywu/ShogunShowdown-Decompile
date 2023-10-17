using System.Collections.Generic;
using AgentEnums;
using Parameters;
using UnityEngine;
using Utils;

public class AgentsFactory : MonoBehaviour
{
	private static string enemiesResourcesPath = "Agents/Enemies";

	private static string bossesResourcesPath = "Agents/Bosses";

	private static string heroesResourcesPath = "Agents/Heroes";

	private Dictionary<EnemyEnum, Enemy> enemiesPrefabs;

	private Dictionary<HeroEnum, Hero> heroesPrefabs;

	public static AgentsFactory Instance { get; private set; }

	public PseudoRandomWithMemory<bool> EliteEnemyRandomizer { get; private set; }

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
		EliteEnemyRandomizer = new PseudoRandomWithMemory<bool>(new(bool, float)[2]
		{
			(true, GameParams.eliteEnemyProbability),
			(false, 1f - GameParams.eliteEnemyProbability)
		}, 1.05f);
	}

	public Enemy InstantiateEnemy(EnemyEnum enemyEnum, Transform parent)
	{
		return ((Component)Object.Instantiate<Enemy>(enemiesPrefabs[enemyEnum], parent)).GetComponent<Enemy>();
	}

	public Hero InstantiateHero(HeroEnum heroEnum)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)Object.Instantiate<Hero>(heroesPrefabs[heroEnum], Vector3.zero, Quaternion.identity)).GetComponent<Hero>();
	}

	public void Initialize()
	{
		List<GameObject> list = new List<GameObject>();
		list.AddRange(ResourcesUtils.LoadGameObjects(enemiesResourcesPath));
		list.AddRange(ResourcesUtils.LoadGameObjects(bossesResourcesPath));
		enemiesPrefabs = new Dictionary<EnemyEnum, Enemy>();
		foreach (GameObject item in list)
		{
			Enemy component = item.GetComponent<Enemy>();
			enemiesPrefabs.Add(component.EnemyEnum, component);
		}
		heroesPrefabs = new Dictionary<HeroEnum, Hero>();
		foreach (GameObject item2 in ResourcesUtils.LoadGameObjects(heroesResourcesPath))
		{
			Hero component2 = item2.GetComponent<Hero>();
			heroesPrefabs.Add(component2.HeroEnum, component2);
		}
	}
}
