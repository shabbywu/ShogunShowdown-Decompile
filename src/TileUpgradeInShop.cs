using ShopStuff;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class TileUpgradeInShop : MonoBehaviour, IBuyable
{
	public int basePrice;

	public Price price;

	public TileUpgrade[] tileUpgrades;

	public TileUpgradeReward tileUpgradeReward;

	public Sprite[] backgrounds;

	public Color color;

	private bool inProgress;

	private void Update()
	{
		if (inProgress && !tileUpgradeReward.UpgradeAnimationInProgress)
		{
			if (!tileUpgradeReward.InProgress && price.CanAfford)
			{
				tileUpgradeReward.StartEvent(null);
			}
			if (tileUpgradeReward.InProgress && !price.CanAfford)
			{
				tileUpgradeReward.EndEvent();
			}
		}
	}

	private void Start()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		tileUpgradeReward.Initialize();
		tileUpgradeReward.InfoBoxAlwaysOpen = true;
		tileUpgradeReward.ButtonAlwaysOpen = true;
		tileUpgradeReward.IsPayedReward = true;
		EventsManager.Instance.RewardsScreenUpgradeChoosen.AddListener(new UnityAction(TileUpgradeHasBeenSelected));
	}

	~TileUpgradeInShop()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		try
		{
			EventsManager.Instance.RewardsScreenUpgradeChoosen.RemoveListener(new UnityAction(TileUpgradeHasBeenSelected));
		}
		finally
		{
			((object)this).Finalize();
		}
	}

	public void Begin(RewardSaveData rewardSaveData = null)
	{
		inProgress = true;
		price.buyable = this;
		price.Currency = CurrencyEnum.coins;
		if (rewardSaveData == null)
		{
			price.Value = basePrice;
			tileUpgradeReward.TileUpgrade = MyRandom.NextFromArray(tileUpgrades);
			tileUpgradeReward.StartEvent(null);
		}
		else
		{
			price.Value = rewardSaveData.price;
			tileUpgradeReward.StartEvent(rewardSaveData);
		}
		((Component)price).gameObject.SetActive(price.Value > 0);
	}

	public void End()
	{
		inProgress = false;
		tileUpgradeReward.InfoBoxAlwaysOpen = false;
		tileUpgradeReward.ButtonAlwaysOpen = false;
		tileUpgradeReward.EndEvent();
	}

	public void TileUpgradeHasBeenSelected()
	{
		price.Pay();
		price.Value += basePrice;
		EventsManager.Instance.SaveRunProgress.Invoke();
	}

	public void CanAffordUpdate(bool canAfford)
	{
	}
}
