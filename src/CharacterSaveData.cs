using System;
using System.Collections.Generic;
using ProgressionEnums;
using UnityEngine;

[Serializable]
public class CharacterSaveData : NamedSaveData
{
	public int bestDay;

	public int bestTime;

	public int bestHits;

	public int bestTurns;

	public int bestCombos;

	public List<HeroStamp> stamps;

	public List<HeroStamp> ultimateStamps;

	public CharacterSaveData(string name)
	{
		base.name = name;
		stamps = new List<HeroStamp>();
		ultimateStamps = new List<HeroStamp>();
		bestDay = -1;
		bestTime = -1;
		bestHits = -1;
		bestTurns = -1;
		bestCombos = -1;
	}

	public HeroStampRank GetHeroStampRank(HeroStamp stamp)
	{
		if (ultimateStamps.Contains(stamp))
		{
			return HeroStampRank.ultimate;
		}
		if (stamps.Contains(stamp))
		{
			return HeroStampRank.regular;
		}
		return HeroStampRank.noRank;
	}

	public void AddHeroStamp(HeroStamp stamp, HeroStampRank rank)
	{
		if (!stamps.Contains(stamp))
		{
			stamps.Add(stamp);
		}
		if (rank == HeroStampRank.ultimate && !ultimateStamps.Contains(stamp))
		{
			ultimateStamps.Add(stamp);
		}
	}

	public void ShogunDefeated(int day, int time, int turns, int combos, int hits)
	{
		bestDay = Mathf.Max(day, bestDay);
		if (bestTime < 0)
		{
			bestTime = time;
		}
		else
		{
			bestTime = Math.Min(bestTime, time);
		}
		if (bestTurns < 0)
		{
			bestTurns = turns;
		}
		else
		{
			bestTurns = Math.Min(bestTurns, turns);
		}
		bestCombos = Math.Max(bestCombos, combos);
		if (bestHits < 0)
		{
			bestHits = hits;
		}
		else
		{
			bestHits = Math.Min(bestHits, hits);
		}
	}
}
