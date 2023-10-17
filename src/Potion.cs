using System.Collections.Generic;
using InfoBoxUtils;
using UINavigation;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public abstract class Potion : MonoBehaviour, IInfoBoxable, INavigationTarget
{
	[SerializeField]
	private SpriteRenderer mainSpriteRenderer;

	[SerializeField]
	private SpriteRenderer outlineSpriteRenderer;

	private InfoBoxActivator infoBoxActivator;

	private MyButton sellConsumableButton;

	public abstract PotionsManager.PotionEnum PotionEnum { get; }

	public abstract string LocalizationTableKey { get; }

	public abstract List<CombatSceneManager.Mode> AllowedModes { get; }

	public abstract int PriceForHeroSellingPotion { get; }

	public string Name => LocalizationUtils.LocalizedString("Consumables", LocalizationTableKey + "_Name");

	public string Description => ProcessDescription(LocalizationUtils.LocalizedString("Consumables", LocalizationTableKey + "_Description"));

	public MyButton SellButton => sellConsumableButton;

	private bool Highlighted { get; set; }

	public bool AlreadyUsed { get; private set; }

	public bool CanBeUsed
	{
		get
		{
			if (!CanBeUsedInCurrentMode)
			{
				return false;
			}
			if (CombatManager.Instance.CombatInProgress)
			{
				return !CombatManager.Instance.TurnInProgress;
			}
			return true;
		}
	}

	private bool CanBeUsedInCurrentMode => AllowedModes.Contains(CombatSceneManager.Instance.CurrentMode);

	public bool CanBeSold
	{
		get
		{
			return sellConsumableButton.Interactable;
		}
		set
		{
			sellConsumableButton.Interactable = value;
			if (value)
			{
				sellConsumableButton.Appear();
			}
			else
			{
				sellConsumableButton.Disappear();
			}
		}
	}

	public Transform Transform => ((Component)this).transform;

	public string InfoBoxText => InfoBox.StandardInfoBoxFormatting(Name, Description);

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	protected abstract void Effect();

	protected virtual string ProcessDescription(string description)
	{
		return description;
	}

	private void Awake()
	{
		((Renderer)outlineSpriteRenderer).enabled = false;
		AlreadyUsed = false;
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
		sellConsumableButton = ((Component)this).GetComponentInChildren<MyButton>();
		string key = ((PriceForHeroSellingPotion > 1) ? "SellForCoins" : "SellForCoin");
		string s = string.Format(LocalizationUtils.LocalizedString("Terms", key), Name, PriceForHeroSellingPotion);
		sellConsumableButton.SetInfoBoxText(TextUitls.ReplaceTags(s));
	}

	private void Start()
	{
		CanBeSold = (Object)(object)CombatSceneManager.Instance != (Object)null && (Object)(object)CombatSceneManager.Instance.Room != (Object)null && CombatSceneManager.Instance.Room is ShopRoom && CombatSceneManager.Instance.CurrentMode != CombatSceneManager.Mode.mapSelection;
		EventsManager.Instance.ModeSwitched.AddListener((UnityAction<CombatSceneManager.Mode>)ModeSwitched);
		UpdateGraphics();
		AppearAnimation();
	}

	public void Select()
	{
		Highlighted = true;
		SoundEffectsManager.Instance.Play("PotionHighlight");
		infoBoxActivator.Open();
		UpdateGraphics();
	}

	public void Deselect()
	{
		Highlighted = false;
		infoBoxActivator.Close();
		UpdateGraphics();
	}

	public void Submit()
	{
		if (!AlreadyUsed)
		{
			if (!CanBeUsed)
			{
				SoundEffectsManager.Instance.Play("CannotPerformAction");
				return;
			}
			AlreadyUsed = true;
			EventsManager.Instance.PotionUsed.Invoke(this);
			Effect();
			SoundEffectsManager.Instance.Play("PotionUsed");
			DisappearAnimationAndDestroy();
		}
	}

	public void SellConsumable()
	{
		if (!AlreadyUsed)
		{
			AlreadyUsed = true;
			SoundEffectsManager.Instance.Play("MoneySpent");
			sellConsumableButton.Disappear();
			DisappearAnimationAndDestroy();
			Globals.Coins += PriceForHeroSellingPotion;
		}
	}

	private void UpdateGraphics()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		((Renderer)outlineSpriteRenderer).enabled = Highlighted;
		if (CanBeUsedInCurrentMode)
		{
			mainSpriteRenderer.color = Color.white;
			outlineSpriteRenderer.color = Color.white;
		}
		else
		{
			mainSpriteRenderer.color = Color.gray;
			outlineSpriteRenderer.color = Color.gray;
		}
	}

	private void ModeSwitched(CombatSceneManager.Mode mode)
	{
		UpdateGraphics();
		if (mode == CombatSceneManager.Mode.mapSelection || mode == CombatSceneManager.Mode.transition)
		{
			CanBeSold = false;
		}
	}

	private void AppearAnimation()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localScale = Vector3.zero;
		LeanTween.scale(((Component)this).gameObject, Vector3.one, 0.2f).setEase((LeanTweenType)27);
	}

	private void DisappearAnimationAndDestroy()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		EffectsManager.Instance.CreateInGameEffect("PotionUsedEffect", ((Component)this).transform.position);
		((Component)this).transform.localScale = Vector3.one;
		LeanTween.scale(((Component)this).gameObject, Vector3.zero, 0.2f).setEase((LeanTweenType)26);
		Object.Destroy((Object)(object)((Component)this).gameObject, 0.2f);
	}
}
