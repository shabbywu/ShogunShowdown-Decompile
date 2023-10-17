using System;
using System.Collections.Generic;
using System.Linq;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public abstract class Enemy : Agent
{
	protected Hero hero;

	protected AttackEnum tileToPlay;

	protected AttackEffectEnum attackEffectForTileToPlay;

	protected ActionEnum previousAction;

	private EliteEnemyIcon eliteEnemyIcon;

	private EliteParticleEffect eliteParticlEffect;

	protected bool firstTurn = true;

	public abstract EnemyEnum EnemyEnum { get; }

	public abstract bool IsPurelyRangedEnemy { get; }

	public abstract bool CanBeElite { get; }

	public EnemyActionPreview EnemyActionPreview { get; private set; }

	protected virtual EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[0];


	protected List<AttackEnum> TilesPlayed { get; private set; } = new List<AttackEnum>();


	protected AttackEnum? LastTilePlayed
	{
		get
		{
			if (TilesPlayed.Count != 0)
			{
				return TilesPlayed[TilesPlayed.Count - 1];
			}
			return null;
		}
	}

	public EliteTypeEnum EliteType { get; set; }

	protected virtual EliteTypeEnum[] IncompatibleEliteTypes { get; } = new EliteTypeEnum[0];


	protected AttackEffectEnum AttackEffect
	{
		get
		{
			if (EliteType != EliteTypeEnum.doubleStrike)
			{
				return AttackEffectEnum.none;
			}
			return AttackEffectEnum.replay;
		}
	}

	public override bool Movable => EliteType != EliteTypeEnum.heavy;

	protected abstract int DefaultInitialHP { get; }

	protected abstract int HigerInitialHP { get; }

	protected override int InitialMaxHP
	{
		get
		{
			int num = (Ascension.HigherEnemiesHP ? HigerInitialHP : DefaultInitialHP);
			switch (EliteType)
			{
			case EliteTypeEnum.none:
				return num;
			case EliteTypeEnum.reactiveShield:
				return DefaultInitialHP + 1;
			case EliteTypeEnum.doubleStrike:
				return Math.Max(num, DefaultInitialHP + 2);
			case EliteTypeEnum.heavy:
			case EliteTypeEnum.quickWitted:
				return Math.Max(num, DefaultInitialHP + 1);
			default:
				return num;
			}
		}
	}

	public bool KilledByFriend { get; set; }

	public virtual Enemy GetNextEnemyToSummon { get; }

	public bool Summoned { get; set; }

	public Vector3 PickupSpawningOrigin => ((Component)this).transform.position + 0.5f * Vector3.up;

	protected bool CanMoveAwayFromHero => CanPerformAction(MoveAwayFromHero());

	public override string InfoBoxText
	{
		get
		{
			string text = base.Name;
			if (EliteType != 0)
			{
				text = "Elite " + text;
			}
			string text2 = "[enemy_color]" + TextUitls.SingleLineHeader(text) + "[end_color]\n[vspace][lightgray_color]" + base.Description + "[end_color]";
			EnemyTraitsEnum[] enemyTraits = EnemyTraits;
			foreach (EnemyTraitsEnum enemyTrait in enemyTraits)
			{
				text2 = text2 + "\n[vspace]" + AgentEnumsUtils.EnemyTraitDescription(enemyTrait);
			}
			if (EliteType != 0)
			{
				text2 = text2 + "\n[vspace]" + AgentEnumsUtils.EliteDescription(EliteType);
			}
			return TextUitls.ReplaceTags(text2);
		}
	}

	protected abstract ActionEnum AIPickAction();

	public override void Awake()
	{
		EnemyAssertions();
		base.Awake();
		EnemyActionPreview = ((Component)base.AgentCombatInfo).GetComponentInChildren<EnemyActionPreview>();
		eliteEnemyIcon = ((Component)base.AgentCombatInfo).GetComponentInChildren<EliteEnemyIcon>();
		InitializeHP();
		hero = Globals.Hero;
	}

	public override void Start()
	{
		base.Start();
		PotentiallyPromoteToRandomElite();
		base.AttackQueue.NActiveContainers = 0;
		base.AttackQueue.Hide();
		if (loadedAgentCombatSaveData != null)
		{
			LoadSaveDataInStart();
		}
	}

	private void LoadSaveDataInStart()
	{
		foreach (TileSaveData item in loadedAgentCombatSaveData.attackQueue)
		{
			base.AttackQueue.NActiveContainers++;
			CreateAndAddTileToAttackQueue(item.attackEnum, item.attackEffect);
		}
		PreviewNextAction();
		if (EliteType != 0)
		{
			PromoteToElite(EliteType, initializeHP: false);
		}
	}

	public void PotentiallyPromoteToRandomElite()
	{
		if (CanBeElite && Ascension.EliteEnemies && loadedAgentCombatSaveData == null && AgentsFactory.Instance.EliteEnemyRandomizer.GetNext())
		{
			List<EliteTypeEnum> allEnums = EnumUtils.GetAllEnums<EliteTypeEnum>();
			allEnums.RemoveAll((EliteTypeEnum eliteType) => eliteType == EliteTypeEnum.none || IncompatibleEliteTypes.Contains(eliteType));
			PromoteToElite(MyRandom.NextRandomUniform(allEnums), initializeHP: true);
		}
	}

	private void PromoteToElite(EliteTypeEnum eliteType, bool initializeHP)
	{
		EliteType = eliteType;
		eliteEnemyIcon.Initialize(EliteType);
		eliteParticlEffect = EffectsManager.Instance.CreateInGameEffect("EliteParticleEffect", ((Component)this).transform).GetComponent<EliteParticleEffect>();
		eliteParticlEffect.Initialize(EliteType);
		((Component)base.AgentGraphics).GetComponent<EliteEnemyMaterialSwapper>().ReplaceAgentDefaultMaterial(eliteType);
		if (initializeHP)
		{
			InitializeHP();
		}
	}

	public override void RegisterActionInProgress(float t, bool imposed = false)
	{
		base.RegisterActionInProgress(t, imposed);
		if (!imposed)
		{
			EnemyActionPreview.HidePreview();
		}
	}

	public override void AttackHasBeenPlayed(Attack attack)
	{
	}

	public override void ReceiveAttack(Hit hit, Agent other)
	{
		bool hasShield = base.HasShield;
		base.ReceiveAttack(hit, other);
		if ((Object)(object)other != (Object)null && base.AgentStats.HP <= 0 && !IsOpponent(other) && !hit.IsCollision)
		{
			EventsManager.Instance.EnemyFriendlyKill.Invoke();
			KilledByFriend = true;
		}
		if ((Object)(object)other == (Object)(object)Globals.Hero)
		{
			EventsManager.Instance.HeroDealtDamage.Invoke(hit.Damage);
		}
		if (EliteType == EliteTypeEnum.reactiveShield && !hasShield && hit.Damage > 0 && base.IsAlive)
		{
			AddShield();
		}
	}

	public void PlayTileAction()
	{
		CreateAndAddTileToAttackQueue(tileToPlay, attackEffectForTileToPlay);
		RegisterActionInProgress(0.1f);
		TilesPlayed.Add(tileToPlay);
	}

	private void CreateAndAddTileToAttackQueue(AttackEnum attackEnum, AttackEffectEnum attackEffect)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = TilesFactory.Instance.Create(attackEnum, 0, attackEffect);
		tile.Interactable = false;
		tile.Attack.Cooldown = 0;
		tile.Attack.TileEffect = TileEffectEnum.none;
		((Component)tile).transform.position = ((Component)this).transform.position;
		((Component)tile).transform.localScale = 0.3f * Vector3.one;
		tile.Graphics.UpdateGraphics();
		base.AttackQueue.AddTile(tile);
	}

	public override void Freeze(int duration)
	{
		if (Freezable)
		{
			base.Freeze(duration);
			base.AttackQueue.Graphics.Idle();
			if (base.AttackInProgress)
			{
				InterruptAttackAndClearQueue();
				base.Cell = ClosestCellToAgentTransform();
				SetPositionToCellPosition();
			}
			if (base.Action == ActionEnum.playTile)
			{
				base.AttackQueue.NActiveContainers--;
			}
			base.Action = ActionEnum.wait;
			EnemyActionPreview.PreviewAction(ActionEnum.wait);
		}
	}

	private void InterruptAttackAndClearQueue()
	{
		if (!base.AttackInProgress)
		{
			Debug.LogError((object)"Trying to interrupt attack when no attack is in progress");
			return;
		}
		((MonoBehaviour)this).StopCoroutine(executeAttacksInQueueCoroutine);
		((EnemyAttackQueue)base.AttackQueue).Clear();
		base.AttackInProgress = false;
	}

	public void ShowCombatUI()
	{
		((Component)base.AttackQueue).gameObject.SetActive(true);
		((Component)base.AgentCombatInfo).gameObject.SetActive(true);
	}

	public void DecideNextAction()
	{
		if (base.AgentStats.ice > 0)
		{
			base.Action = ActionEnum.wait;
		}
		else
		{
			base.Action = AIPickAction();
			if (!CanPerformAction(base.Action))
			{
				base.Action = ActionEnum.wait;
			}
		}
		previousAction = base.Action;
		firstTurn = false;
		PreviewNextAction();
	}

	private void PreviewNextAction()
	{
		if (base.Action == ActionEnum.playTile)
		{
			((EnemyAttackQueue)base.AttackQueue).AboutToPlayTile();
		}
		if (base.Action == ActionEnum.attack)
		{
			base.AttackQueue.Graphics.AboutToAttack();
			base.Animator.SetTrigger("AboutToAttack");
			base.Animator.SetBool("Idle", false);
			EffectsManager.Instance.CreateInGameEffect("AboutToAttackEffect", ((Component)base.AttackQueue.Graphics).transform);
			SoundEffectsManager.Instance.Play("AboutToAttack");
		}
		EnemyActionPreview.PreviewAction(base.Action);
	}

	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		if (base.AgentStats.HP <= 0 && base.IsAlive)
		{
			Die();
		}
	}

	public override void Die()
	{
		base.Die();
		EventsManager.Instance.EnemyDied.Invoke(this);
		if (!Summoned)
		{
			PickupFactory.Instance.PotentiallySpawnPickup(base.Cell);
		}
		EnemyActionPreview.HidePreview();
		EnemyActionPreview.HideAttackOrder();
		((Component)eliteEnemyIcon).gameObject.SetActive(false);
		if ((Object)(object)eliteParticlEffect != (Object)null)
		{
			eliteParticlEffect.Stop();
		}
	}

	protected Dir HeroDir()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)hero).transform.position.x > ((Component)this).transform.position.x)
		{
			return Dir.right;
		}
		return Dir.left;
	}

	protected bool IsFacingHero()
	{
		return base.FacingDir == HeroDir();
	}

	protected int DistanceFromHero()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		int num = 1;
		Dir dir = HeroDir();
		while (true)
		{
			Cell cell = base.Cell.Neighbour(dir, num);
			if ((Object)(object)cell == (Object)null)
			{
				break;
			}
			if ((Object)(object)cell.Agent == (Object)(object)hero)
			{
				return num;
			}
			num++;
		}
		Debug.LogError((object)"Problem determining DistanceFromHero!");
		Debug.LogError((object)$"Enemy cell position  : {((Component)base.Cell).transform.position}");
		Debug.LogError((object)$"Hero cell position   : {((Component)hero.Cell).transform.position}");
		Debug.LogError((object)$"HeroDir              : {dir}");
		Debug.LogError((object)$"Current dist         : {num}");
		return 1;
	}

	public bool IsPathToHeroFree()
	{
		Dir dir = HeroDir();
		int num = DistanceFromHero();
		for (int i = 1; i < num; i++)
		{
			if ((Object)(object)base.Cell.Neighbour(dir, i).Agent != (Object)null)
			{
				return false;
			}
		}
		return true;
	}

	protected bool IsThreatheningHero()
	{
		Attack[] attacks = base.AttackQueue.Attacks;
		foreach (Attack attack in attacks)
		{
			if (attack.HasValue && HasOpponentInRange(attack))
			{
				return true;
			}
		}
		return false;
	}

	protected ActionEnum FaceHero()
	{
		if (IsFacingHero())
		{
			Debug.LogWarning((object)"Using 'FaceHero' function, but the enemy is already facing the hero...");
			return ActionEnum.wait;
		}
		if (base.FacingDir == Dir.left)
		{
			return ActionEnum.flipRight;
		}
		return ActionEnum.flipLeft;
	}

	protected ActionEnum MoveAwayFromHero()
	{
		if (HeroDir() == Dir.right)
		{
			return ActionEnum.moveLeft;
		}
		return ActionEnum.moveRight;
	}

	protected ActionEnum MoveTowardsHero()
	{
		if (HeroDir() == Dir.right)
		{
			return ActionEnum.moveRight;
		}
		return ActionEnum.moveLeft;
	}

	protected bool CanPerformAction(ActionEnum action)
	{
		return action switch
		{
			ActionEnum.moveLeft => HasFreeCell(Dir.left), 
			ActionEnum.moveRight => HasFreeCell(Dir.right), 
			_ => true, 
		};
	}

	protected ActionEnum PlayTile(AttackEnum attackEnum, AttackEffectEnum attackEffect = AttackEffectEnum.none)
	{
		tileToPlay = attackEnum;
		attackEffectForTileToPlay = attackEffect;
		return ActionEnum.playTile;
	}

	protected ActionEnum TurnAroundActionEnum()
	{
		if (base.FacingDir == Dir.left)
		{
			return ActionEnum.flipRight;
		}
		return ActionEnum.flipLeft;
	}

	protected ActionEnum MoveTowardsStrikingPosition()
	{
		if (!base.AttackQueue.HasOffensiveAttack)
		{
			Debug.LogWarning((object)"Using 'MoveTowardsStrikingPosition' function, but the enemy does not have offensive tiles...");
			return ActionEnum.wait;
		}
		if (IsThreatheningHero())
		{
			Debug.LogWarning((object)"Using 'MoveTowardsStrikingPosition' function, but the enemy is already threathening the hero...");
			return ActionEnum.wait;
		}
		int num = 1000;
		int num2 = 0;
		Attack[] attacks = base.AttackQueue.Attacks;
		foreach (Attack attack in attacks)
		{
			if (attack.HasValue)
			{
				num = Mathf.Min(num, attack.Range.Min());
				num2 = Mathf.Max(num2, attack.Range.Max());
			}
		}
		int num3 = DistanceFromHero();
		if (num3 < num)
		{
			return MoveAwayFromHero();
		}
		if (num3 > num2)
		{
			return MoveTowardsHero();
		}
		return ActionEnum.wait;
	}

	private void EnemyAssertions()
	{
	}

	public EnemyCombatSaveData GetEnemyCombatSaveData()
	{
		EnemyCombatSaveData enemyCombatSaveData = new EnemyCombatSaveData();
		PopulateAgentCombatSaveData(enemyCombatSaveData);
		enemyCombatSaveData.enemy = EnemyEnum;
		enemyCombatSaveData.action = base.Action;
		enemyCombatSaveData.previousAction = previousAction;
		enemyCombatSaveData.tileToPlay = tileToPlay;
		enemyCombatSaveData.attackEffectForTileToPlay = attackEffectForTileToPlay;
		enemyCombatSaveData.firstTurn = firstTurn;
		enemyCombatSaveData.eliteType = EliteType;
		return enemyCombatSaveData;
	}

	public void LoadFromSaveData(EnemyCombatSaveData save)
	{
		LoadFromSaveData((AgentCombatSaveData)save);
		base.Action = save.action;
		previousAction = save.previousAction;
		tileToPlay = save.tileToPlay;
		attackEffectForTileToPlay = save.attackEffectForTileToPlay;
		firstTurn = save.firstTurn;
		EliteType = save.eliteType;
	}
}
