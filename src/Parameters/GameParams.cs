namespace Parameters;

public static class GameParams
{
	public static int maxDay = 5;

	public static int initialNumberOfConsumableSlots = 3;

	public static int roomClearedThreshold = 3;

	public static float autoRepeatDelay = 0.4f;

	public static float autoRepeatRate = autoRepeatDelay / 3f;

	public static float stickyInputTime = 0.2f;

	public static float holdButtonTime = 0.5f;

	public static int totalLevelsBeforeOfferingIncreaseMaxLevelTileUpgrade = 2;

	public static int deckSizeBeforeOfferingSacrificeUpgrade = 4;

	public static float enemyCoinDropProbabilityHigh = 0.5f;

	public static float enemyCoinDropProbabilityLow = 0.3f;

	public static float edamameDropRatePerRoomHigh = 0.6f;

	public static float potionsDropRatePerRoomHigh = 0.4f;

	public static float scrollsDropRatePerRoomHigh = 0.4f;

	public static float edamameDropRatePerRoomLow = 0.3f;

	public static float potionsDropRatePerRoomLow = 0.25f;

	public static float scrollsDropRatePerRoomLow = 0.25f;

	public static float pickupDropVariance = 0.5f;

	public static int approximateNumberOfEnemiesPerRoom = 10;

	public static int iceEffectTurnsDuration = 4;

	public static int poisonEffectTurnsDuration = 3;

	public static float freeShopConsumableProbabilityAtLowDropAscension = 1f / 3f;

	public static int maxTurnsBeforeNextWaveWhenOneEnemyLeft = 6;

	public static float eliteEnemyProbability = 0.1f;

	public static int BossKillMetacurrencyReward(int sector, int day)
	{
		int num;
		switch (sector)
		{
		case 1:
			num = 1;
			break;
		case 2:
		case 3:
			num = 2;
			break;
		case 4:
		case 5:
			num = 3;
			break;
		default:
			num = 4;
			break;
		}
		return num + day;
	}

	public static int UnlocksMetacurrencyPrice(int indexOfUnlockItem)
	{
		if (indexOfUnlockItem < 3)
		{
			return 1;
		}
		if (indexOfUnlockItem < 6)
		{
			return 3;
		}
		if (indexOfUnlockItem < 9)
		{
			return 6;
		}
		if (indexOfUnlockItem < 12)
		{
			return 10;
		}
		if (indexOfUnlockItem < 15)
		{
			return 15;
		}
		return 20;
	}

	public static int ShopUpgradeMetacurrencyPrice(int numberOfTimesUpgraded)
	{
		return numberOfTimesUpgraded switch
		{
			0 => 1, 
			1 => 4, 
			_ => 8, 
		};
	}
}
