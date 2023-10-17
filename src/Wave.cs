using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class Wave
{
	private List<Enemy> enemiesToSpawn;

	private static float nextToAgentPenalty = 2f;

	private static float surroundingHeroPenalty = 5f;

	private static float edgeCellBonus = -0.2f;

	private static float flankingBonus = -0.2f;

	private static bool debug = false;

	public int Duration { get; private set; }

	public int NEnemies => enemiesToSpawn.Count;

	public float Probability { get; private set; }

	private bool Summoned { get; set; }

	public List<string> ListOfEnemyNames => enemiesToSpawn.Select((Enemy enemy) => enemy.TechnicalName).ToList();

	public Wave(Enemy[] enemies, int duration, float probability = 1f, bool summoned = false)
	{
		enemiesToSpawn = enemies.ToList();
		Duration = duration;
		Summoned = summoned;
		Probability = probability;
	}

	public void Spawn(CombatRoom room)
	{
		int num = 5;
		List<(Enemy, Cell)> list = null;
		for (int i = 0; i < num; i++)
		{
			list = GenerateEnemySpawningPositions(enemiesToSpawn, room.Grid.Cells);
			List<Agent> list2 = LayoutFromCellsAndSpawningPositions(list, room.Grid.Cells);
			bool flag = !IsHeroFlankedByTwoRangedEnemies(list2);
			if (debug && !flag)
			{
				Debug.Log((object)$"Hero was flanked by two ranged enemies. Trying again... (iteration {i + 1}/{num})");
				Debug.Log((object)("layout: " + string.Join(", ", list2.Select((Agent agent) => (!((Object)(object)agent == (Object)null)) ? agent.TechnicalName : "null").ToList())));
			}
			if (flag)
			{
				break;
			}
		}
		List<Enemy> list3 = new List<Enemy>();
		foreach (var (enemy2, cell) in list)
		{
			list3.Add(Spawner.SpawnEnemy(cell, ((Component)enemy2).gameObject));
			list3[^1].Summoned = Summoned;
		}
		EffectsManager.Instance.GenericSpawningEffect();
		EventsManager.Instance.NewWaveSpawns.Invoke(this);
		if (debug)
		{
			Debug.Log((object)("Wave: " + string.Join(", ", enemiesToSpawn.Select((Enemy enemy) => enemy.TechnicalName).ToList())));
			EventsManager.Instance.LogToGameScreen.Invoke(string.Join(", ", enemiesToSpawn.Select((Enemy enemy) => enemy.TechnicalName).ToList()));
		}
	}

	private List<(Enemy, Cell)> GenerateEnemySpawningPositions(List<Enemy> enemies, Cell[] cells)
	{
		List<(Enemy, Cell)> list = new List<(Enemy, Cell)>();
		enemies.Shuffle();
		List<Agent> list2 = LayoutFromCells(cells);
		int[] input = Enumerable.Range(0, cells.Length).ToArray();
		foreach (Enemy enemy in enemies)
		{
			float[] probabilities = SpawningProbabilities(list2);
			int num = MyRandom.NextFromArray(input, probabilities);
			list.Add((enemy, cells[num]));
			list2[num] = enemy;
		}
		return list;
	}

	private List<Agent> LayoutFromCells(Cell[] cells)
	{
		List<Agent> list = new List<Agent>();
		foreach (Cell cell in cells)
		{
			if ((Object)(object)cell.Agent != (Object)null)
			{
				list.Add(cell.Agent);
			}
			else
			{
				list.Add(null);
			}
		}
		return list;
	}

	private List<Agent> LayoutFromCellsAndSpawningPositions(List<(Enemy, Cell)> spawningPositions, Cell[] cells)
	{
		Dictionary<Cell, Enemy> dictionary = new Dictionary<Cell, Enemy>();
		foreach (var (value, key) in spawningPositions)
		{
			dictionary.Add(key, value);
		}
		List<Agent> list = new List<Agent>();
		foreach (Cell cell in cells)
		{
			if ((Object)(object)cell.Agent != (Object)null)
			{
				list.Add(cell.Agent);
			}
			else if (dictionary.ContainsKey(cell))
			{
				list.Add(dictionary[cell]);
			}
			else
			{
				list.Add(null);
			}
		}
		return list;
	}

	private bool IsHeroFlankedByTwoRangedEnemies(List<Agent> layout)
	{
		int iHero = layout.IndexOf(Globals.Hero);
		Enemy enemy = LeftFlankingEnemy(layout, iHero);
		if ((Object)(object)enemy == (Object)null || !enemy.IsPurelyRangedEnemy)
		{
			return false;
		}
		Enemy enemy2 = RightFlankingEnemy(layout, iHero);
		if ((Object)(object)enemy2 == (Object)null || !enemy2.IsPurelyRangedEnemy)
		{
			return false;
		}
		return true;
	}

	private Enemy LeftFlankingEnemy(List<Agent> layout, int iHero)
	{
		for (int num = iHero - 1; num >= 0; num--)
		{
			if ((Object)(object)layout[num] != (Object)null && layout[num] is Enemy)
			{
				return (Enemy)layout[num];
			}
		}
		return null;
	}

	private Enemy RightFlankingEnemy(List<Agent> layout, int iHero)
	{
		for (int i = iHero + 1; i < layout.Count; i++)
		{
			if ((Object)(object)layout[i] != (Object)null && layout[i] is Enemy)
			{
				return (Enemy)layout[i];
			}
		}
		return null;
	}

	private float[] SpawningProbabilities(List<Agent> layout)
	{
		int num = layout.IndexOf(Globals.Hero);
		Agent agent = LeftFlankingEnemy(layout, num);
		Agent agent2 = RightFlankingEnemy(layout, num);
		int count = layout.Count;
		float[] array = new float[count];
		for (int i = 0; i < count; i++)
		{
			if ((Object)(object)layout[i] != (Object)null)
			{
				array[i] = 0f;
				continue;
			}
			float num2 = 1f;
			if (i == 0 || i == count - 1)
			{
				num2 += edgeCellBonus;
			}
			if (i - 1 >= 0 && (Object)(object)layout[i - 1] != (Object)null)
			{
				num2 += nextToAgentPenalty;
			}
			if (i + 1 < count && (Object)(object)layout[i + 1] != (Object)null)
			{
				num2 += nextToAgentPenalty;
			}
			if ((Object)(object)agent == (Object)null && i < num)
			{
				num2 += flankingBonus;
			}
			if ((Object)(object)agent2 == (Object)null && i > num)
			{
				num2 += flankingBonus;
			}
			if (i == num + 1 && num - 1 >= 0 && (Object)(object)layout[num - 1] != (Object)null)
			{
				num2 += surroundingHeroPenalty;
			}
			else if (i == num - 1 && num + 1 < count && (Object)(object)layout[num + 1] != (Object)null)
			{
				num2 += surroundingHeroPenalty;
			}
			array[i] = 1f / num2;
		}
		return array;
	}
}
