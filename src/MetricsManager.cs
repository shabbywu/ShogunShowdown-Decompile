using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;

public class MetricsManager : MonoBehaviour
{
	public RoomMetrics currentRoomMetrics;

	public RunMetrics runMetrics;

	private string RunsHistorySaveData => "RunsHistory.json";

	public static MetricsManager Instance { get; private set; }

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
		runMetrics = new RunMetrics();
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(BeginRun));
		EventsManager.Instance.EnterRoom.AddListener((UnityAction<Room>)EnterRoom);
		EventsManager.Instance.NewWaveSpawns.AddListener((UnityAction<Wave>)NewWaveSpawns);
		EventsManager.Instance.ExitRoom.AddListener((UnityAction<Room>)ExitRoom);
		EventsManager.Instance.GameOver.AddListener((UnityAction<bool>)GameOver);
	}

	private void GameOver(bool win)
	{
		runMetrics.win = win;
		runMetrics.metaCurrencyAtGameOver = Globals.KillCount;
		EndOfRoom();
		if (Globals.RecordRunHistory)
		{
			StoreRunHistory();
		}
		if (Globals.UseUnityAnalytics)
		{
			UploadRunDataToUnityAnalytics();
		}
	}

	public void UploadRunDataToUnityAnalytics()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{ "hero", runMetrics.hero },
			{ "runNumber", runMetrics.runNumber },
			{ "win", runMetrics.win },
			{
				"day",
				runMetrics.runStats.day
			},
			{
				"lastRoom",
				runMetrics.roomMetrics.Last().roomId
			},
			{
				"nemesisRooms",
				GetNemesisRooms()
			},
			{
				"nemesisEnemies",
				GetNemesisEnemies()
			}
		};
		AnalyticsService.Instance.CustomData("gameOver", (IDictionary<string, object>)dictionary);
		if (runMetrics.win)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>
			{
				{ "hero", runMetrics.hero },
				{ "runNumber", runMetrics.runNumber },
				{
					"day",
					runMetrics.runStats.day
				},
				{
					"deck",
					string.Join(", ", currentRoomMetrics.deck)
				},
				{
					"skills",
					string.Join(", ", currentRoomMetrics.skills)
				}
			};
			AnalyticsService.Instance.CustomData("win", (IDictionary<string, object>)dictionary2);
		}
		AnalyticsService.Instance.Flush();
	}

	public void StoreRunHistory()
	{
		RunsHistory runsHistory = new RunsHistory();
		if (FileManager.FileExists(RunsHistorySaveData) && FileManager.LoadFromFile(RunsHistorySaveData, out var result))
		{
			JsonUtility.FromJsonOverwrite(result, (object)runsHistory);
		}
		runsHistory.runs.Add(runMetrics);
		FileManager.WriteToFile(RunsHistorySaveData, JsonUtility.ToJson((object)runsHistory));
	}

	public void BeginRun()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		EventsManager.Instance.HeroTookDamage.AddListener((UnityAction<int>)HeroTookDamage);
		EventsManager.Instance.BeginningOfCombatTurn.AddListener(new UnityAction(BeginningOfCombatTurn));
		EventsManager.Instance.HeroSpecialMove.AddListener(new UnityAction(PlayerSpecialMove));
		EventsManager.Instance.HeroTurnedAround.AddListener(new UnityAction(HeroTurnedAround));
		EventsManager.Instance.HeroAttacks.AddListener((UnityAction<AttackQueue>)HeroAttacked);
		EventsManager.Instance.EnemyFriendlyKill.AddListener(new UnityAction(EnemyFriendlyKill));
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)ComboKill);
		EventsManager.Instance.HeroIsHit.AddListener((UnityAction<(Hit, Agent)>)HeroIsHit);
		EventsManager.Instance.PotionUsed.AddListener((UnityAction<Potion>)PotionUsed);
		EventsManager.Instance.PlayerChoiceInNewTileReward.AddListener((UnityAction<string>)PlayerChoiceInNewTileReward);
		EventsManager.Instance.PlayerChoiceInTileUpgradeReward.AddListener((UnityAction<string>)PlayerChoiceInTileUpgradeReward);
		EventsManager.Instance.ShopItemBought.AddListener((UnityAction<string>)ShopItemBought);
		EventsManager.Instance.NewTilePicked.AddListener((UnityAction<Tile>)NewTilePicked);
		runMetrics.version = Application.version;
		runMetrics.hero = Globals.Hero.TechnicalName;
		runMetrics.runNumber = Globals.NRuns;
		runMetrics.numberOfUnlockedTilesAtBeginRun = UnlocksManager.Instance.GetUnlockedTiles().Count;
		runMetrics.numberOfCompletedQuestsAtBeginRun = QuestsManager.Instance.NumberOfCompletedQuests;
		runMetrics.runContinued = Globals.ContinueRun;
		if (!Globals.ContinueRun)
		{
			runMetrics.runStats.Initialize();
		}
	}

	public void UpdateRunStats()
	{
		runMetrics.runStats.day = Globals.Day;
		runMetrics.runStats.coins = Globals.Coins;
		runMetrics.runStats.time = (int)CombatSceneManager.Instance.RunTime;
	}

	private void EnterRoom(Room room)
	{
		currentRoomMetrics = default(RoomMetrics);
		currentRoomMetrics.roomId = room.Id;
		currentRoomMetrics.initialHP = Globals.Hero.AgentStats.HP;
		currentRoomMetrics.finalHP = 0;
		currentRoomMetrics.damageTaken = 0;
		currentRoomMetrics.turns = 0;
		currentRoomMetrics.coins = Globals.Coins;
		currentRoomMetrics.time = (int)CombatSceneManager.Instance.RunTime;
		currentRoomMetrics.sector = Progression.Instance.CurrentLocation.sector;
		currentRoomMetrics.deck = new List<string>();
		currentRoomMetrics.enemies = new List<string>();
		currentRoomMetrics.attacks = new List<string>();
		currentRoomMetrics.hits = new List<string>();
		currentRoomMetrics.consumablesUsed = new List<string>();
		currentRoomMetrics.skills = new List<string>();
		foreach (Tile item in TilesManager.Instance.Deck)
		{
			currentRoomMetrics.deck.Add(item.Attack.TechNameAndStats);
		}
		foreach (Item item2 in ItemsManager.Instance.Items)
		{
			currentRoomMetrics.skills.Add($"{item2.Name}_lvl_{item2.Level}");
		}
		currentRoomMetrics.nPlayerSpecialMove = 0;
		currentRoomMetrics.nCombos = 0;
	}

	private void PlayerChoiceInNewTileReward(string description)
	{
		runMetrics.playerChoicesInNewTileRewards.Add(description);
	}

	private void PlayerChoiceInTileUpgradeReward(string description)
	{
		runMetrics.playerChoicesInTileUpgradeRewards.Add(description);
	}

	private void ShopItemBought(string description)
	{
		runMetrics.shopItemsBought.Add(description);
	}

	private void NewTilePicked(Tile tile)
	{
		runMetrics.runStats.nNewTilesPicked++;
	}

	private void NewWaveSpawns(Wave wave)
	{
		currentRoomMetrics.iWave++;
		foreach (string listOfEnemyName in wave.ListOfEnemyNames)
		{
			currentRoomMetrics.enemies.Add($"{currentRoomMetrics.iWave} {listOfEnemyName}");
		}
	}

	public void ExitRoom(Room room)
	{
		if (room is CombatRoom)
		{
			runMetrics.runStats.numberOfCombatRoomsCleared++;
		}
		EndOfRoom();
	}

	private void EndOfRoom()
	{
		currentRoomMetrics.finalHP = Globals.Hero.AgentStats.HP;
		currentRoomMetrics.time = (int)CombatSceneManager.Instance.RunTime - currentRoomMetrics.time;
		runMetrics.roomMetrics.Add(currentRoomMetrics);
	}

	private void HeroTookDamage(int damage)
	{
		currentRoomMetrics.damageTaken += damage;
	}

	private void BeginningOfCombatTurn()
	{
		if (CombatSceneManager.Instance.RunInProgress)
		{
			currentRoomMetrics.turns++;
			runMetrics.runStats.turns++;
		}
	}

	private void EnemyFriendlyKill()
	{
		runMetrics.runStats.friendlyKills++;
	}

	private void PlayerSpecialMove()
	{
		currentRoomMetrics.nPlayerSpecialMove++;
	}

	private void HeroTurnedAround()
	{
		runMetrics.runStats.nTurnArounds++;
	}

	private void HeroAttacked(AttackQueue attackQueue)
	{
		currentRoomMetrics.attacks.Add(attackQueue.ContentDescription);
	}

	private void HeroIsHit((Hit hit, Agent attacker) t)
	{
		int num = ((!(CombatSceneManager.Instance.Room is WaveRoom)) ? 1 : (((WaveRoom)CombatSceneManager.Instance.Room).WaveNumber + 1));
		string arg = (((Object)(object)t.attacker != (Object)null && t.attacker is Enemy) ? ((Enemy)t.attacker).TechnicalName : "NotAnEnemy");
		string description = t.hit.Description;
		currentRoomMetrics.hits.Add($"{num} {arg} {description}");
		runMetrics.runStats.hits++;
	}

	private void ComboKill(Enemy enemy)
	{
		if (!enemy.Summoned)
		{
			currentRoomMetrics.nCombos++;
			runMetrics.runStats.combos++;
		}
	}

	private void PotionUsed(Potion potion)
	{
		currentRoomMetrics.consumablesUsed.Add(Enum.GetName(typeof(PotionsManager.PotionEnum), potion.PotionEnum));
		runMetrics.runStats.nConsumablesUsed++;
	}

	private string GetNemesisRooms()
	{
		List<string> list = new List<string>();
		foreach (RoomMetrics roomMetric in runMetrics.roomMetrics)
		{
			if (roomMetric.damageTaken > 3)
			{
				list.Add(roomMetric.roomId);
			}
		}
		return string.Join(", ", list);
	}

	private string GetNemesisEnemies()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (RoomMetrics roomMetric in runMetrics.roomMetrics)
		{
			foreach (string hit in roomMetric.hits)
			{
				string key = hit.Split(' ')[1];
				if (dictionary.ContainsKey(key))
				{
					dictionary[key]++;
				}
				else
				{
					dictionary.Add(key, 1);
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, int> item in dictionary)
		{
			if (item.Value > 4)
			{
				list.Add(item.Key);
			}
		}
		return string.Join(", ", list);
	}
}
