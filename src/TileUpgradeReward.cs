using System.Collections;
using System.Collections.Generic;
using InfoBoxUtils;
using TileEnums;
using TilesUtils;
using UINavigation;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class TileUpgradeReward : Reward, ITilesChanged, IInfoBoxable, INavigationGroup
{
	public List<TileUpgradeOptions> tileUpgradeOptions;

	[SerializeField]
	private TileUpgrade fallbackTileUpgrade;

	private EventTileContainer cc;

	private MyButton button;

	private Animator _animator;

	private PseudoRandomWithMemory<TileUpgrade> tileUpgradeRewardGen;

	private bool preMapInfoBoxWasOpen;

	private InfoBoxActivator infoBoxActivator;

	private Animator Animator
	{
		get
		{
			if ((Object)(object)_animator == (Object)null)
			{
				_animator = ((Component)this).GetComponent<Animator>();
			}
			return _animator;
		}
	}

	public TileUpgrade TileUpgrade { get; set; }

	public bool InfoBoxAlwaysOpen { get; set; }

	public bool ButtonAlwaysOpen { get; set; }

	public bool UpgradeAnimationInProgress { get; private set; }

	public string InfoBoxText
	{
		get
		{
			string text = "[begin_large]" + TileUpgrade.Description + "[end_large]";
			if (TileUpgrade.Details != "")
			{
				text = text + "\n[vspace]" + TileUpgrade.Details;
			}
			return TextUitls.ReplaceTags(text);
		}
	}

	public bool InfoBoxEnabled => true;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public override bool Rerollable => true;

	public List<INavigationTarget> Targets
	{
		get
		{
			if (cc.HasTile)
			{
				return new List<INavigationTarget> { cc };
			}
			return new List<INavigationTarget>();
		}
	}

	public INavigationTarget SelectedTarget { get; set; }

	public Dictionary<NavigationDirection, INavigationGroup> ConnectedGroups { get; set; }

	public bool CanBeNavigatedTo => cc.HasTile;

	public override void Initialize()
	{
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
		cc = ((Component)this).GetComponentInChildren<EventTileContainer>();
		button = ((Component)this).GetComponentInChildren<MyButton>();
		cc.NotifyTileChanged = this;
		if (tileUpgradeRewardGen == null)
		{
			InitializRewardsGenerator();
		}
		Disable();
	}

	public override void Reroll()
	{
		EventsManager.Instance.PlayerChoiceInTileUpgradeReward.Invoke($"{TileUpgrade.TechName} Payed:{base.IsPayedReward} Rerolled");
		((MonoBehaviour)this).StartCoroutine(RerollCoroutine());
	}

	public override void Skipped()
	{
		base.Skipped();
		EventsManager.Instance.PlayerChoiceInTileUpgradeReward.Invoke($"{TileUpgrade.TechName} Payed:{base.IsPayedReward} Skipped");
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		EventsManager.Instance.MapOpened.AddListener(new UnityAction(MapOpened));
		EventsManager.Instance.MapClosed.AddListener(new UnityAction(MapClosed));
	}

	private void OnDestroy()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		EventsManager.Instance.MapOpened.RemoveListener(new UnityAction(MapOpened));
		EventsManager.Instance.MapClosed.RemoveListener(new UnityAction(MapClosed));
	}

	private IEnumerator RerollCoroutine()
	{
		EventsManager.Instance.RewardBusy.Invoke();
		Disable();
		Animator.SetTrigger("Reroll");
		yield return (object)new WaitForSeconds(0.3f);
		TileUpgrade = GetNextRandomUpgrade();
		Enable();
	}

	public void ShowUpgradeDescription()
	{
		infoBoxActivator.Open();
	}

	public void HideUpgradeDescription()
	{
		infoBoxActivator.Close();
	}

	public override void StartEvent(RewardSaveData rewardSaveData)
	{
		base.InProgress = true;
		if (rewardSaveData != null && rewardSaveData.inProgress)
		{
			TileUpgrade = tileUpgradeOptions.Find((TileUpgradeOptions o) => o.tileUpgrade.TileUpgradeEnum == rewardSaveData.tileUpgradeEnum).tileUpgrade;
		}
		if ((Object)(object)TileUpgrade == (Object)null)
		{
			TileUpgrade = GetNextRandomUpgrade();
		}
		Enable();
		TilesManager.Instance.CanInteractWithTiles = true;
		TilesManager.Instance.EventTargetContainer = cc;
	}

	public override void EndEvent()
	{
		Disable();
		TilesManager.Instance.EventTargetContainer = null;
		TilesManager.Instance.CanInteractWithTiles = false;
		EnebleAllTiles();
		base.InProgress = false;
	}

	private IEnumerator UpgradeAndEndEvent()
	{
		EventsManager.Instance.PlayerChoiceInTileUpgradeReward.Invoke($"{TileUpgrade.TechName} Payed:{base.IsPayedReward} Picked:{cc.Tile.Attack.TechNameAndStats}");
		UpgradeAnimationInProgress = true;
		TileUpgrade.Upgrade(cc.Tile);
		if (cc.HasTile)
		{
			EventsManager.Instance.TileUpgraded.Invoke(cc.Tile);
		}
		TilesManager.Instance.CanInteractWithTiles = false;
		EventsManager.Instance.EndTileUpgradeMode.Invoke();
		UpgradeEffectAnimation();
		yield return (object)new WaitForSeconds(1.3f);
		SoundEffectsManager.Instance.Play("TileSubmit");
		EndEvent();
		UpgradeAnimationInProgress = false;
	}

	public void DecisionTaken()
	{
		if (cc.HasTile)
		{
			button.Interactable = false;
			base.Exausted = true;
			EventsManager.Instance.RewardsScreenUpgradeChoosen.Invoke();
			((MonoBehaviour)this).StartCoroutine(UpgradeAndEndEvent());
		}
	}

	public void UpgradeEffectAnimation()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("GenericUpgrade");
		SoundEffectsManager.Instance.Play("Spawn");
		EffectsManager.Instance.CreateInGameEffect("TileUpgradeEffect", ((Component)cc).transform.position);
		if (cc.HasTile)
		{
			EffectsManager.Instance.WaitAndCreateInGameEffect("TileShineEffect", ((Component)cc.Tile).transform, 0.65f);
		}
		EffectsManager.Instance.ScreenShake();
		Animator.SetTrigger("Upgrade");
		Animator.SetBool("Enabled", false);
		if (!ButtonAlwaysOpen)
		{
			button.Disappear();
		}
	}

	void ITilesChanged.TilesChanged()
	{
		button.Interactable = cc.HasTile;
		cc.Highlight(cc.HasTile);
		if (cc.HasTile && Globals.Hero.AllowExternallyImposingFacingDir)
		{
			Globals.Hero.LookAt(((Component)this).transform);
		}
	}

	private void EnebleAllTiles()
	{
		Tile[] tiles = TilesManager.Instance.hand.TCC.Tiles;
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].TileIsEnabled = true;
		}
	}

	public void ButtonHighlighted(bool value)
	{
		if (value)
		{
			Animator.SetBool("Highlighted", cc.HasTile);
			if (cc.HasTile && Globals.Hero.AllowExternallyImposingFacingDir)
			{
				Globals.Hero.LookAt(((Component)this).transform);
			}
		}
		else
		{
			Animator.SetBool("Highlighted", false);
		}
	}

	private void Enable()
	{
		SoundEffectsManager.Instance.Play("KomainuOpenMouth");
		Animator.SetBool("Enabled", true);
		infoBoxActivator.Open();
		button.Appear();
		button.Interactable = false;
		button.SetText(TileUpgrade.ButtonText);
		EventsManager.Instance.BeginTileUpgradeMode.Invoke(TileUpgrade);
		EventsManager.Instance.RewardReady.Invoke();
		cc.Interactable = true;
	}

	private void Disable()
	{
		if (cc.HasTile)
		{
			TilesManager.Instance.TakeTile(cc.RemoveTile());
		}
		Animator.SetBool("Enabled", false);
		if (!InfoBoxAlwaysOpen)
		{
			infoBoxActivator.Close();
		}
		if (!ButtonAlwaysOpen)
		{
			button.Disappear();
		}
		EventsManager.Instance.EndTileUpgradeMode.Invoke();
		cc.Interactable = false;
	}

	public void Appear()
	{
		Animator.SetTrigger("Appear");
	}

	public void Disappear()
	{
		Animator.SetTrigger("Disappear");
	}

	public void Hide()
	{
		Animator.SetTrigger("Hide");
	}

	private void InitializRewardsGenerator()
	{
		List<(TileUpgrade, float)> list = new List<(TileUpgrade, float)>();
		foreach (TileUpgradeOptions tileUpgradeOption in tileUpgradeOptions)
		{
			list.Add((tileUpgradeOption.tileUpgrade, tileUpgradeOption.probability));
		}
		tileUpgradeRewardGen = new PseudoRandomWithMemory<TileUpgrade>(list.ToArray(), 1.2f, allowSameConsecutiveResults: false);
	}

	private TileUpgrade GetNextRandomUpgrade()
	{
		for (int i = 0; i < 100; i++)
		{
			TileUpgrade next = tileUpgradeRewardGen.GetNext();
			if (next.CanBeOfferedGivenThisDeck(TilesManager.Instance.Deck))
			{
				return next;
			}
		}
		Debug.Log((object)"Could not find a fulfillable tile upgrade. Using fallback tile upgrade.");
		return fallbackTileUpgrade;
	}

	private void MapOpened()
	{
		preMapInfoBoxWasOpen = (Object)(object)infoBoxActivator != (Object)null && infoBoxActivator.InfoBoxIsOpen;
		if (preMapInfoBoxWasOpen)
		{
			infoBoxActivator.Close();
		}
	}

	private void MapClosed()
	{
		if (preMapInfoBoxWasOpen)
		{
			infoBoxActivator.Open();
		}
	}

	public override void PopulateSaveData(RewardSaveData rewardSaveData)
	{
		rewardSaveData.inProgress = base.InProgress;
		rewardSaveData.rewardExausted = base.Exausted;
		rewardSaveData.tileUpgradeEnum = ((!((Object)(object)TileUpgrade == (Object)null)) ? TileUpgrade.TileUpgradeEnum : TileUpgradeEnum.none);
	}

	public INavigationGroup Navigate(NavigationDirection navigationDirection)
	{
		return UINavigationHelper.HandleOutOfGroupNavigation(this, navigationDirection);
	}

	public void OnEntry(NavigationDirection navigationDirection, INavigationTarget previousTarget = null, Vector3? entryPosition = null)
	{
		UINavigationHelper.SelectNewTarget(this, cc);
	}

	public INavigationGroup SubmitCurrentTarget()
	{
		cc.Submit();
		return UINavigationHelper.HandleOutOfGroupNavigation(this, NavigationDirection.down, NavigationDirection.left);
	}
}
