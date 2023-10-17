using System;
using System.Collections;
using System.Collections.Generic;
using AgentEnums;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using UnlocksID;
using Utils;

public abstract class Hero : Agent, ISavable
{
	public HeroData heroData;

	[SerializeField]
	private Quest questForUnlocking;

	private InfoBoxActivatorWithFrame infoBoxActivatorWithFrame;

	public abstract HeroEnum HeroEnum { get; }

	public abstract List<AttackEnum[]> Decks { get; }

	public string SpecialAbilityName => LocalizationUtils.LocalizedString("Agents", TechnicalName + "_SpecialAbilityName");

	public abstract UnlockID UnlockID { get; }

	public float Luck { get; set; }

	public CharacterSaveData CharacterSaveData { get; private set; }

	public bool RemoveShieldWhenExitCombatMode { get; set; } = true;


	public SpecialMove SpecialMove { get; private set; }

	public Quest QuestForUnlocking => questForUnlocking;

	public int InitialTilesLevel => 3;

	public int InitialHandContainers { get; } = 2;


	protected override int InitialMaxHP
	{
		get
		{
			if (!Ascension.LowerHeroHP)
			{
				return 10;
			}
			return 5;
		}
	}

	public bool AllowExternallyImposingFacingDir { get; set; }

	public bool Unlocked
	{
		get
		{
			if (!UnlocksManager.Instance.Unlocked(UnlockID))
			{
				if ((Object)(object)QuestForUnlocking != (Object)null)
				{
					return UnlocksManager.Instance.Unlocked(QuestForUnlocking.unlockID);
				}
				return false;
			}
			return true;
		}
	}

	public override string InfoBoxText
	{
		get
		{
			string s = "[header_color]" + TextUitls.SingleLineHeader(base.Name) + "[end_color]\n[vspace]" + base.Description;
			if (!SpecialMove.IsEnabled)
			{
				s = "[header_color]" + TextUitls.SingleLineHeader(base.Name) + "[end_color]";
			}
			return TextUitls.ReplaceTags(s);
		}
	}

	public override void Awake()
	{
		base.Awake();
		infoBoxActivatorWithFrame = ((Component)this).GetComponentInChildren<InfoBoxActivatorWithFrame>();
		SpecialMove = ((Component)this).GetComponent<SpecialMove>();
		InitializeHP();
	}

	public override void Start()
	{
		base.Start();
	}

	public List<Tile> InstantiateInitialTiles()
	{
		List<Tile> list = new List<Tile>();
		AttackEnum[] array = Decks[0];
		foreach (AttackEnum attackEnum in array)
		{
			list.Add(TilesFactory.Instance.Create(attackEnum, InitialTilesLevel));
		}
		return list;
	}

	public override void AttackHasBeenPlayed(Attack attack)
	{
		EventsManager.Instance.HeroPlayedAttack.Invoke();
	}

	public bool MoveActionAllowed(Dir dir)
	{
		if (RegularMoveAllowed(dir))
		{
			return true;
		}
		if (SpecialMove.Allowed(this, dir))
		{
			if (SpecialMove.Cooldown.IsCharged)
			{
				return true;
			}
			SpecialMove.Cooldown.CannotPerformSpecialMoveEffect();
		}
		return false;
	}

	public virtual IEnumerator PerformMoveAction(Dir dir)
	{
		base.CellBeforeExecution = base.Cell;
		if (RegularMoveAllowed(dir))
		{
			Move(dir);
		}
		else if (SpecialMove.Allowed(this, dir))
		{
			yield return ((MonoBehaviour)this).StartCoroutine(SpecialMove.Perform(this, dir));
		}
	}

	protected bool RegularMoveAllowed(Dir dir)
	{
		Cell cell = base.Cell.Neighbour(dir, 1);
		if ((Object)(object)cell != (Object)null)
		{
			return (Object)(object)cell.Agent == (Object)null;
		}
		return false;
	}

	public override void TurnAround()
	{
		if (CombatManager.Instance.CombatInProgress)
		{
			EventsManager.Instance.HeroTurnedAround.Invoke();
		}
		base.TurnAround();
	}

