using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class Ascension
{
	public enum Buffs
	{
		lowerDrops,
		eliteEnemies,
		higherEnemiesHP,
		lowerHeroHP
	}

	private static readonly Dictionary<Buffs, int> dayOnWhichBuffIsActivated = new Dictionary<Buffs, int>
	{
		{
			Buffs.lowerDrops,
			2
		},
		{
			Buffs.eliteEnemies,
			3
		},
		{
			Buffs.higherEnemiesHP,
			4
		},
		{
			Buffs.lowerHeroHP,
			5
		}
	};

	private static readonly Dictionary<Buffs, string> localizationTableBuffName = new Dictionary<Buffs, string>
	{
		{
			Buffs.lowerDrops,
			"Ascension_LowerDrops"
		},
		{
			Buffs.eliteEnemies,
			"Ascension_EliteEnemies"
		},
		{
			Buffs.higherEnemiesHP,
			"Ascension_HigherEnemiesHP"
		},
		{
			Buffs.lowerHeroHP,
			"Ascension_LowerHeroHP"
		}
	};

	public static bool LowerDrops => IsBuffActive(Buffs.lowerDrops);

	public static bool EliteEnemies => IsBuffActive(Buffs.eliteEnemies);

	public static bool HigherEnemiesHP => IsBuffActive(Buffs.higherEnemiesHP);

	public static bool LowerHeroHP => IsBuffActive(Buffs.lowerHeroHP);

	public static bool IsBuffActive(Buffs buff)
	{
		return dayOnWhichBuffIsActivated[buff] <= Globals.Day;
	}

	public static Buffs BuffActivatedOnDay(int day)
	{
		foreach (KeyValuePair<Buffs, int> item in dayOnWhichBuffIsActivated)
		{
			if (item.Value == day)
			{
				return item.Key;
			}
		}
		Debug.LogError((object)$"BuffActivatedOnDay: No buff found for day {day}. I should not get here!");
		return Buffs.lowerDrops;
	}

	public static string DescriptionOfBuffActivatedOnDay(int day)
	{
		return LocalizationUtils.LocalizedString("Metaprogression", localizationTableBuffName[BuffActivatedOnDay(day)]);
	}
}
