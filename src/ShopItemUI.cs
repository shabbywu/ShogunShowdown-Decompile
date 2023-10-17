using InfoBoxUtils;
using ShopStuff;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IInfoBoxable, IBuyable, INavigationTarget
{
	public InfoBoxActivator infoBoxActivator;

	public Price price;

	[Header("Sprite renderers")]
	public SpriteRenderer itemSpriteRend;

	public SpriteRenderer itemBackgroundSpriteRend;

	public SpriteRenderer bodySpriteRend;

	public SpriteRenderer onSaleIconSpriteRend;

	private Vector3 currentVelocity;

	private Animator animator;

	private bool pointerOnMe;

	private bool _interactable;

	private bool alreadyBought;

	private UpdateValueEvent updateValueEvent;

	private float smoothtime = 0.06f;

	private Shop shop;

	public ShopItemData shopItemData;

	public Vector3 TargetPosition { get; set; }

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			_interactable = value;
			if (_interactable && pointerOnMe)
			{
				Highlight();
			}
		}
	}

	public bool OnSale { get; private set; }

	public int IndexInShop { get; set; }

	public ShopItemTypeEnum SlotType
	{
		get
		{
			if (shopItemData.ShopItemTypeEnum == ShopItemTypeEnum.shopUpgrade)
			{
				return ((ShopUpgradeShopItem)shopItemData).shopItemTypeToIncrease;
			}
			return shopItemData.ShopItemTypeEnum;
		}
	}

	public string InfoBoxText { get; protected set; }

	public bool InfoBoxEnabled
	{
		get
		{
			if (Interactable)
			{
				return !alreadyBought;
			}
			return false;
		}
	}

	public BoxWidth BoxWidth { get; private set; } = BoxWidth.medium;


	public Transform Transform => ((Component)this).transform;

	public void Open()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localScale = new Vector3(0f, 0f, 1f);
		animator.SetTrigger("Open");
	}

	public void Close()
	{
		animator.SetTrigger("Close");
	}

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = Vector3.SmoothDamp(((Component)this).transform.localPosition, TargetPosition, ref currentVelocity, smoothtime);
	}

	public void Initialize(ShopItemData shopItemData, Shop shop, bool onSale = false)
	{
		animator = ((Component)this).GetComponent<Animator>();
		this.shopItemData = shopItemData;
		this.shop = shop;
		OnSale = onSale && shopItemData.price > 1;
		price.buyable = this;
		price.Currency = shopItemData.currency;
		price.Value = shopItemData.price;
		if (OnSale)
		{
			price.OnSale = true;
			price.Value = Mathf.FloorToInt((float)price.Value / 2f);
		}
		((Renderer)onSaleIconSpriteRend).enabled = OnSale;
		InitializeInfoBoxText();
		InitializeItemGraphics();
		Interactable = true;
	}

	private void InitializeItemGraphics()
	{
		bodySpriteRend.sprite = shopItemData.idleSprite;
		itemSpriteRend.sprite = shopItemData.Sprite;
	}

	public void BuyAnimationOver()
	{
		shop.RemoveShopItemUIAfterBuying(this);
	}

	private void InitializeInfoBoxText()
	{
		InfoBoxText = TextUitls.SingleLineHeader(shopItemData.Name) + "[vspace]\n";
		InfoBoxText = InfoBoxText + "<color=#" + shopItemData.itemTypeColorHex + ">" + shopItemData.ItemTypeName + "[end_color]";
		InfoBoxText += "\n";
		InfoBoxText += shopItemData.Description;
		InfoBoxText = TextUitls.ReplaceTags(InfoBoxText);
	}

	protected virtual void Buy()
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		EventsManager.Instance.ShopItemBought.Invoke($"{((Object)shopItemData).name} Price:{price.Value} Currency:{price.Currency}");
		shop.ItemBought(this);
		alreadyBought = true;
		price.Pay();
		price.Value = -1;
		SoundEffectsManager.Instance.Play("MoneySpent");
		SoundEffectsManager.Instance.Play("MenuItemSubmit");
		EffectsManager.Instance.CreateInGameEffect("ShopItemBoughtEffect", ((Component)this).transform.position);
		animator.SetTrigger("Selected");
		shopItemData.Buy();
		infoBoxActivator.Close();
	}

	private void Highlight()
	{
		animator.SetBool("Highlighted", true);
		SoundEffectsManager.Instance.Play("MenuItemHighlight");
		infoBoxActivator.Open();
		if (price.CanAfford)
		{
			bodySpriteRend.sprite = shopItemData.highlighCanAffordSprite;
		}
		else
		{
			bodySpriteRend.sprite = shopItemData.highlighCannotAffordSprite;
		}
		if (Globals.Hero.AllowExternallyImposingFacingDir)
		{
			Globals.Hero.LookAt(((Component)this).transform);
		}
	}

	private void Normal()
	{
		animator.SetBool("Highlighted", false);
		infoBoxActivator.Close();
		bodySpriteRend.sprite = shopItemData.idleSprite;
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		Select();
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		Deselect();
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)eventData.button == 0)
		{
			Submit();
		}
	}

	public void CanAffordUpdate(bool canAfford)
	{
		if (!alreadyBought)
		{
			animator.SetBool("CanBuy", canAfford);
		}
	}

	public void Select()
	{
		pointerOnMe = true;
		if (Interactable && !alreadyBought)
		{
			Highlight();
		}
	}

	public void Deselect()
	{
		pointerOnMe = false;
		if (Interactable && !alreadyBought)
		{
			Normal();
		}
	}

	public void Submit()
	{
		if (Interactable && !alreadyBought)
		{
			if (price.CanAfford && Interactable)
			{
				Buy();
				return;
			}
			animator.SetTrigger("Selected");
			SoundEffectsManager.Instance.Play("CannotPerformAction");
		}
	}
}
