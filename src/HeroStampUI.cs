using InfoBoxUtils;
using ProgressionEnums;
using UINavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

public class HeroStampUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IInfoBoxable, INavigationTarget
{
	public Image stampImage;

	public Image highalightImage;

	public HeroStampChallenge stampChallenge;

	public Sprite stampNoRank;

	public Sprite stampRegularRank;

	public Sprite stampUltimateRank;

	private Animator animator;

	public InfoBoxActivator infoBoxActivator;

	private Hero hero;

	private bool Highlighted
	{
		set
		{
			((Behaviour)highalightImage).enabled = value;
			if (value)
			{
				infoBoxActivator.Open();
			}
			else
			{
				infoBoxActivator.Close();
			}
		}
	}

	public string InfoBoxText
	{
		get
		{
			string text = stampChallenge.Description;
			string text2 = stampChallenge.StampName;
			if ((Object)(object)hero != (Object)null)
			{
				if (hero.CharacterSaveData.GetHeroStampRank(stampChallenge.HeroStampEnum) == HeroStampRank.ultimate)
				{
					text2 = string.Format(LocalizationUtils.LocalizedString("Terms", "Ultimate"), text2);
				}
				if (stampChallenge.ShowUltimateDescription)
				{
					text = text + "\n[vspace]" + stampChallenge.UltimateDescription;
				}
			}
			return InfoBox.StandardInfoBoxFormatting(text2, text);
		}
	}

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public Transform Transform => ((Component)this).transform;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.NewDifficultySelected.AddListener(new UnityAction(UpdateSpriteUsingGlobalPlayer));
		Initialize(Globals.Hero);
	}

	public void Initialize(Hero hero)
	{
		this.hero = hero;
		animator = ((Component)this).GetComponent<Animator>();
		UpdateSprite(hero);
	}

	public void UnlockAnimation()
	{
		animator.SetTrigger("Unlock");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Select();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Deselect();
	}

	public void UpdateSprite(Hero hero)
	{
		switch (hero.CharacterSaveData.GetHeroStampRank(stampChallenge.HeroStampEnum))
		{
		case HeroStampRank.noRank:
			stampImage.sprite = stampNoRank;
			break;
		case HeroStampRank.regular:
			stampImage.sprite = stampRegularRank;
			break;
		case HeroStampRank.ultimate:
			stampImage.sprite = stampUltimateRank;
			break;
		}
	}

	public void UnlockAnimationEffect()
	{
		SoundEffectsManager.Instance.Play("UnlockHeroStamp");
		EffectsManager.Instance.ScreenShake();
	}

	public void UpdateSpriteUsingGlobalPlayer()
	{
		UpdateSprite(Globals.Hero);
	}

	public virtual void Select()
	{
		Highlighted = true;
	}

	public virtual void Deselect()
	{
		Highlighted = false;
	}

	public virtual void Submit()
	{
	}
}
