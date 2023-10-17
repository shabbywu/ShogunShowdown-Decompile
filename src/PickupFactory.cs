using System.Collections.Generic;
using System.Linq;
using Parameters;
using PickupEnums;
using UnityEngine;
using Utils;

public class PickupFactory : MonoBehaviour
{
	[SerializeField]
	private PickupDrop[] healingPickups;

	[SerializeField]
	private PickupDrop[] potionsPickups;

	[SerializeField]
	private PickupDrop[] scrollsPickups;

	private static string pickupsResourcesPath = "Pickups";

	private static float maxDistanceFromOrigin = 0.3f;

	private static float maxInitialVelocityX = 0.5f;

	private static float initialVelocityY = 3f;

	private Dictionary<PickupEnum, Pickup> pickupPrefabsDict;

	private PseudoRandomWithMemory<PickupEnum> randomHealingPickupGenerator;

	private PseudoRandomWithMemory<PickupEnum> randomPotionPickupGenerator;

	private PseudoRandomWithMemory<PickupEnum> randomScrollPickupGenerator;

	public static PickupFactory Instance { get; private set; }

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
		pickupPrefabsDict = new Dictionary<PickupEnum, Pickup>();
		foreach (GameObject item in ResourcesUtils.LoadGameObjects(pickupsResourcesPath))
		{
			Pickup component = item.GetComponent<Pickup>();
			pickupPrefabsDict.Add(component.PickupEnum, component);
		}
		randomHealingPickupGenerator = InitializePickupRandomGenerator(healingPickups);
		randomPotionPickupGenerator = InitializePickupRandomGenerator(potionsPickups);
		randomScrollPickupGenerator = InitializePickupRandomGenerator(scrollsPickups);
	}

	public Pickup InstantiatePickup(PickupEnum pickupEnum, Cell cell, Vector3? origin = null, float? minimumX = null, float? maximumX = null, Vector2? initialVelocity = null, bool playSoundEffect = true)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		Pickup component = ((Component)Object.Instantiate<Pickup>(pickupPrefabsDict[pickupEnum], ((Component)cell).transform)).GetComponent<Pickup>();
		Vector3 val = ((!origin.HasValue) ? (((Component)cell).transform.position + 0.5f * Vector3.up) : origin.Value);
		float mininumX = ((!minimumX.HasValue) ? (val.x - maxDistanceFromOrigin) : minimumX.Value);
		float maximumX2 = ((!maximumX.HasValue) ? (val.x + maxDistanceFromOrigin) : maximumX.Value);
		Vector2 initialVelocity2 = (Vector2)((!initialVelocity.HasValue) ? new Vector2(Random.Range(0f - maxInitialVelocityX, maxInitialVelocityX), initialVelocityY) : initialVelocity.Value);
		((Component)component).transform.position = val;
		component.Initialize(cell, mininumX, maximumX2, initialVelocity2);
		if (playSoundEffect)
		{
			SoundEffectsManager.Instance.Play("PickupDrop");
		}
		EventsManager.Instance.PickupCreated.Invoke(component);
		return component;
	}

	private PseudoRandomWithMemory<PickupEnum> InitializePickupRandomGenerator(PickupDrop[] pickupDrops)
	{
		(PickupEnum, float)[] array = new(PickupEnum, float)[pickupDrops.Length];
		for (int i = 0; i < pickupDrops.Length; i++)
		{
			array[i] = (pickupDrops[i].pickupEnum, pickupDrops[i].probability);
		}
		return new PseudoRandomWithMemory<PickupEnum>(array, 1.2f);
	}

	public float DropProbability(int nDrops, float dropRatePerRoom, int iRoom)
	{
		float num = dropRatePerRoom / (float)GameParams.approximateNumberOfEnemiesPerRoom;
		float num2 = dropRatePerRoom * (float)iRoom;
		float num3 = ((num2 == 0f) ? ((float)nDrops - num2) : (((float)nDrops - num2) / num2));
		return Mathf.Clamp(2f * num / (1f + Mathf.Exp(num3 / GameParams.pickupDropVariance)), 0f, 1f);
	}

	public void PotentiallySpawnPickup(Cell cell)
	{
		RunStats runStats = MetricsManager.Instance.runMetrics.runStats;
		if (MyRandom.Bool(Ascension.LowerDrops ? GameParams.enemyCoinDropProbabilityLow : GameParams.enemyCoinDropProbabilityHigh))
		{
			InstantiatePickup(PickupEnum.coin, cell);
		}
		foreach (Pickup pickup in CombatSceneManager.Instance.Room.Pickups)
		{
			if (pickup.PickupEnum != 0 && (Object)(object)pickup.Cell == (Object)(object)cell)
			{
				return;
			}
		}
		int numberOfCombatRoomsCleared = runStats.numberOfCombatRoomsCleared;
		float dropRatePerRoom = (Ascension.LowerDrops ? GameParams.edamameDropRatePerRoomLow : GameParams.edamameDropRatePerRoomHigh);
		float num = DropProbability(runStats.nHealPickupDrops, dropRatePerRoom, numberOfCombatRoomsCleared);
		if (BoostHealPotionDropProbability())
		{
			num *= 1.5f;
		}
		if (MyRandom.Bool(num))
		{
			if (ShouldConvertHealPotionDropIntoMoney())
			{
				for (int i = 0; i < HealSmallPotion.sellingPrice / 2; i++)
				{
					InstantiatePickup(PickupEnum.coin, cell);
				}
			}
			else
			{
				InstantiatePickup(randomHealingPickupGenerator.GetNext(), cell);
			}
			runStats.nHealPickupDrops++;
			return;
		}
		float dropRatePerRoom2 = (Ascension.LowerDrops ? GameParams.potionsDropRatePerRoomLow : GameParams.potionsDropRatePerRoomHigh);
		if (MyRandom.Bool(DropProbability(runStats.nPotionsPickupDrops, dropRatePerRoom2, numberOfCombatRoomsCleared)))
		{
			InstantiatePickup(randomPotionPickupGenerator.GetNext(), cell);
			runStats.nPotionsPickupDrops++;
			return;
		}
		float dropRatePerRoom3 = (Ascension.LowerDrops ? GameParams.scrollsDropRatePerRoomLow : GameParams.scrollsDropRatePerRoomHigh);
		if (MyRandom.Bool(DropProbability(runStats.nScrollsPickupDrops, dropRatePerRoom3, numberOfCombatRoomsCleared)))
		{
			InstantiatePickup(randomScrollPickupGenerator.GetNext(), cell);
			runStats.nScrollsPickupDrops++;
		}
	}

	private bool BoostHealPotionDropProbability()
	{
		if (Globals.Hero.AgentStats.HP < Globals.Hero.AgentStats.maxHP / 2)
		{
			return PotionsManager.Instance.NumberOfPotionOfThisType(PotionsManager.PotionEnum.healSmall) == 0;
		}
		return false;
	}

	private bool ShouldConvertHealPotionDropIntoMoney()
	{
		return PotionsManager.Instance.NumberOfPotionOfThisType(PotionsManager.PotionEnum.healSmall) >= 2;
	}

	private void SimulateRuns()
	{
		int num = 3;
		float num2 = 0.5f;
		List<string> list = new List<string>
		{
			"bg-1", "bg-2", "debug-empty", "wc-1", "wc-2", "debug-empty", "pc-1", "pc-2", "pc-3", "debug-empty",
			"tg-1", "tg-2", "tg-3", "debug-empty", "hs-1", "hs-2", "hs-3", "debug-empty", "k-1", "debug-empty",
			"s-1", "debug-empty"
		};
		Debug.Log((object)$"Simulation using the DropRatePerRoom with dropRatePerRoom = {num2}");
		string text = "";
		List<int> list2 = new List<int>();
		for (int i = 0; i < num; i++)
		{
			list2.Add(0);
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num3 = WavesFactory.Instance.GetWaves(list[j]).Aggregate(0, (int acc, Wave wave) => acc + wave.NEnemies);
			text += $"{j:0.00}, {num2 * (float)j:0.00}";
			for (int k = 0; k < num3; k++)
			{
				for (int l = 0; l < num; l++)
				{
					if (MyRandom.Bool(DropProbability(list2[l], num2, j)))
					{
						list2[l]++;
					}
				}
			}
			for (int m = 0; m < num; m++)
			{
				text += $", {list2[m]:0.00}";
			}
			text += "\n";
		}
		Debug.Log((object)text);
		Debug.Log((object)"");
		float num4 = 0.5f;
		int num5 = 5;
		Debug.Log((object)$"Simulation using the coinDropProbability with coinDropProbability = {num4}");
		text = "roomname,coins\n";
		int num6 = 10;
		for (int n = 0; n < list.Count; n++)
		{
			int num7 = WavesFactory.Instance.GetWaves(list[n]).Aggregate(0, (int acc, Wave wave) => acc + wave.NEnemies);
			for (int num8 = 0; num8 < num7; num8++)
			{
				if (MyRandom.Bool(num4))
				{
					num6++;
				}
			}
			if (num7 == 0)
			{
				num6 += num5;
				text += $"{list[n]}{n}, {num6}\n";
			}
		}
		Debug.Log((object)text);
	}
}
