using ShopStuff;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

public class Price : MonoBehaviour
{
	public Image currencyIconImage;

	public TextMeshProUGUI priceTextBlack;

	public TextMeshProUGUI priceTextWhite;

	public IBuyable buyable;

	[Header("Currencies sprites")]
	public Sprite coinsCurrencySprite;

	public Sprite metaCurrencySprite;

	public Sprite hpCurrencySprite;

	public Sprite maxHPCurrencySprite;

	private int _value;

	private CurrencyEnum _currency;

	private UpdateValueEvent updateValueEvent;

	public bool OnSale { get; set; }

	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			UpdatePrice(0);
		}
	}

	public CurrencyEnum Currency
	{
		get
		{
			return _currency;
		}
		set
		{
			_currency = value;
			InitializeCurrencyData();
			UpdatePrice(0);
		}
	}

	public bool CanAfford
	{
		get
		{
			switch (Currency)
			{
			case CurrencyEnum.coins:
				return Globals.Coins >= Value;
			case CurrencyEnum.meta:
				return Globals.KillCount >= Value;
			case CurrencyEnum.hp:
				return Globals.Hero.AgentStats.HP > Value;
			case CurrencyEnum.maxHP:
				return Globals.Hero.AgentStats.maxHP > Value;
			default:
				Debug.LogWarning((object)"Price CanAfford: I should not get here");
				return false;
			}
		}
	}

	public void Pay()
	{
		if (!CanAfford)
		{
			Debug.LogWarning((object)"Pay: I should not get here because I cannot afford");
		}
		switch (Currency)
		{
		case CurrencyEnum.coins:
			Globals.Coins -= Value;
			break;
		case CurrencyEnum.meta:
			Globals.KillCount -= Value;
			break;
		case CurrencyEnum.hp:
			Globals.Hero.AddToHealth(-Value);
			EffectsManager.Instance.CreateInGameEffect("HitEffect", ((Component)Globals.Hero).transform);
			break;
		case CurrencyEnum.maxHP:
			Globals.Hero.AgentStats.maxHP -= Value;
			break;
		}
	}

	~Price()
	{
		try
		{
			if (updateValueEvent != null)
			{
				((UnityEvent<int>)updateValueEvent).RemoveListener((UnityAction<int>)UpdatePrice);
			}
		}
		finally
		{
			((object)this).Finalize();
		}
	}

	private void InitializeCurrencyData()
	{
		if (updateValueEvent != null)
		{
			((UnityEvent<int>)updateValueEvent).RemoveListener((UnityAction<int>)UpdatePrice);
		}
		switch (Currency)
		{
		case CurrencyEnum.coins:
			currencyIconImage.sprite = coinsCurrencySprite;
			updateValueEvent = EventsManager.Instance.CoinsUpdate;
			break;
		case CurrencyEnum.meta:
			currencyIconImage.sprite = metaCurrencySprite;
			updateValueEvent = EventsManager.Instance.MetaCurrencyUpdate;
			break;
		case CurrencyEnum.hp:
			currencyIconImage.sprite = hpCurrencySprite;
			updateValueEvent = EventsManager.Instance.HeroHPUpdate;
			break;
		case CurrencyEnum.maxHP:
			currencyIconImage.sprite = maxHPCurrencySprite;
			updateValueEvent = EventsManager.Instance.HeroMaxHPUpdate;
			break;
		}
		((UnityEvent<int>)updateValueEvent).AddListener((UnityAction<int>)UpdatePrice);
	}

	private void UpdatePrice(int dummy)
	{
		if (buyable != null)
		{
			buyable.CanAffordUpdate(CanAfford);
		}
		if (Value < 0)
		{
			((TMP_Text)priceTextWhite).text = "--";
			((TMP_Text)priceTextBlack).text = "--";
			return;
		}
		((TMP_Text)priceTextWhite).text = $"{Value}";
		((TMP_Text)priceTextBlack).text = $"{Value}";
		if (!CanAfford)
		{
			((TMP_Text)priceTextWhite).text = "<color=#" + Colors.orangeHex + ">" + ((TMP_Text)priceTextWhite).text + "</color>";
		}
		else if (OnSale)
		{
			((TMP_Text)priceTextWhite).text = "<color=#" + Colors.birghtYellowHex + ">" + ((TMP_Text)priceTextWhite).text + "</color>";
		}
	}
}