	public override void ExecuteAttacksInQueue()
	{
		EventsManager.Instance.HeroAttacks.Invoke(base.AttackQueue);
		base.ExecuteAttacksInQueue();
	}

	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		if (actualDeltaHealth < 0)
		{
			EventsManager.Instance.HeroTookDamage.Invoke(-actualDeltaHealth);
		}
		if (base.AgentStats.HP <= 0)
		{
			Die();
		}
		((UnityEvent<int>)EventsManager.Instance.HeroHPUpdate).Invoke(base.AgentStats.HP);
	}

	public override void SetMaxHealth(int value)
	{
		base.SetMaxHealth(value);
		((UnityEvent<int>)EventsManager.Instance.HeroMaxHPUpdate).Invoke(base.AgentStats.maxHP);
	}

	public void SetLockedCharacterGraphics()
	{
		base.Animator.SetBool("LockedCharacter", true);
	}

	public override void ReceiveAttack(Hit hit, Agent other)
	{
		if (!base.AgentStats.shield)
		{
			EventsManager.Instance.HeroIsHit.Invoke((hit, other));
		}
		base.ReceiveAttack(hit, other);
	}

	public override void EnterCombatMode()
	{
		infoBoxActivatorWithFrame.ResetListeners();
		base.EnterCombatMode();
		SpecialMove.Cooldown.EnterCombatMode();
	}

	public override void ExitCombatMode()
	{
		base.ExitCombatMode();
		SpecialMove.Cooldown.ExitCombatMode();
		if (RemoveShieldWhenExitCombatMode)
		{
			RemoveShield();
		}
	}

	public void Wait()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!Globals.InCamp)
		{
			EffectsManager.Instance.CreateInGameEffect("WaitEffect", ((Component)base.AgentGraphics).transform.position);
			SoundEffectsManager.Instance.Play("Wait");
			EventsManager.Instance.HeroWaited.Invoke();
		}
	}

	public override void Die()
	{
		((HeroAttackQueue)base.AttackQueue).MoveTilesToHand();
		((Component)SpecialMove.Cooldown).gameObject.SetActive(false);
		base.Die();
		EventsManager.Instance.GameOver.Invoke(false);
	}

	public void LookAt(Transform lookAtThis)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (lookAtThis.position.x > ((Component)this).transform.position.x)
		{
			FlipRight();
		}
		else
		{
			FlipLeft();
		}
	}

	public HeroCombatSaveData GetHeroCombatSaveData()
	{
		HeroCombatSaveData heroCombatSaveData = new HeroCombatSaveData();
		PopulateAgentCombatSaveData(heroCombatSaveData);
		heroCombatSaveData.name = base.Name;
		heroCombatSaveData.heroEnum = HeroEnum;
		heroCombatSaveData.specialMoveCooldownCharge = SpecialMove.Cooldown.Charge;
		return heroCombatSaveData;
	}

	public void LoadFromSaveData(HeroCombatSaveData save)
	{
		LoadFromSaveData((AgentCombatSaveData)save);
		foreach (TileSaveData tileSaveData in loadedAgentCombatSaveData.attackQueue)
		{
			Tile tile2 = Array.Find(TilesManager.Instance.hand.TCC.Tiles, (Tile tile) => tileSaveData.IsEquivalentTo(tile.GetTileSaveData()));
			base.AttackQueue.NActiveContainers++;
			base.AttackQueue.AddTile(tile2.TileContainer.RemoveTile());
			tile2.TileContainer.TeleportTileInContainer();
		}
		SpecialMove.Cooldown.Charge = save.specialMoveCooldownCharge;
	}

	public void PopulateSaveData(SaveData saveData)
	{
		saveData.SetCharacterSaveData(CharacterSaveData);
	}

	public void LoadFromSaveData(SaveData saveData)
	{
		CharacterSaveData = (CharacterSaveData)SaveData.GetNamedSaveData(saveData.characterSaveData, TechnicalName);
		if (CharacterSaveData == null)
		{
			CharacterSaveData = new CharacterSaveData(TechnicalName);
		}
	}
}
