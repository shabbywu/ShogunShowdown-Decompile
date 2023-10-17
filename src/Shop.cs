using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using PickupEnums;
using ShopStuff;
using TMPro;
using UINavigation;
using UnityEngine;
using Utils;

public class Shop : MonoBehaviour, ISavable, INavigationGroup
{
	[SerializeField]
	private ShopData shopData;

	[SerializeField]
	private ShopItemUI shopItemUIPrefab;

	[SerializeField]
	private GameObject shopItemUIsContainer;

	[SerializeField]
	private TextMeshProUGUI shopNameTextMesh;

	[SerializeField]
	private ShopItemSlotUI shopItemSlotPrefab;

	private List<ShopItemUI> shopItemUIs;

	private List<ShopItemSlotUI> shopItemSlotUIs;

	private ShopKeeper shopKeeper;

	private static float shopItemOnSaleProbability = 0.075f;

	private bool shopAlreadyUpgraded;

	private bool freeConsumableAlreadyGiven;

	private bool shouldGiveFreeConsumable;

	private bool loadedFromRunSaveData;

	private int indexInShopOfLastSelectedTarget;

	private ShopSaveData shopSaveData;

	private static float ShopItemsDeltaX { get; } = 1.3125f;


	private string Name => LocalizationUtils.LocalizedString("Locations", shopData.technicalName);

	private bool IsFullyUpgraded
	{
		get
		{
			ShopItemPool[] pools = shopData.pools;
			foreach (ShopItemPool shopItemPool in pools)
			{
				if (shopSaveData.NSlotsPerType(shopItemPool.type) < shopItemPool.nSlotsMax)
				{
					return false;
				}
			}
			return true;
		}
	}

	private ShopItemData[] ShopItemDataCurrentlySelling => shopItemUIs.Select((ShopItemUI shopItemUI) => shopItemUI.shopItemData).ToArray();

	protected int NShopItems => shopItemUIs.Count;

	protected int NMaxShopItems
	{
		get
		{
			int num = 0;
			ShopItemPool[] pools = shopData.pools;
			foreach (ShopItemPool shopItemPool in pools)
			{
				num += shopItemPool.nSlotsMax;
			}
			return num;
		}
	}

	public List<INavigationTarget> Targets => ((IEnumerable<INavigationTarget>)shopItemUIs).ToList();

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => shopItemUIs.Count > 0;

	public void Initialize(ShopInRunSaveData shopInRunSaveData = null)
	{
		LoadFromSaveData(SaveDataManager.Instance.saveData);
		((TMP_Text)shopNameTextMesh).text = Name;
		ShopItemPool[] pools = shopData.pools;
		for (int i = 0; i < pools.Length; i++)
		{
			pools[i].Initialize();
		}
		shopItemUIs = new List<ShopItemUI>();
		loadedFromRunSaveData = shopInRunSaveData != null;
		if (shopInRunSaveData == null)
		{
			InstantiateShopItems();
			shouldGiveFreeConsumable = DetermineWhetherToGiveFreeConsumable();
		}
		else
		{
			LoadShopItems(shopInRunSaveData);
			shopAlreadyUpgraded = shopInRunSaveData.alreadyUpgraded;
			freeConsumableAlreadyGiven = shopInRunSaveData.freeConsumableAlreadyGiven;
			shouldGiveFreeConsumable = shopInRunSaveData.shouldGiveFreeConsumable;
		}
		InstantiateItemSlotUI();
		shopKeeper = ((Component)this).GetComponentInChildren<ShopKeeper>();
		Close();
	}

