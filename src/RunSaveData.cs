using System;
using System.Collections.Generic;
using PickupEnums;
using SkillEnums;
using Utils;

[Serializable]
public class RunSaveData
{
	public string version;

	public int runNumber;

	public bool hasRunInProgress;

	public bool mapSelectionInProgress;

	public RunStats runStats;

	public MapSaveData mapSaveData;

	public ProgressionSaveData progressionSaveData;

	public List<TileSaveData> deck;

	public List<PotionsManager.PotionEnum> potions;

	public List<SkillEnum> skills;

	public List<int> skillsLevel;

	public List<PickupEnum> pickups;

	public List<int> pickupsCellIndex;

	public CombatRoomSaveData combatRoom;

	public ShopRoomSaveData shopRoom;

	public RewardRoomSaveData rewardRoom;

	public HeroCombatSaveData hero;

	public string Description => TextUitls.ReplaceTags(string.Concat(string.Concat("" + "[low_priority_color]{0}:[end_color] " + MyTime.ToMinAndSecFormat(runStats.time) + "[vspace]\n", "[low_priority_color]{1}:[end_color] ", hero.name, "[vspace]\n"), "[low_priority_color]{2}:[end_color] ", mapSaveData.currentLocationName.Replace("\n", " ")));

	public RunSaveData()
	{
		hasRunInProgress = false;
		runStats = new RunStats();
		mapSelectionInProgress = false;
		mapSaveData = new MapSaveData();
		progressionSaveData = new ProgressionSaveData();
		deck = new List<TileSaveData>();
		potions = new List<PotionsManager.PotionEnum>();
		skills = new List<SkillEnum>();
		skillsLevel = new List<int>();
		pickups = new List<PickupEnum>();
		pickupsCellIndex = new List<int>();
		combatRoom = new CombatRoomSaveData();
		shopRoom = new ShopRoomSaveData();
		rewardRoom = new RewardRoomSaveData();
		hero = new HeroCombatSaveData();
	}
}
