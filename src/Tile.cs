using System;
using System.Collections;
using InfoBoxUtils;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Tile : MonoBehaviour, IInfoBoxable
{
	private Animator animator;

	private bool debug;

	public MouseTileInteraction mouseTileInteraction;

	private ContainerOverlapMonitor containerOverlapMonitor;

	private TilesManager tilesManager;

	private bool _tileIsEnabled = true;

	private bool _interactable;

	private BoxCollider boxCollider;

	private int cooldownCharge;

	private TrailRenderer trailRenderer;

	private ParticleSystem trailParticleSystem;

	private TileContainer _tileContainer;

	private string[] highlightSounds = new string[2] { "TileHighlight_1", "TileHighlight_2" };

	public bool TileIsEnabled
	{
		get
		{
			return _tileIsEnabled;
		}
		set
		{
			if (!_tileIsEnabled && value && (Object)(object)TileContainer != (Object)null)
			{
				TileContainer.NotifyTileChanged.TilesChanged();
			}
			_tileIsEnabled = value;
			animator.SetBool("Disabled", !_tileIsEnabled);
			if (_tileIsEnabled && PointerOnTile)
			{
				OnPointerEnter();
			}
			if (!_tileIsEnabled)
			{
				Interactable = false;
			}
		}
	}

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			_interactable = value && TileIsEnabled;
			((Behaviour)mouseTileInteraction).enabled = _interactable;
			((Collider)boxCollider).enabled = _interactable;
			if (!_interactable)
			{
				if (PointerOnTile)
				{
					OnPointerExit();
				}
				Highlight(value: false);
			}
		}
	}

	public bool TrailEmitting
	{
		get
		{
			return trailRenderer.emitting;
		}
		set
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			trailRenderer.emitting = value;
			EmissionModule emission = trailParticleSystem.emission;
			((EmissionModule)(ref emission)).enabled = value;
		}
	}

	public TileGraphics Graphics { get; private set; }

	public Attack Attack { get; private set; }

	public TileContainer TileContainer
	{
		get
		{
			return _tileContainer;
		}
		set
		{
			if ((Object)(object)value != (Object)(object)_tileContainer && this.TileContainerChanged != null)
			{
				this.TileContainerChanged(this, null);
			}
			_tileContainer = value;
			animator.SetBool("InAttackQueue", (Object)(object)TileContainer != (Object)null && TileContainer is AttackQueueTileContainer);
		}
	}

	public TileContainer TileContainerOrigin { get; set; }

	public float Speed => mouseTileInteraction.Speed;

	public int PreferredHandContainer { get; set; } = -1;


	public string ExtraInfo { get; set; } = "";


	public TileContainer TargetTileContainer => containerOverlapMonitor.TargetContainer;

	public int EndOfTurnCooldownRecharge { get; set; }

	public bool PointerOnTile { get; private set; }

	public InfoBoxActivator InfoBoxActivator { get; set; }

	public bool FullyCharged => CooldownCharge == Attack.Cooldown;

	public int CooldownCharge
	{
		get
		{
			return cooldownCharge;
		}
		set
		{
			int num = cooldownCharge;
			cooldownCharge = Mathf.Min(value, Attack.Cooldown);
			Graphics.cooldownGraphics.Charge = cooldownCharge;
			if (FullyCharged && !TileIsEnabled)
			{
				TileIsEnabled = true;
			}
			if (FullyCharged && num < Attack.Cooldown && CombatManager.Instance.CombatInProgress)
			{
				EffectsManager.Instance.CreateInGameEffect("TileRechargeEffect", ((Component)this).transform);
			}
		}
	}

	private bool JustPlayed { get; set; }

	public int TurnsBeforeCharged => Attack.Cooldown - CooldownCharge;

	public int PostExecutionCooldownRecharge { get; set; }

	public Vector3 OriginOfPostExecutionCooldownRechargeEffect { get; set; }

	public string InfoBoxText
	{
		get
		{
			string text = "[header_color]" + TextUitls.SingleLineHeader(Attack.Name) + "[end_color]";
			text += "\n[vspace]";
			text += Attack.Description;
			if (Attack.AttackEffect != 0)
			{
				text += "\n[vspace]";
				text = text + "[bright_yellow]" + TileEnumsUtils.LocalizedAttackEffectName(Attack.AttackEffect) + "[end_color]: ";
				text += TileEnumsUtils.LocalizedAttackEffectDescription(Attack.AttackEffect);
			}
			if (Attack.TileEffect != 0)
			{
				text += "\n[vspace]";
				text = text + "[bright_yellow]" + TileEnumsUtils.LocalizedTileEffectName(Attack.TileEffect) + "[end_color]: ";
				text += TileEnumsUtils.LocalizedTileEffectDescription(Attack.TileEffect);
			}
			if (ExtraInfo != "")
			{
				text += "\n[vspace]";
				text += ExtraInfo;
			}
			return TextUitls.ReplaceTags(text);
		}
	}

	public bool InfoBoxEnabled
	{
		get
		{
			if (!mouseTileInteraction.Dragging)
			{
				if (!Globals.TilesInfoMode)
				{
					return Globals.FullInfoMode;
				}
				return true;
			}
			return false;
		}
	}

	public BoxWidth BoxWidth => BoxWidth.medium;

	public event EventHandler TileContainerChanged;

	private void Awake()
	{
		mouseTileInteraction = ((Component)this).GetComponent<MouseTileInteraction>();
		containerOverlapMonitor = ((Component)this).GetComponent<ContainerOverlapMonitor>();
		Graphics = ((Component)this).GetComponentInChildren<TileGraphics>();
		boxCollider = ((Component)this).GetComponent<BoxCollider>();
		animator = ((Component)this).GetComponent<Animator>();
		trailRenderer = ((Component)this).GetComponentInChildren<TrailRenderer>();
		trailParticleSystem = ((Component)this).GetComponentInChildren<ParticleSystem>();
		InfoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
	}

	public void Initialize(Attack combat)
	{
		Attack = combat;
		Graphics.Initialize(this);
		cooldownCharge = combat.Cooldown;
		CooldownCharge = combat.Cooldown;
		TileIsEnabled = true;
		tilesManager = TilesManager.Instance;
		((Object)this).name = Attack.Name;
	}

	public void Dropped()
	{
		tilesManager.TileDropped(this);
		Graphics.ShowDragShadow(value: false);
	}

	public void LeftClicked()
	{
		if ((Object)(object)TileContainer != (Object)null)
		{
			InfoBoxActivator.Close();
			TileContainer.UponTileSubmit();
		}
	}

	public void BeginDragging()
	{
		TileContainerOrigin = TileContainer;
		TileContainer.RemoveTile();
		tilesManager.TileBeingDragged = this;
		containerOverlapMonitor.Clear();
		containerOverlapMonitor.Add(TileContainerOrigin);
		BeginDragEffect();
		InfoBoxActivator.Close();
		if (debug)
		{
			Debug.Log((object)"BeginDragging");
		}
	}

	public void BeginDragEffect()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		SoundEffectsManager.Instance.Play("TileDrag");
		EffectsManager.Instance.CreateInGameEffect("TileInteractionEffect", Vector3.Scale(((Component)this).transform.position, new Vector3(1f, 1f, 0.5f))).GetComponent<TileInteractionEffect>().InitializeOutwards(((Component)this).transform);
		Graphics.ShowDragShadow(value: true);
	}

	public void Played(bool successfully)
	{
		if (successfully)
		{
			animator.SetTrigger("SuccessfullyPlayed");
		}
		else
		{
			animator.SetTrigger("UnsuccessfullyPlayed");
			SoundEffectsManager.Instance.Play("CannotPerformAction");
			EffectsManager.Instance.SmallScreenShake();
		}
		JustPlayed = true;
		CooldownCharge = 0;
	}

	public void OnBeginningOfTurn()
	{
		EndOfTurnCooldownRecharge = 1;
	}

	public void OnEndOfTurn()
	{
		if (CooldownCharge < Attack.Cooldown && !JustPlayed)
		{
			CooldownCharge += EndOfTurnCooldownRecharge;
		}
		TileIsEnabled = CooldownCharge == Attack.Cooldown;
		JustPlayed = false;
		PostExecutionCooldownRecharge = 0;
	}

	public IEnumerator PostExecutionCooldownRechargeSequence()
	{
		int cooldownToRecharge = Mathf.Clamp(PostExecutionCooldownRecharge, 0, Attack.Cooldown);
		if (cooldownToRecharge != 0)
		{
			for (int i = 0; i < cooldownToRecharge; i++)
			{
				EffectsManager.Instance.CreateInGameEffect("ComboRechargeEffect", OriginOfPostExecutionCooldownRechargeEffect + 1f * Vector3.up).GetComponent<ComboRechargeEffect>().Initialize(((Component)this).transform);
			}
			yield return (object)new WaitForSeconds(0.25f);
			CooldownCharge += cooldownToRecharge;
			if (!FullyCharged)
			{
				SoundEffectsManager.Instance.Play("PostExecutionCooldownRecharge");
			}
			float num = (FullyCharged ? 0.4f : 0.25f);
			yield return (object)new WaitForSeconds(num);
		}
	}

	public void GoToContainer(TileContainer target)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		EffectsManager.Instance.CreateInGameEffect("TileGoToEffect", ((Component)this).transform.position).GetComponent<TileGoToEffect>().Initialize(((Component)this).transform.position, ((Component)target).transform.position);
		animator.SetTrigger("Moving");
		if ((Object)(object)TileContainer != (Object)null)
		{
			TileContainer.RemoveTile();
		}
		target.AddTile(this);
		((Component)this).transform.position = new Vector3(((Component)this).transform.position.x, ((Component)this).transform.position.y, -0.1f);
	}

	public void Highlight(bool value)
	{
		if (value)
		{
			SoundEffectsManager.Instance.PlayRandom(highlightSounds);
		}
		animator.SetBool("Highlighted", value);
	}

	public void OnPointerEnter()
	{
		if (!PointerOnTile)
		{
			PointerOnTile = true;
			Highlight(value: true);
			if (TileIsEnabled)
			{
				tilesManager.OnTilePointerEnter(this);
			}
		}
	}

	public void OnPointerExit()
	{
		PointerOnTile = false;
		Highlight(value: false);
		tilesManager.OnTilePointerExit(this);
	}

	public void RechargeCooldown()
	{
		CooldownCharge = Attack.Cooldown;
	}

	public void SetUpListenersForDeckTile()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		EventsManager.Instance.BeginTileUpgradeMode.AddListener((UnityAction<TileUpgrade>)BeginTileUpgradeMode);
		EventsManager.Instance.EndTileUpgradeMode.AddListener(new UnityAction(EndTileUpgradeMode));
		EventsManager.Instance.ModeSwitched.AddListener((UnityAction<CombatSceneManager.Mode>)ModeSwitched);
	}

	private void BeginTileUpgradeMode(TileUpgrade tileUpgrade)
	{
		Graphics.ShowLevel(value: true);
		bool flag2 = (TileIsEnabled = tileUpgrade.CanUpgradeTile(this));
		Interactable = flag2;
		if (!flag2)
		{
			ExtraInfo = tileUpgrade.CannotUpgradeText(this);
		}
		else
		{
			ExtraInfo = "";
		}
	}

	private void EndTileUpgradeMode()
	{
		ExtraInfo = "";
		TileIsEnabled = true;
	}

	private void ModeSwitched(CombatSceneManager.Mode mode)
	{
		if (mode == CombatSceneManager.Mode.transition)
		{
			TrailEmitting = false;
		}
	}

	public TileSaveData GetTileSaveData()
	{
		return new TileSaveData
		{
			attackEnum = Attack.AttackEnum,
			cooldown = Attack.Cooldown,
			cooldownCharge = CooldownCharge,
			value = Attack.Value,
			baseValue = Attack.BaseValue,
			level = Attack.Level,
			maxLevel = Attack.MaxLevel,
			attackEffect = Attack.AttackEffect,
			tileEffect = Attack.TileEffect
		};
	}
}