	private void LoadShopItems(ShopInRunSaveData shopInRunSaveData)
	{
		for (int i = 0; i < shopInRunSaveData.shopItemDataNames.Count; i++)
		{
			ShopItemData shopItemData = null;
			ShopItemPool[] pools = shopData.pools;
			for (int j = 0; j < pools.Length; j++)
			{
				ShopItemData[] items = pools[j].items;
				foreach (ShopItemData shopItemData2 in items)
				{
					if (((Object)shopItemData2).name == shopInRunSaveData.shopItemDataNames[i])
					{
						shopItemData = shopItemData2;
					}
				}
			}
			InstantiateShopItem(shopItemData, open: false, shopInRunSaveData.onSale[i]);
			foreach (ShopItemUI shopItemUI in shopItemUIs)
			{
				if (shopItemUI.shopItemData.ShopItemTypeEnum == ShopItemTypeEnum.shopUpgrade)
				{
					Price price = shopItemUI.price;
					int j = (shopItemUI.price.Value = GameParams.ShopUpgradeMetacurrencyPrice(shopSaveData.numberOfTimesUpgraded));
					price.Value = j;
				}
			}
		}
	}

	private ShopItemPool GetShopItemPoolOfType(ShopItemTypeEnum type)
	{
		ShopItemPool[] pools = shopData.pools;
		foreach (ShopItemPool shopItemPool in pools)
		{
			if (shopItemPool.type == type)
			{
				return shopItemPool;
			}
		}
		Debug.LogError((object)"ShopItemPoolOfType: requested type: '{type}' not defined in the pools (in the shop Inspector)");
		return null;
	}

	private void InstantiateShopItems(ShopItemData[] itemsToAvoid = null, ShopItemData[] itemsToPrefereblyAvoid = null)
	{
		ShopItemPool[] pools = shopData.pools;
		foreach (ShopItemPool shopItemPool in pools)
		{
			ShopItemData[] nextNItems = shopItemPool.GetNextNItems(shopSaveData.NSlotsPerType(shopItemPool.type), itemsToAvoid, itemsToPrefereblyAvoid);
			foreach (ShopItemData shopItemData in nextNItems)
			{
				InstantiateShopItem(shopItemData);
			}
		}
		if (!shopAlreadyUpgraded && !IsFullyUpgraded)
		{
			InstantiateShopUpgradeItem();
		}
	}

	private void InstantiateShopUpgradeItem()
	{
		List<ShopItemData> possibleShopUpgrades = GetPossibleShopUpgrades();
		InstantiateShopItem(MyRandom.NextRandomUniform(possibleShopUpgrades)).price.Value = GameParams.ShopUpgradeMetacurrencyPrice(shopSaveData.numberOfTimesUpgraded);
	}

	private List<ShopItemData> GetPossibleShopUpgrades()
	{
		List<ShopItemData> list = new List<ShopItemData>();
		ShopItemData[] items = GetShopItemPoolOfType(ShopItemTypeEnum.shopUpgrade).items;
		foreach (ShopItemData shopItemData in items)
		{
			ShopItemTypeEnum shopItemTypeToIncrease = ((ShopUpgradeShopItem)shopItemData).shopItemTypeToIncrease;
			if (shopSaveData.NSlotsPerType(shopItemTypeToIncrease) < GetShopItemPoolOfType(shopItemTypeToIncrease).nSlotsMax)
			{
				list.Add(shopItemData);
			}
		}
		return list;
	}

