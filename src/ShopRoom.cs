using System.Collections;
using UnityEngine;

public class ShopRoom : Room
{
	[SerializeField]
	private Transform leftTransform;

	[SerializeField]
	private Transform rightTransform;

	[SerializeField]
	private MyButton goButton;

	private TileUpgradeInShop tileUpgradeInShop;

	private Shop shop;

	private ShopInRunSaveData shopInRunSaveData;

	private RewardSaveData rewardSaveData;

	public TileUpgradeReward TileUpgradeReward => tileUpgradeInShop.tileUpgradeReward;

	public Shop Shop => shop;

	public override void Begin()
	{
		base.Name = "shop";
		Globals.TilesInfoMode = true;
		CombatSceneManager.Instance.CurrentMode = CombatSceneManager.Mode.reward;
		Globals.Hero.AllowExternallyImposingFacingDir = true;
		Globals.Hero.SetCombatUIActive(value: true);
		TilesManager.Instance.ShowTilesLevel(value: true);
		TilesManager.Instance.CanInteractWithTiles = false;
		tileUpgradeInShop.Begin(rewardSaveData);
		shop.Begin();
		((Component)goButton).gameObject.SetActive(true);
		goButton.Appear();
		Potion[] heldPotions = PotionsManager.Instance.HeldPotions;
		for (int i = 0; i < heldPotions.Length; i++)
		{
			heldPotions[i].CanBeSold = true;
		}
		EventsManager.Instance.ShopBegin.Invoke();
	}

	public override void End()
	{
		tileUpgradeInShop.End();
		shop.End();
		Potion[] heldPotions = PotionsManager.Instance.HeldPotions;
		for (int i = 0; i < heldPotions.Length; i++)
		{
			heldPotions[i].CanBeSold = false;
		}
		Globals.TilesInfoMode = false;
		if (Globals.FullInfoMode)
		{
			CombatSceneManager.Instance.DisableInfoMode();
		}
		TilesManager.Instance.CanInteractWithTiles = false;
		Globals.Hero.AllowExternallyImposingFacingDir = false;
		EventsManager.Instance.ShopEnd.Invoke();
		base.End();
	}

	public void InitializeShop(ShopComponent left, ShopComponent right)
	{
		tileUpgradeInShop = Object.Instantiate<GameObject>(((Component)left).gameObject, leftTransform).GetComponent<TileUpgradeInShop>();
		shop = Object.Instantiate<GameObject>(((Component)right).gameObject, rightTransform).GetComponent<Shop>();
		shop.Initialize(shopInRunSaveData);
		ShopEnvironment componentInChildren = ((Component)this).GetComponentInChildren<ShopEnvironment>();
		componentInChildren.InitializeLeft(tileUpgradeInShop);
		componentInChildren.InitializeRight(Progression.Instance.CurrentLocation.island);
	}

	public override IEnumerator ProcessTurn()
	{
		yield return null;
	}

	public override void PopulateSaveData(RunSaveData runSaveData)
	{
		base.PopulateSaveData(runSaveData);
		runSaveData.shopRoom = new ShopRoomSaveData();
		runSaveData.shopRoom.leftShopComponentName = ((Component)leftTransform).GetComponentInChildren<ShopComponent>().technicalName;
		runSaveData.shopRoom.rightShopComponentName = ((Component)rightTransform).GetComponentInChildren<ShopComponent>().technicalName;
		tileUpgradeInShop.tileUpgradeReward.PopulateSaveData(runSaveData.shopRoom.reward);
		runSaveData.shopRoom.reward.inProgress = true;
		runSaveData.shopRoom.reward.price = tileUpgradeInShop.price.Value;
		shop.PopulateShopInRunSaveData(runSaveData.shopRoom.shopInRun);
	}

	public override void LoadFromSaveData(RunSaveData runSaveData)
	{
		base.LoadFromSaveData(runSaveData);
		rewardSaveData = runSaveData.shopRoom.reward;
		shopInRunSaveData = runSaveData.shopRoom.shopInRun;
	}
}
