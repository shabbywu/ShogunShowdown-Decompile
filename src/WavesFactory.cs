using System.Collections.Generic;
using UnityEngine;
using Utils;

public class WavesFactory : MonoBehaviour
{
	[SerializeField]
	private Enemy tank;

	[SerializeField]
	private Enemy archer;

	[SerializeField]
	private Enemy spear;

	[SerializeField]
	private Enemy swirl;

	[SerializeField]
	private Enemy grappler;

	[SerializeField]
	private Enemy shadow;

	[SerializeField]
	private Enemy guardian;

	[SerializeField]
	private Enemy dasher;

	[SerializeField]
	private Enemy ninja;

	[SerializeField]
	private Enemy shielder;

	[SerializeField]
	private Enemy volley;

	private bool shogunDefeated;

	public static WavesFactory Instance { get; private set; }

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
	}

	public List<Wave> GetWaves(string id)
	{
		shogunDefeated = (Object)(object)UnlocksManager.Instance != (Object)null && UnlocksManager.Instance.ShogunDefeated;
		switch (id)
		{
		case "bg-1":
			return BambooGrove_1();
		case "bg-2":
			return BambooGrove_2();
		case "wc-1":
			return WaterfallCaves_1();
		case "wc-2":
			return WaterfallCaves_2();
		case "ab-1":
			return AncientBattleground_1();
		case "ab-2":
			return AncientBattleground_2();
		case "pc-1":
			return PortCity_1();
		case "pc-2":
			return PortCity_2();
		case "pc-3":
			return PortCity_3();
		case "gy-1":
		case "tg-1":
			return TempleGate_1();
		case "gy-2":
		case "tg-2":
			return TempleGate_2();
		case "gy-3":
		case "tg-3":
			return TempleGate_3();
		case "hs-1":
		case "toi-1":
			return HotSprings_1();
		case "hs-2":
		case "toi-2":
			return HotSprings_2();
		case "hs-3":
		case "toi-3":
			return HotSprings_3();
		case "h-1":
		case "n-1":
		case "t-1":
			return PreDaimyo();
		case "s-1":
			return ShogunCastle();
		case "pg-1":
		case "pg-2":
			return Playground();
		case "debug-empty":
			return new List<Wave>();
		default:
			Debug.LogError((object)("GetWaves: undefined id '" + id + "'!"));
			return null;
		}
	}

	private List<Wave> TestWave()
	{
		int n = 10;
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[1] { Wav(tank, volley, volley, 12) });
		return PickN(waveGen, n);
	}

	private List<Wave> Playground()
	{
		int nWaves = 100;
		List<Enemy> pool = new List<Enemy> { tank, swirl, archer, dasher };
		return BuildWavesFromPool(pool, nWaves, 2, 3);
	}

	private List<Wave> BambooGrove_1()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[2]
		{
			Wav(tank, 9),
			Wav(swirl, 9)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[2]
		{
			Wav(tank, swirl, 11),
			Wav(swirl, swirl, 11)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 2));
		list.AddRange(PickN(waveGen2, 2));
		return list;
	}

	private List<Wave> BambooGrove_2()
	{
		int n = 5;
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[6]
		{
			Wav(tank, tank, 12),
			Wav(dasher, tank, 12),
			Wav(dasher, swirl, 12),
			Wav(tank, swirl, swirl, 15),
			Wav(dasher, swirl, swirl, 15),
			Wav(dasher, tank, tank, 16)
		});
		return PickN(waveGen, n);
	}

	private List<Wave> WaterfallCaves_1()
	{
		int n = 3;
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[4]
		{
			Wav(spear, tank, 16),
			Wav(spear, swirl, 16),
			Wav(spear, dasher, 15),
			Wav(tank, dasher, 15)
		});
		return PickN(waveGen, n);
	}

	private List<Wave> WaterfallCaves_2()
	{
		int n = 4;
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[5]
		{
			Wav(spear, swirl, swirl, 16),
			Wav(tank, tank, tank, 13),
			Wav(spear, spear, 15),
			Wav(spear, dasher, dasher, 14),
			Wav(dasher, dasher, 13)
		});
		return PickN(waveGen, n);
	}

	private List<Wave> AncientBattleground_1()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[3]
		{
			Wav(volley, tank, 9),
			Wav(volley, swirl, 9),
			Wav(volley, dasher, 9)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[2]
		{
			Wav(tank, tank, tank, 13),
			Wav(swirl, swirl, tank, 13)
		});
		PseudoRandomWithMemory<Wave> waveGen3 = WaveGen(new Wave[6]
		{
			Wav(volley, tank, tank, 15),
			Wav(volley, tank, dasher, 14),
			Wav(volley, tank, swirl, 14),
			Wav(volley, swirl, swirl, 14),
			Wav(volley, swirl, dasher, 14),
			Wav(volley, dasher, dasher, 13)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen3, 2));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen3, 1));
		return list;
	}

	private List<Wave> AncientBattleground_2()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[4]
		{
			Wav(tank, 3),
			Wav(dasher, 3),
			Wav(swirl, 3),
			Wav(volley, 3)
		});
		List<Wave> list = new List<Wave>();
		for (int i = 0; i < 10; i++)
		{
			list.AddRange(PickN(waveGen, 1));
		}
		return list;
	}

	private List<Wave> PortCity_1()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[2]
		{
			Wav(archer, tank, 11),
			Wav(archer, swirl, 11)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[6]
		{
			Wav(archer, spear, tank, 15),
			Wav(spear, spear, swirl, 14),
			Wav(archer, tank, swirl, 15),
			Wav(archer, tank, tank, 15),
			Wav(volley, spear, swirl, 14, shogunDefeated ? 1f : 0f),
			Wav(volley, archer, tank, 15, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 2));
		list.AddRange(PickN(waveGen2, 3));
		return list;
	}

	private List<Wave> PortCity_2()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[2]
		{
			Wav(guardian, spear, 16),
			Wav(guardian, tank, 16)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[6]
		{
			Wav(guardian, dasher, dasher, 14),
			Wav(guardian, archer, 14),
			Wav(guardian, spear, 16),
			Wav(guardian, tank, 15),
			Wav(guardian, swirl, 14),
			Wav(volley, guardian, 14, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 2));
		list.AddRange(PickN(waveGen2, 3));
		return list;
	}

	private List<Wave> PortCity_3()
	{
		int n = 5;
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[8]
		{
			Wav(spear, spear, dasher, dasher, 18),
			Wav(tank, tank, swirl, archer, 18),
			Wav(tank, spear, swirl, dasher, 18),
			Wav(archer, guardian, tank, 18),
			Wav(guardian, archer, spear, 18),
			Wav(guardian, spear, swirl, 18),
			Wav(volley, guardian, spear, 18, shogunDefeated ? 1 : 0),
			Wav(volley, volley, tank, tank, 18, shogunDefeated ? 1 : 0)
		});
		return PickN(waveGen, n);
	}

	private List<Wave> TempleGate_1()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[2]
		{
			Wav(grappler, spear, 14),
			Wav(grappler, tank, 14)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[5]
		{
			Wav(spear, spear, tank, 15),
			Wav(guardian, guardian, tank, 17),
			Wav(spear, guardian, tank, 16),
			Wav(guardian, swirl, archer, 15),
			Wav(volley, guardian, tank, 15, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen3 = WaveGen(new Wave[6]
		{
			Wav(grappler, tank, tank, 16),
			Wav(grappler, tank, guardian, 18),
			Wav(grappler, tank, spear, 16),
			Wav(grappler, tank, dasher, 16),
			Wav(grappler, guardian, dasher, 16),
			Wav(volley, grappler, tank, 16, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen3, 1));
		list.AddRange(PickN(waveGen2, 2));
		list.AddRange(PickN(waveGen3, 1));
		return list;
	}

	private List<Wave> TempleGate_2()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[3]
		{
			Wav(ninja, spear, 10),
			Wav(ninja, tank, 9),
			Wav(ninja, guardian, 12)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[7]
		{
			Wav(spear, spear, tank, 14),
			Wav(guardian, guardian, tank, 16),
			Wav(guardian, swirl, archer, 14),
			Wav(spear, guardian, dasher, 16),
			Wav(archer, dasher, tank, 14),
			Wav(dasher, dasher, spear, 14),
			Wav(volley, tank, dasher, 14, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen3 = WaveGen(new Wave[5]
		{
			Wav(ninja, spear, tank, 17),
			Wav(ninja, guardian, tank, 17),
			Wav(ninja, swirl, swirl, 14),
			Wav(ninja, guardian, dasher, 16),
			Wav(volley, ninja, tank, 17, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen3, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen3, 1));
		return list;
	}

	private List<Wave> TempleGate_3()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[8]
		{
			Wav(spear, spear, tank, tank, 17),
			Wav(swirl, swirl, swirl, swirl, 17),
			Wav(spear, archer, guardian, 15),
			Wav(spear, spear, dasher, 16),
			Wav(guardian, archer, tank, 16),
			Wav(archer, archer, tank, tank, 15),
			Wav(volley, spear, tank, 17, shogunDefeated ? 1f : 0f),
			Wav(volley, volley, swirl, swirl, 15, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[12]
		{
			Wav(ninja, tank, tank, swirl, 18),
			Wav(grappler, spear, swirl, swirl, 18),
			Wav(ninja, dasher, tank, 18),
			Wav(ninja, guardian, archer, 18),
			Wav(archer, guardian, spear, dasher, 18),
			Wav(grappler, archer, tank, 18),
			Wav(grappler, guardian, spear, dasher, 18),
			Wav(grappler, guardian, swirl, dasher, 18),
			Wav(volley, grappler, tank, 18, shogunDefeated ? 1f : 0f),
			Wav(volley, ninja, guardian, 18, shogunDefeated ? 1f : 0f),
			Wav(volley, ninja, spear, 18, shogunDefeated ? 1f : 0f),
			Wav(volley, grappler, spear, 18, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen, 2));
		list.AddRange(PickN(waveGen2, 2));
		return list;
	}

	private List<Wave> HotSprings_1()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[5]
		{
			Wav(shielder, spear, spear, 14),
			Wav(shielder, spear, archer, 14),
			Wav(shielder, grappler, tank, 14),
			Wav(shielder, archer, swirl, 14),
			Wav(volley, shielder, tank, 14, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[9]
		{
			Wav(archer, archer, spear, 14),
			Wav(guardian, guardian, dasher, 14),
			Wav(grappler, guardian, 10),
			Wav(ninja, guardian, 10),
			Wav(grappler, tank, 10),
			Wav(ninja, tank, 10),
			Wav(volley, archer, guardian, 16, shogunDefeated ? 1f : 0f),
			Wav(volley, grappler, 10, shogunDefeated ? 1f : 0f),
			Wav(volley, ninja, 10, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen, 1));
		return list;
	}

	private List<Wave> HotSprings_2()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[6]
		{
			Wav(archer, 2),
			Wav(tank, 2),
			Wav(dasher, 2),
			Wav(swirl, 2),
			Wav(spear, 2),
			Wav(volley, 2, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[4]
		{
			Wav(grappler, 3),
			Wav(ninja, 3),
			Wav(shielder, 3, 0.75f),
			Wav(guardian, 3)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen2, 1));
		for (int i = 0; i < 7; i++)
		{
			list.AddRange(PickN(waveGen, 1));
			list.AddRange(PickN(waveGen2, 1));
		}
		return list;
	}

	private List<Wave> HotSprings_3()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[7]
		{
			Wav(spear, spear, tank, tank, 17),
			Wav(spear, spear, dasher, dasher, 14),
			Wav(shielder, tank, archer, archer, 14),
			Wav(swirl, swirl, archer, shielder, 14),
			Wav(spear, guardian, archer, dasher, 15),
			Wav(volley, spear, dasher, swirl, 17, shogunDefeated ? 1f : 0f),
			Wav(volley, dasher, dasher, 14, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[7]
		{
			Wav(grappler, archer, guardian, 16),
			Wav(ninja, grappler, tank, 16),
			Wav(shielder, spear, spear, spear, 20),
			Wav(grappler, ninja, tank, spear, 20),
			Wav(shielder, guardian, archer, archer, 16),
			Wav(volley, ninja, guardian, 18, shogunDefeated ? 1f : 0f),
			Wav(volley, volley, shielder, archer, 16, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 2));
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 2));
		return list;
	}

	private List<Wave> PreDaimyo()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[2]
		{
			Wav(shadow, spear, 11),
			Wav(shadow, guardian, 11)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[11]
		{
			Wav(spear, spear, guardian, 12),
			Wav(spear, spear, archer, 14),
			Wav(dasher, dasher, dasher, 14),
			Wav(archer, archer, shielder, 14),
			Wav(ninja, guardian, 14),
			Wav(ninja, tank, tank, 14),
			Wav(grappler, guardian, 14),
			Wav(grappler, tank, spear, 16),
			Wav(volley, ninja, tank, 15, shogunDefeated ? 1f : 0f),
			Wav(volley, spear, guardian, 12, shogunDefeated ? 1f : 0f),
			Wav(volley, grappler, tank, 16, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen3 = WaveGen(new Wave[6]
		{
			Wav(shadow, guardian, 12),
			Wav(shadow, spear, tank, 12),
			Wav(shadow, tank, dasher, 12),
			Wav(shadow, guardian, shielder, 12),
			Wav(shadow, ninja, 12),
			Wav(volley, shadow, tank, 12, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		list.AddRange(PickN(waveGen, 1));
		list.AddRange(PickN(waveGen2, 1));
		list.AddRange(PickN(waveGen3, 1));
		list.AddRange(PickN(waveGen2, 3));
		list.AddRange(PickN(waveGen3, 1));
		list.AddRange(PickN(waveGen2, 2));
		list.AddRange(PickN(waveGen3, 1));
		return list;
	}

	private List<Wave> ShogunCastle()
	{
		PseudoRandomWithMemory<Wave> waveGen = WaveGen(new Wave[7]
		{
			Wav(tank, tank, spear, spear, 4),
			Wav(tank, tank, archer, archer, 4),
			Wav(tank, tank, dasher, dasher, 4),
			Wav(guardian, guardian, spear, spear, 4),
			Wav(guardian, guardian, tank, tank, 4),
			Wav(volley, volley, tank, tank, 4, shogunDefeated ? 1f : 0f),
			Wav(volley, guardian, tank, tank, 4, shogunDefeated ? 1f : 0f)
		});
		PseudoRandomWithMemory<Wave> waveGen2 = WaveGen(new Wave[9]
		{
			Wav(shadow, archer, 4),
			Wav(grappler, archer, 4),
			Wav(ninja, shielder, 4),
			Wav(shadow, dasher, 4),
			Wav(grappler, dasher, 4),
			Wav(ninja, dasher, 4),
			Wav(volley, shadow, 4, shogunDefeated ? 1f : 0f),
			Wav(volley, grappler, 4, shogunDefeated ? 1f : 0f),
			Wav(volley, ninja, 4, shogunDefeated ? 1f : 0f)
		});
		List<Wave> list = new List<Wave>();
		for (int i = 0; i < 5; i++)
		{
			list.AddRange(PickN(waveGen, 1));
			list.AddRange(PickN(waveGen2, 1));
		}
		return list;
	}

	private PseudoRandomWithMemory<Wave> WaveGen(Wave[] waves)
	{
		(Wave, float)[] array = new(Wave, float)[waves.Length];
		for (int i = 0; i < waves.Length; i++)
		{
			array[i] = (waves[i], waves[i].Probability);
		}
		return new PseudoRandomWithMemory<Wave>(array, 2f, array.Length == 1);
	}

	private List<Wave> PickN(PseudoRandomWithMemory<Wave> waveGen, int n)
	{
		List<Wave> list = new List<Wave>();
		for (int i = 0; i < n; i++)
		{
			list.Add(waveGen.GetNext());
		}
		return list;
	}

	private Wave Wav(Enemy e1, int t, float p = 1f)
	{
		return new Wave(new Enemy[1] { e1 }, t, p);
	}

	private Wave Wav(Enemy e1, Enemy e2, int t, float p = 1f)
	{
		return new Wave(new Enemy[2] { e1, e2 }, t, p);
	}

	private Wave Wav(Enemy e1, Enemy e2, Enemy e3, int t, float p = 1f)
	{
		return new Wave(new Enemy[3] { e1, e2, e3 }, t, p);
	}

	private Wave Wav(Enemy e1, Enemy e2, Enemy e3, Enemy e4, int t, float p = 1f)
	{
		return new Wave(new Enemy[4] { e1, e2, e3, e4 }, t, p);
	}

	private Wave Wav(Enemy e1, Enemy e2, Enemy e3, Enemy e4, Enemy e5, int t, float p = 1f)
	{
		return new Wave(new Enemy[5] { e1, e2, e3, e4, e5 }, t, p);
	}

	private Wave BuildWaveFromPool(List<Enemy> pool, int minEnemies, int maxEnemies)
	{
		int num = Random.Range(minEnemies, maxEnemies + 1);
		List<Enemy> list = new List<Enemy>();
		for (int i = 0; i < num; i++)
		{
			list.Add(MyRandom.NextRandomUniform(pool));
		}
		return new Wave(list.ToArray(), 2 * num + 7);
	}

	private List<Wave> BuildWavesFromPool(List<Enemy> pool, int nWaves, int minEnemies, int maxEnemies)
	{
		List<Wave> list = new List<Wave>();
		for (int i = 0; i < nWaves; i++)
		{
			list.Add(BuildWaveFromPool(pool, minEnemies, maxEnemies));
		}
		return list;
	}

	private List<Wave> GenerateABunchOfWavesFromPool()
	{
		int nWaves = 100;
		List<Enemy> pool = new List<Enemy> { archer, spear, guardian, dasher, ninja, tank };
		return BuildWavesFromPool(pool, nWaves, 3, 4);
	}
}
