using System;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using UnityEngine;

public class PotionsManager : MonoBehaviour
{
	public enum PotionEnum
	{
		healSmall,
		coolUp,
		freezeTime,
		shield,
		massCurse,
		poison
	}

	[SerializeField]
	private PotionsContainerUI potionsContainerUI;

	private static string potionsResourcesPath = "Potions";

	private int nPotionsSlots;

	private Dictionary<PotionEnum, Potion> potionPrefabs;

	public static PotionsManager Instance { get; private set; }

	public PotionsContainerUI PotionsContainerUI => potionsContainerUI;

	public bool CanPickUpPotion => NPotions < NPotionsSlots;

	public int NPotions => potionsContainerUI.NPotions;

	public Potion[] HeldPotions => potionsContainerUI.Potions;

	private List<PotionEnum> CurrentlyHeldPotionsEnums => HeldPotions.Select((Potion potion) => potion.PotionEnum).ToList();

	public int NPotionsSlots
	{
		get
		{
			return nPotionsSlots;
		}
		set
		{
			nPotionsSlots = value;
			potionsContainerUI.SetNumberOfSlots(nPotionsSlots);
		}
	}

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
		LoadPotionPrefabsFromResources();
		NPotionsSlots = GameParams.initialNumberOfConsumableSlots;
	}

	public void TakePotion(Potion potion)
	{
		potionsContainerUI.AddPotion(potion);
	}

	public void TakePotion(PotionEnum potionEnum)
	{
		Potion potion = Object.Instantiate<Potion>(potionPrefabs[potionEnum], ((Component)this).transform);
		TakePotion(potion);
	}

	public int NumberOfPotionOfThisType(PotionEnum potionEnum)
	{
		return HeldPotions.Where((Potion potion) => potion.PotionEnum == potionEnum).Count();
	}

	private void LoadPotionPrefabsFromResources()
	{
		potionPrefabs = new Dictionary<PotionEnum, Potion>();
		GameObject[] array = Array.ConvertAll(Resources.LoadAll(potionsResourcesPath), (Converter<Object, GameObject>)((Object item) => (GameObject)item));
		for (int i = 0; i < array.Length; i++)
		{
			Potion component = array[i].GetComponent<Potion>();
			potionPrefabs.Add(component.PotionEnum, component);
		}
	}

	public void PopulateSaveData(RunSaveData runInProgressSaveData)
	{
		runInProgressSaveData.potions = CurrentlyHeldPotionsEnums;
	}

	public void LoadFromSaveData(RunSaveData runInProgressSaveData)
	{
		foreach (PotionEnum potion in runInProgressSaveData.potions)
		{
			TakePotion(potion);
		}
	}
}