	private ShopItemUI InstantiateShopItem(ShopItemData shopItemData, bool open = false, bool? sale = null, int forceIndex = -1)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		ShopItemUI component = ((Component)Object.Instantiate<ShopItemUI>(shopItemUIPrefab, shopItemUIsContainer.transform)).GetComponent<ShopItemUI>();
		shopItemData.Initialize(((Component)component).transform, this);
		bool onSale = shopItemData.CanBeOnSale && MyRandom.Bool(shopItemOnSaleProbability);
		if (sale.HasValue)
		{
			onSale = sale.Value;
		}
		component.Initialize(shopItemData, this, onSale);
		if (forceIndex != -1)
		{
			component.IndexInShop = forceIndex;
		}
		else
		{
			component.IndexInShop = ComputeShopItemUIIndexInShop(component);
		}
		component.TargetPosition = ShopItemUITargetPosition(component.IndexInShop);
		((Component)component).transform.localPosition = component.TargetPosition;
		shopItemUIs.Add(component);
		shopItemUIs.Sort((ShopItemUI a, ShopItemUI b) => a.IndexInShop.CompareTo(b.IndexInShop));
		if (open)
		{
			component.Open();
		}
		return component;
	}

	public virtual void Begin()
	{
		if (NShopItems > 0 || loadedFromRunSaveData)
		{
			Open();
			if (shouldGiveFreeConsumable && !freeConsumableAlreadyGiven)
			{
				((MonoBehaviour)this).StartCoroutine(ShopkeeperGiveFreeConsumableCoroutine());
			}
		}
		else if (!loadedFromRunSaveData)
		{
			shopKeeper.BeginInteraction(shopKeeper.AllSoldText);
		}
	}

	private bool DetermineWhetherToGiveFreeConsumable()
	{
		if (!shopData.giveFreePotion)
		{
			return false;
		}
		if (freeConsumableAlreadyGiven)
		{
			return false;
		}
		if (!Ascension.LowerDrops)
		{
			return true;
		}
		return MyRandom.Bool(GameParams.freeShopConsumableProbabilityAtLowDropAscension);
	}

	private IEnumerator ShopkeeperGiveFreeConsumableCoroutine()
	{
		int num = PotionsManager.Instance.NumberOfPotionOfThisType(PotionsManager.PotionEnum.healSmall);
		PickupEnum pickupToGive = ((Ascension.LowerDrops || num > 1) ? PickupEnum.shield : PickupEnum.edamameBrew);
		shopKeeper.BeginInteraction(shopKeeper.GetRandomTextForPotionGift(), 2.75f);
		yield return (object)new WaitForSeconds(0.75f);
		EffectsManager.Instance.CreateInGameEffect("SmallAppearEffect", shopKeeper.ConsumableAppearanceTransform);
		Pickup pickup = InstantiateAndThrowPickupAtHero(pickupToGive);
		freeConsumableAlreadyGiven = true;
		pickup.PhysicsEnabled = false;
		yield return (object)new WaitForSeconds(1f);
		SoundEffectsManager.Instance.Play("ThrowConsumable");
		pickup.PhysicsEnabled = true;
	}

	public bool CanBuyAnything()
	{
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			if (shopItemUI.price.CanAfford)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void End()
	{
		Close();
		shopKeeper.EndInteraction();
		PopulateSaveData(SaveDataManager.Instance.saveData);
	}

	public virtual void ItemBought(ShopItemUI itemBought)
	{
		SetShopItemUIsInteractable(interactable: false);
		if (itemBought.shopItemData is ServiceShopItem)
		{
			PerformShopService(itemBought);
		}
		else if (itemBought.shopItemData.ShopItemTypeEnum == ShopItemTypeEnum.unlock)
		{
			((MonoBehaviour)this).StartCoroutine(UnlockSequence(itemBought));
		}
		else if (itemBought.shopItemData is ShopUpgradeShopItem)
		{
			((MonoBehaviour)this).StartCoroutine(UpgradeShopSequence(itemBought));
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(RegularItemBoughtSequence(itemBought));
		}
	}

	private IEnumerator RegularItemBoughtSequence(ShopItemUI itemBought)
	{
		while ((Object)(object)itemBought != (Object)null)
		{
			yield return null;
		}
		ItemBoughtSequenceOver();
	}

	private IEnumerator UnlockSequence(ShopItemUI itemBought)
	{
		CombatManager.Instance.AllowHeroAction = false;
		UnlockShopItemData unlockShopItemData = (UnlockShopItemData)itemBought.shopItemData;
		UnlockBanner unlockBanner = EffectsManager.Instance.CreateInGameEffect("UnlockBanner", CameraUtils.CenterOfScreen()).GetComponent<UnlockBanner>();
		string text = string.Format(LocalizationUtils.LocalizedString("Terms", "Unlocked"), unlockShopItemData.Name);
		unlockBanner.Initialize(text, "Unlock");
		while ((Object)(object)unlockBanner != (Object)null)
		{
			yield return null;
		}
		while ((Object)(object)itemBought != (Object)null)
		{
			yield return null;
		}
		UnlocksManager.Instance.Unlock(unlockShopItemData.unlockId);
		InstantiateRandomShopItem(ShopItemTypeEnum.unlock, open: true);
		CombatManager.Instance.AllowHeroAction = true;
		ItemBoughtSequenceOver();
		if (NShopItems == 0)
		{
			Close();
			shopKeeper.BeginInteraction(shopKeeper.AllSoldText);
		}
	}

	private IEnumerator UpgradeShopSequence(ShopItemUI upgradeShopItemUI)
	{
		ShopUpgradeShopItem shopUpgradeShopItem = (ShopUpgradeShopItem)upgradeShopItemUI.shopItemData;
		ShopItemTypeEnum type = shopUpgradeShopItem.shopItemTypeToIncrease;
		int num = Array.IndexOf(shopSaveData.shopItemTypes, type);
		shopSaveData.nSlotsPerType[num]++;
		shopSaveData.numberOfTimesUpgraded++;
		GetShopItemPoolOfType(type);
		PopulateSaveData(SaveDataManager.Instance.saveData);
		UpdateShopItemSlotUIs();
		UnlockBanner unlockBanner = EffectsManager.Instance.CreateInGameEffect("UnlockBanner", CameraUtils.CenterOfScreen()).GetComponent<UnlockBanner>();
		string text = string.Format(LocalizationUtils.LocalizedString("ShopAndNPC", "ShopUpgraded"), Name);
		unlockBanner.Initialize(text, "ShopUpgrade");
		while ((Object)(object)unlockBanner != (Object)null)
		{
			yield return null;
		}
		InstantiateRandomShopItem(type, open: true, true, upgradeShopItemUI.IndexInShop);
		ItemBoughtSequenceOver();
		SaveDataManager.Instance.StoreSaveData();
		shopAlreadyUpgraded = true;
		EventsManager.Instance.SaveRunProgress.Invoke();
	}

	public void RemoveShopItemUIAfterBuying(ShopItemUI toRemove)
	{
		RemoveShopItemUI(toRemove);
		EventsManager.Instance.SaveRunProgress.Invoke();
	}

	private void InstantiateItemSlotUI()
	{
		shopItemSlotUIs = new List<ShopItemSlotUI>();
		ShopItemPool[] pools = shopData.pools;
		foreach (ShopItemPool shopItemPool in pools)
		{
			for (int j = 0; j < shopItemPool.nSlotsMax; j++)
			{
				shopItemSlotUIs.Add(((Component)Object.Instantiate<ShopItemSlotUI>(shopItemSlotPrefab, shopItemUIsContainer.transform)).GetComponent<ShopItemSlotUI>());
			}
		}
		UpdateShopItemSlotUIs();
	}

	private void UpdateShopItemSlotUIs()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		ShopItemPool[] pools = shopData.pools;
		foreach (ShopItemPool shopItemPool in pools)
		{
			for (int j = 0; j < shopItemPool.nSlotsMax; j++)
			{
				bool unlocked = j < shopSaveData.NSlotsPerType(shopItemPool.type);
				shopItemSlotUIs[num].UpdateGraphics(unlocked);
				((Component)shopItemSlotUIs[num]).transform.localPosition = ShopItemUITargetPosition(num);
				num++;
			}
		}
	}

	private void RemoveShopItemUI(ShopItemUI toRemove)
	{
		shopItemUIs.Remove(toRemove);
		Object.Destroy((Object)(object)((Component)toRemove).gameObject);
	}

	private void InstantiateRandomShopItem(ShopItemTypeEnum type, bool open = false, bool? sale = null, int forceIndex = -1)
	{
		ShopItemData[] nextNItems = GetShopItemPoolOfType(type).GetNextNItems(1, ShopItemDataCurrentlySelling);
		if (nextNItems.Length != 0)
		{
			InstantiateShopItem(nextNItems[0], open, sale, forceIndex);
		}
	}

	public Pickup InstantiateAndThrowPickupAtHero(PickupEnum pickupEnum)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("ThrowConsumable");
		return PickupFactory.Instance.InstantiatePickup(pickupEnum, CombatSceneManager.Instance.Room.Grid.Cells[0], (Vector3?)shopKeeper.ConsumableAppearanceTransform.position, (float?)(((Component)Globals.Hero).transform.position.x - 10f), (float?)(((Component)Globals.Hero).transform.position.x + 10f), (Vector2?)new Vector2(-5f, 5.5f), playSoundEffect: true);
	}

	protected void Open()
	{
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			shopItemUI.Open();
		}
		foreach (ShopItemSlotUI shopItemSlotUI in shopItemSlotUIs)
		{
			shopItemSlotUI.Open();
		}
	}

	protected void Close()
	{
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			shopItemUI.Close();
		}
		foreach (ShopItemSlotUI shopItemSlotUI in shopItemSlotUIs)
		{
			shopItemSlotUI.Close();
		}
	}

	private int ComputeShopItemUIIndexInShop(ShopItemUI shopItemUI)
	{
		List<ShopItemPool> list = new List<ShopItemPool>();
		ShopItemPool[] pools = shopData.pools;
		foreach (ShopItemPool shopItemPool in pools)
		{
			for (int j = 0; j < shopItemPool.nSlotsMax; j++)
			{
				list.Add(shopItemPool);
			}
		}
		for (int k = 0; k < NMaxShopItems; k++)
		{
			bool flag = false;
			foreach (ShopItemUI shopItemUI2 in shopItemUIs)
			{
				flag = flag || shopItemUI2.IndexInShop == k;
			}
			if (!flag && (shopItemUI.shopItemData.ShopItemTypeEnum == list[k].type || (shopItemUI.shopItemData.ShopItemTypeEnum == ShopItemTypeEnum.shopUpgrade && ((ShopUpgradeShopItem)shopItemUI.shopItemData).shopItemTypeToIncrease == list[k].type)))
			{
				return k;
			}
		}
		Debug.Log((object)"Error in ComputeShopItemUIIndexInShop. Should not get here");
		return -1;
	}

	private Vector3 ShopItemUITargetPosition(int i)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.right * ShopItemsDeltaX * ((float)i - 0.5f * (float)(NMaxShopItems - 1));
	}

	private IEnumerator RerollShopService()
	{
		ShopItemData[] itemsToPrefereblyAvoid = ShopItemDataCurrentlySelling;
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			shopItemUI.Close();
		}
		SoundEffectsManager.Instance.Play("Reroll");
		yield return (object)new WaitForSeconds(0.3f);
		while (shopItemUIs.Count > 0)
		{
			RemoveShopItemUI(shopItemUIs[0]);
		}
		InstantiateShopItems(null, itemsToPrefereblyAvoid);
		foreach (ShopItemUI shopItemUI2 in shopItemUIs)
		{
			shopItemUI2.Open();
		}
		EventsManager.Instance.SaveRunProgress.Invoke();
		ItemBoughtSequenceOver();
	}

	private void HealShopService()
	{
		EffectsManager.Instance.CreateInGameEffect("HealEffect", ((Component)Globals.Hero.AgentGraphics).transform);
		Globals.Hero.AddToHealth((int)Mathf.Ceil((float)Globals.Hero.AgentStats.maxHP / 2f));
		ItemBoughtSequenceOver();
	}

	private IEnumerator GetMoneyShopService(ShopItemUI shopItemUI)
	{
		Globals.Coins += 5;
		SoundEffectsManager.Instance.Play("MoneySpent");
		while ((Object)(object)shopItemUI != (Object)null)
		{
			yield return null;
		}
		ItemBoughtSequenceOver();
	}

	private void PerformShopService(ShopItemUI shopItemUI)
	{
		switch (((ServiceShopItem)shopItemUI.shopItemData).shopServiceEnum)
		{
		case ShopServiceEnum.reroll:
			((MonoBehaviour)this).StartCoroutine(RerollShopService());
			break;
		case ShopServiceEnum.getMoney:
			((MonoBehaviour)this).StartCoroutine(GetMoneyShopService(shopItemUI));
			break;
		}
	}

	private void ItemBoughtSequenceOver()
	{
		SetShopItemUIsInteractable(interactable: true);
		if (Globals.Options.controlScheme == Options.ControlScheme.Gamepad)
		{
			SelectNewTargetAfterItemBoughtSequenceIsOver();
		}
	}

	private void SetShopItemUIsInteractable(bool interactable)
	{
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			shopItemUI.Interactable = interactable;
		}
	}

	public void PopulateShopInRunSaveData(ShopInRunSaveData shopInRunSaveData)
	{
		foreach (ShopItemUI shopItemUI in shopItemUIs)
		{
			shopInRunSaveData.shopItemDataNames.Add(((Object)shopItemUI.shopItemData).name);
			shopInRunSaveData.onSale.Add(shopItemUI.OnSale);
		}
		shopInRunSaveData.alreadyUpgraded = shopAlreadyUpgraded;
		shopInRunSaveData.freeConsumableAlreadyGiven = freeConsumableAlreadyGiven;
		shopInRunSaveData.shouldGiveFreeConsumable = shouldGiveFreeConsumable;
	}

	public void PopulateSaveData(SaveData saveData)
	{
		saveData.SetShopSaveData(shopSaveData);
	}

	public void LoadFromSaveData(SaveData saveData)
	{
		shopSaveData = (ShopSaveData)SaveData.GetNamedSaveData(saveData.shopSaveData, shopData.technicalName);
		if (shopSaveData == null)
		{
			ShopItemTypeEnum[] shopItemTypes = shopData.pools.Select((ShopItemPool pool) => pool.type).ToArray();
			int[] slotsPerType = shopData.pools.Select((ShopItemPool poo) => poo.nSlotsInitial).ToArray();
			shopSaveData = new ShopSaveData(shopData.technicalName, shopItemTypes, slotsPerType);
		}
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		if (SelectedTarget == null)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.down);
		}
		int num = shopItemUIs.IndexOf((ShopItemUI)SelectedTarget);
		if (navigationDirection == NavigationDirection.left)
		{
			num--;
		}
		if (navigationDirection == NavigationDirection.right)
		{
			num++;
		}
		if (num < 0 || navigationDirection == NavigationDirection.down || navigationDirection == NavigationDirection.up)
		{
			return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
		}
		num = Mathf.Clamp(num, 0, shopItemUIs.Count - 1);
		SelectNewTarget(shopItemUIs[num]);
		return this;
	}

	private void SelectNewTarget(ShopItemUI shopItemUI)
	{
		indexInShopOfLastSelectedTarget = shopItemUI.IndexInShop;
		UINavigationHelper.SelectNewTarget(this, shopItemUI);
	}

	private void SelectNewTargetAfterItemBoughtSequenceIsOver()
	{
		ShopItemUI shopItemUI = null;
		int num = int.MaxValue;
		foreach (ShopItemUI item in Enumerable.Reverse(shopItemUIs))
		{
			if (Mathf.Abs(indexInShopOfLastSelectedTarget - item.IndexInShop) < num)
			{
				num = Mathf.Abs(indexInShopOfLastSelectedTarget - item.IndexInShop);
				shopItemUI = item;
			}
		}
		if ((Object)(object)shopItemUI != (Object)null)
		{
			SelectNewTarget(shopItemUI);
		}
	}

	public void OnEntry(NavigationDirection entryDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		if (shopItemUIs.Count != 0)
		{
			SelectNewTarget(shopItemUIs[0]);
		}
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		SelectedTarget?.Submit();
		return this;
	}
}
