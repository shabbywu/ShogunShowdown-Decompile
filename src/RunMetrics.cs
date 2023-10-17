using System;
using System.Collections.Generic;

[Serializable]
public class RunMetrics
{
	public RunStats runStats;

	public string version;

	public string hero;

	public int runNumber;

	public List<RoomMetrics> roomMetrics;

	public List<string> playerChoicesInNewTileRewards;

	public List<string> playerChoicesInTileUpgradeRewards;

	public List<string> shopItemsBought;

	public int numberOfUnlockedTilesAtBeginRun;

	public int numberOfCompletedQuestsAtBeginRun;

	public bool win;

	public bool runContinued;

	public int metaCurrencyAtGameOver;

	public RunMetrics()
	{
		runStats = new RunStats();
		roomMetrics = new List<RoomMetrics>();
		playerChoicesInNewTileRewards = new List<string>();
		playerChoicesInTileUpgradeRewards = new List<string>();
		shopItemsBought = new List<string>();
	}
}
