using ShopStuff;
using UnityEngine;

public class RewardRerolling : MonoBehaviour
{
	public Price price;

	public RewardRoom rewardsScreen;

	public MyButton rerollButton;

	private int basePrice;

	public int RerollPrice { get; set; } = -1;


	private bool CanAfford => Globals.Coins >= RerollPrice;

	public void Hide()
	{
		rerollButton.Disappear();
	}

	public void StartEvent(bool rerollable)
	{
		if (rerollable)
		{
			rerollButton.Appear();
			if (MapManager.Instance.Sector == 1)
			{
				basePrice = 3;
			}
			else if (MapManager.Instance.Sector <= 3)
			{
				basePrice = 4;
			}
			else if (MapManager.Instance.Sector <= 5)
			{
				basePrice = 5;
			}
			else
			{
				basePrice = 6;
			}
			if (RerollPrice == -1)
			{
				RerollPrice = basePrice;
			}
			UpdateState(allowButtonInteraction: false);
		}
	}

	public void EndEvent()
	{
		rerollButton.Disappear();
	}

	public void RerollButtonPressed()
	{
		rewardsScreen.RerollButtonPressed();
		Globals.Coins -= RerollPrice;
		SoundEffectsManager.Instance.Play("Reroll");
		SoundEffectsManager.Instance.Play("MoneySpent");
		RerollPrice += basePrice;
		UpdateState(allowButtonInteraction: false);
	}

	public void UpdateState(bool allowButtonInteraction)
	{
		price.Currency = CurrencyEnum.coins;
		price.Value = RerollPrice;
		rerollButton.Interactable = allowButtonInteraction && CanAfford;
	}
}
