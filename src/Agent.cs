using System.Collections;
using System.Collections.Generic;
using CombatEnums;
using InfoBoxUtils;
using TileEnums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using Utils;

public abstract class Agent : MonoBehaviour, IInfoBoxable
{
	protected static float walkTime = 0.2f;

	public static float turnAroundTime = 4f / 15f;

	protected static float waitTimeBetweenAttacks = 0.2f;

	protected static float unsuccessfullyPlayedTileWaitTime = 0.5f;

	protected GameObject iceEffect;

	protected GameObject shieldEffect;

	protected GameObject poisonEffect;

	protected GameObject markEffect;

	private BoxCollider2D boxCollider;

	private Light2D light2d;

	public Tile tileInProgress;

	private InfoBoxActivator infoBoxActivator;

	protected AgentCombatSaveData loadedAgentCombatSaveData;

	protected Coroutine executeAttacksInQueueCoroutine;

	private Cell _cell;

	private Dir _facingDir;

	public AgentCombatInfo AgentCombatInfo { get; set; }

	public AgentGraphics AgentGraphics { get; set; }

	public AgentStats AgentStats { get; private set; } = new AgentStats();


	public abstract string TechnicalName { get; }

	public Animator Animator { get; private set; }

	protected abstract int InitialMaxHP { get; }

	public virtual bool Movable { get; } = true;


	public virtual bool Freezable { get; } = true;


	public bool MoveActionPerformed { get; set; }

	public int MoveDistance { get; set; } = 1;


	public bool IsAlive { get; protected set; } = true;


	public bool IsAtFullHealth => AgentStats.HP == AgentStats.maxHP;

	public Agent LastAttacker { get; protected set; }

	public bool CommittedSeppuku { get; private set; }

	public Dir MoveDir
	{
		get
		{
			if (Action == ActionEnum.moveRight)
			{
				return Dir.right;
			}
			if (Action == ActionEnum.moveLeft)
			{
				return Dir.left;
			}
			Debug.LogError((object)"Asking for MoveDir, but the action is not a move...");
			return Dir.right;
		}
	}

	public ActionEnum Action { get; set; }

	public bool ActionIsMove
	{
		get
		{
			if (Action != ActionEnum.moveRight)
			{
				return Action == ActionEnum.moveLeft;
			}
			return true;
		}
	}

	public bool ActionInProgress { get; protected set; }

	public bool AttackInProgress { get; set; }

	public AttackQueue AttackQueue { get; private set; }

	public Cell Cell
	{
		get
		{
			return _cell;
		}
		set
		{
			if ((Object)(object)_cell != (Object)null && (Object)(object)_cell.Agent == (Object)(object)this)
			{
				_cell.Agent = null;
			}
			if ((Object)(object)value != (Object)null && (Object)(object)_cell != (Object)(object)value)
			{
				OnCellChanged(_cell, value);
			}
			_cell = value;
			_cell.Agent = this;
		}
	}

	public Cell CellBeforeExecution { get; set; }

	public Dir FacingDir
	{
		get
		{
			return _facingDir;
		}
		set
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			_facingDir = value;
			if ((Object)(object)AgentGraphics == (Object)null)
			{
				AgentGraphics = ((Component)this).GetComponentInChildren<AgentGraphics>();
			}
			if (_facingDir == Dir.left)
			{
				((Component)AgentGraphics).transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (_facingDir == Dir.right)
			{
				((Component)AgentGraphics).transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				Debug.LogError((object)$"Facing dir should be 'left' or 'right', and not '{_facingDir}'");
			}
		}
	}

	public Cell CellInFront => Cell.Neighbour(FacingDir, 1);

	public Cell CellBehind => Cell.Neighbour(DirUtils.Opposite(FacingDir), 1);

	public bool HasShield => AgentStats.shield;

	public string Name => LocalizationUtils.LocalizedString("Agents", TechnicalName + "_Name");

	public string Description => LocalizationUtils.LocalizedString("Agents", TechnicalName + "_Description");

	public abstract string InfoBoxText { get; }

	public bool InfoBoxEnabled => Globals.FullInfoMode;

	public BoxWidth BoxWidth => BoxWidth.medium;

	public virtual void Awake()
	{
		Animator = ((Component)this).GetComponent<Animator>();
		AttackQueue = ((Component)this).GetComponentInChildren<AttackQueue>();
		AgentCombatInfo = ((Component)this).GetComponentInChildren<AgentCombatInfo>();
		AgentGraphics = ((Component)this).GetComponentInChildren<AgentGraphics>();
		light2d = ((Component)this).GetComponentInChildren<Light2D>();
		boxCollider = ((Component)this).GetComponent<BoxCollider2D>();
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
		FacingDir = MyRandom.NextFromArray(new Dir[2]
		{
			Dir.left,
			Dir.right
		});
	}

	public virtual void Start()
	{
		Animator.SetBool("Idle", true);
	}

	public void SetPositionToCellPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = ((Component)Cell).transform.position;
	}

	protected virtual void OnCellChanged(Cell oldCell, Cell newCell)
	{
	}

	public virtual void Die()
	{
		IsAlive = false;
		RemoveAllStatusEffects();
		RemoveShield();
		AttackQueue.DestroyAllTiles();
		AttackQueue.Hide();
		Cell.Agent = null;
		((Behaviour)boxCollider).enabled = false;
		((Behaviour)light2d).enabled = false;
		TriggerDeathAnimation();
		Object.Destroy((Object)(object)((Component)infoBoxActivator).gameObject);
	}

	private void RemoveAllStatusEffects()
	{
		if ((Object)(object)iceEffect != (Object)null)
		{
			Defrost();
		}
		if ((Object)(object)markEffect != (Object)null)
		{
			RemoveMark();
		}
		if ((Object)(object)poisonEffect != (Object)null)
		{
			poisonEffect.SendMessage("EndPoison");
		}
		AgentStats.ResetStatusEffects();
	}

	public void DieAnimationOver()
	{
		((Behaviour)Animator).enabled = false;
		((Component)AgentCombatInfo).gameObject.SetActive(false);
		((Behaviour)this).enabled = false;
	}

	public void CommitSeppuku()
	{
		CommittedSeppuku = true;
		LastAttacker = this;
		AddToHealth(-AgentStats.HP);
	}

	public bool AgentPositionMatchesCellPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.1f;
		return Mathf.Abs(((Component)Cell).transform.position.x - ((Component)this).transform.position.x) < num;
	}

	public abstract void AttackHasBeenPlayed(Attack attack);

	public bool HasInAttackStack(AttackEnum attackEnum)
	{
		Tile[] tiles = AttackQueue.TCC.Tiles;
		for (int i = 0; i < tiles.Length; i++)
		{
			if (tiles[i].Attack.AttackEnum == attackEnum)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasInAttackStack(List<AttackEnum> attackEnums)
	{
		Tile[] tiles = AttackQueue.TCC.Tiles;
		foreach (Tile tile in tiles)
		{
			if (attackEnums.Contains(tile.Attack.AttackEnum))
			{
				return true;
			}
		}
		return false;
	}

	public void FullHeal()
	{
		if (!IsAtFullHealth)
		{
			AddToHealth(AgentStats.maxHP - AgentStats.HP);
		}
	}

	public void AddToHealth(int delta)
	{
		int hP = AgentStats.HP;
		AgentStats.HP += delta;
		int actualDeltaHealth = AgentStats.HP - hP;
		AgentCombatInfo.healthBar.HealthUpdate(AgentStats.HP);
		PostHealthUpdateEvents(actualDeltaHealth);
	}

	public virtual void ReceiveAttack(Hit hit, Agent attacker)
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		LastAttacker = attacker;
		if (Cell.Effect == Cell.CellEffect.curse)
		{
			hit.Damage++;
		}
		if ((Object)(object)attacker != (Object)null && attacker.Cell.Effect == Cell.CellEffect.curse)
		{
			hit.Damage--;
		}
		if (Cell.Effect == Cell.CellEffect.blessing)
		{
			hit.Damage--;
		}
		if ((Object)(object)attacker != (Object)null && attacker.Cell.Effect == Cell.CellEffect.blessing)
		{
			hit.Damage++;
		}
		if (AgentStats.mark && hit.Damage > 0 && !AgentStats.shield)
		{
			hit.Damage *= 2;
			SoundEffectsManager.Instance.Play("SpecialHit");
			RemoveMark();
		}
		hit.Damage = Mathf.Max(0, hit.Damage);
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).Invoke(attacker, this, hit);
		EffectsManager.Instance.CreateInGameEffect("HitEffect", ((Component)AgentGraphics).transform);
		if (AgentStats.shield)
		{
			RemoveShield();
			SoundEffectsManager.Instance.Play("ShieldDisappear");
			return;
		}
		if (AgentStats.HP == hit.Damage)
		{
			EventsManager.Instance.PreciseKill.Invoke(this);
		}
		AddToHealth(-hit.Damage);
		AgentGraphics.HitReceivedEffect();
		EffectsManager.Instance.CreateInGameEffect("DamageNumberEffect", ((Component)AgentGraphics).transform.position + Vector3.up * 0.5f).GetComponent<DamageNumberEffect>().Text = hit.Damage.ToString();
	}

	public void InitializeHP()
	{
		AgentStats.maxHP = InitialMaxHP;
		AgentStats.HP = InitialMaxHP;
		AgentCombatInfo.healthBar.Initialize(AgentStats.maxHP, AgentStats.HP);
	}

	public void AddToMaxHealth(int delta)
	{
		SetMaxHealth(AgentStats.maxHP + delta);
		AddToHealth(delta);
	}

	public virtual void SetMaxHealth(int value)
	{
		AgentStats.maxHP = value;
		AgentStats.HP = Mathf.Clamp(AgentStats.HP, 0, AgentStats.maxHP);
		AgentCombatInfo.healthBar.Initialize(value, AgentStats.HP);
	}

	public virtual void EnterCombatMode()
	{
		SetCombatUIActive(value: true);
	}

	public virtual void ExitCombatMode()
	{
		AttackQueue.Hide();
		RemoveAllStatusEffects();
	}

	public void SetCombatUIActive(bool value)
	{
		((Component)AgentCombatInfo).gameObject.SetActive(value);
		if (value)
		{
			AgentCombatInfo.healthBar.Initialize(AgentStats.maxHP, AgentStats.HP);
		}
		if ((Object)(object)infoBoxActivator != (Object)null)
		{
			((Component)infoBoxActivator).gameObject.SetActive(value);
		}
	}

	public void ApplyPoisonEffect(int duration)
	{
		if (AgentStats.poison == 0)
		{
			poisonEffect = EffectsManager.Instance.CreateInGameEffect("PoisonEffect", ((Component)AgentGraphics).transform);
		}
		AgentStats.poison = duration;
	}

	public virtual void Defrost()
	{
		((Behaviour)Animator).enabled = true;
		iceEffect.SendMessage("Defrost");
		iceEffect = null;
		if (IsAlive)
		{
			Animator.SetTrigger("TriggerIdle");
		}
	}

	public void GetMarked()
	{
		if (!AgentStats.mark)
		{
			AgentStats.mark = true;
			markEffect = EffectsManager.Instance.CreateInGameEffect("MarkEffect", ((Component)AgentGraphics).transform);
			SoundEffectsManager.Instance.Play("MarkEffect");
		}
	}

	public void RemoveMark()
	{
		AgentStats.mark = false;
		Object.Destroy((Object)(object)markEffect);
	}

	public virtual void Freeze(int duration)
	{
		if (Freezable)
		{
			AgentStats.ice = duration;
			ActionInProgress = false;
			if ((Object)(object)iceEffect == (Object)null && duration > 1)
			{
				iceEffect = EffectsManager.Instance.CreateInGameEffect("IceEffect", ((Component)AgentGraphics).transform);
			}
			if (duration > 1)
			{
				((Behaviour)Animator).enabled = false;
			}
		}
	}

	public void AddShield()
	{
		if ((Object)(object)shieldEffect != (Object)null)
		{
			RemoveShield();
		}
		shieldEffect = EffectsManager.Instance.CreateInGameEffect("ShieldEffect", AgentGraphics.AnimationFollowingTransform);
		AgentStats.shield = true;
	}

	protected void RemoveShield()
	{
		if (AgentStats.shield)
		{
			shieldEffect.GetComponent<Animator>().SetTrigger("Disappear");
			shieldEffect = null;
			AgentStats.shield = false;
		}
	}

	protected bool HasFreeCell(Dir dir)
	{
		if (Cell.Neighbours.ContainsKey(dir))
		{
			return (Object)(object)Cell.Neighbours[dir].Agent == (Object)null;
		}
		return false;
	}

	public void OnBeginningOfTurn()
	{
		MoveActionPerformed = false;
		if (Action == ActionEnum.attack)
		{
			Attack[] attacks = AttackQueue.Attacks;
			for (int i = 0; i < attacks.Length; i++)
			{
				attacks[i].AttackDeclared(this);
			}
		}
	}

	public void Flip()
	{
		FacingDir = DirUtils.Opposite(FacingDir);
	}

	public void FlipLeft()
	{
		if (FacingDir == Dir.right)
		{
			RegisterActionInProgress(turnAroundTime);
			TurnAround();
		}
	}

	public void FlipRight()
	{
		if (FacingDir == Dir.left)
		{
			RegisterActionInProgress(turnAroundTime);
			TurnAround();
		}
	}

	public virtual void WalkTo(Cell target)
	{
		_StepToOtherCell(target);
		Cell = target;
	}

	private void _StepToOtherCell(Cell target)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		RegisterActionInProgress(walkTime);
		GameObject val = EffectsManager.Instance.CreateInGameEffect("WalkSmokeEffect", ((Component)this).transform.position);
		if (((Component)target).transform.position.x < ((Component)Cell).transform.position.x)
		{
			val.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		Animator.SetTrigger("Walk");
		SoundEffectsManager.Instance.PlayAfterDeltaT("Move", 0.8f * walkTime);
		EffectsManager.Instance.WaitAndCreateInGameEffect("LandWindWirlEffect", ((Component)target).transform.position, walkTime);
		((MonoBehaviour)this).StartCoroutine(MoveToCoroutine(((Component)Cell).transform.position, ((Component)target).transform.position, walkTime));
	}

	public IEnumerator DashToOtherCell(Cell target, float speed = 9.1f, bool createDustEffect = true, bool createDashEffect = true)
	{
		float time = (float)Cell.Distance(Cell, target) / speed;
		yield return ((MonoBehaviour)this).StartCoroutine(MoveToCoroutine(((Component)Cell).transform.position, ((Component)target).transform.position, time, 0f, createDustEffect, createDashEffect));
	}

	public void Move(Dir dir)
	{
		if (MoveActionPerformed)
		{
			return;
		}
		Cell cell = null;
		for (int i = 1; i <= MoveDistance; i++)
		{
			Cell cell2 = Cell.Neighbour(dir, i);
			if ((Object)(object)cell2 == (Object)null || !cell2.IsFree)
			{
				break;
			}
			cell = cell2;
		}
		if ((Object)(object)cell == (Object)null)
		{
			return;
		}
		if ((Object)(object)cell.Agent == (Object)null)
		{
			WalkTo(cell);
			MoveActionPerformed = true;
		}
		else
		{
			Agent agent = cell.Agent;
			if (!agent.MoveActionPerformed && agent.ActionIsMove && agent.MoveDir == dir && agent.CouldMoveInDir(dir))
			{
				WalkTo(cell);
			}
		}
		MoveActionPerformed = true;
	}

	public bool CouldMoveInDir(Dir dir)
	{
		Cell cell = Cell.Neighbour(dir, 1);
		if ((Object)(object)cell == (Object)null)
		{
			return false;
		}
		if ((Object)(object)cell.Agent == (Object)null)
		{
			return true;
		}
		return false;
	}

	public void ImposedMovement(Cell targetCell, float speed = 7f, float waitBeforeMoving = 0f, bool createDustEffect = false)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)targetCell.Agent != (Object)null))
		{
			float num = Mathf.Abs(((Component)Cell).transform.position.x - ((Component)targetCell).transform.position.x) / speed;
			RegisterActionInProgress(num + waitBeforeMoving, imposed: true);
			SoundEffectsManager.Instance.Play("Swap");
			((MonoBehaviour)this).StartCoroutine(MoveToCoroutine(((Component)Cell).transform.position, ((Component)targetCell).transform.position, num, waitBeforeMoving, createDustEffect));
			if (IsAlive)
			{
				Cell = targetCell;
			}
		}
	}

	public virtual void ExecuteAttacksInQueue()
	{
		ActionInProgress = true;
		executeAttacksInQueueCoroutine = ((MonoBehaviour)this).StartCoroutine(ExecuteAttacksInQueueCoroutine());
	}

	public void ApplyAttackEffect()
	{
		if (!((Object)(object)tileInProgress == (Object)null))
		{
			tileInProgress.Attack.ApplyEffect();
		}
	}

	public void AttackOver()
	{
		AttackInProgress = false;
	}

	public bool IsOpponent(Agent other)
	{
		return ((Component)this).gameObject.tag != ((Component)other).gameObject.tag;
	}

	public virtual void TurnAround()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		EffectsManager.Instance.CreateInGameEffect("WalkSmokeEffect", ((Component)this).transform.position);
		Animator.SetTrigger("Flip");
		SoundEffectsManager.Instance.PlayAfterDeltaT("Move", 0.8f * turnAroundTime);
		EffectsManager.Instance.WaitAndCreateInGameEffect("LandWindWirlEffect", ((Component)this).transform.position, turnAroundTime);
	}

	protected virtual void PostHealthUpdateEvents(int actualDeltaHealth)
	{
	}

	protected Cell ClosestCellToAgentTransform()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Cell cell = Cell;
		foreach (Cell item in Cell.AllCellsInGrid())
		{
			if (Vector3.Distance(((Component)item).transform.position, ((Component)this).transform.position) < Vector3.Distance(((Component)cell).transform.position, ((Component)this).transform.position))
			{
				cell = item;
			}
		}
		return cell;
	}

	public IEnumerator MoveToCoroutine(Vector3 initialPosition, Vector3 finalPosition, float time, float waitBeforeMoving = 0f, bool createDustEffect = false, bool createDashEffect = false)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Dir dir = ((finalPosition.x > initialPosition.x) ? Dir.right : Dir.left);
		float dustDeltaX = 0f;
		float currentTime = 0f;
		yield return (object)new WaitForSeconds(waitBeforeMoving);
		if (createDashEffect)
		{
			GameObject val = EffectsManager.Instance.CreateInGameEffect("DashWindWirlEffect", ((Component)this).transform.position);
			if (dir == Dir.left)
			{
				val.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		while (currentTime < time)
		{
			float num = Mathf.Lerp(initialPosition.x, finalPosition.x, currentTime / time);
			dustDeltaX += Mathf.Abs(num - ((Component)this).transform.position.x);
			((Component)this).transform.position = new Vector3(num, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
			if (createDustEffect && dustDeltaX > 0.5f)
			{
				dustDeltaX = 0f;
				EffectsManager.Instance.CreateInGameEffect("SmallDashEffect", ((Component)this).transform.position, dir == Dir.left);
			}
			currentTime += Time.deltaTime;
			yield return null;
		}
		((Component)this).transform.position = finalPosition;
	}

	public virtual void RegisterActionInProgress(float t, bool imposed = false)
	{
		ActionInProgress = true;
		((MonoBehaviour)this).StartCoroutine(WaitAndEndAction(t));
	}

	public void RegisterAttackInProgress(float t)
	{
		AttackInProgress = true;
		((MonoBehaviour)this).StartCoroutine(WaitAndEndAttack(t));
	}

	private IEnumerator WaitAndEndAction(float t)
	{
		yield return (object)new WaitForSeconds(t);
		ActionInProgress = false;
	}

	private IEnumerator WaitAndEndAttack(float t)
	{
		yield return (object)new WaitForSeconds(t);
		AttackInProgress = false;
	}

	private IEnumerator ExecuteAttacksInQueueCoroutine()
	{
		AttackQueue.BeginAttack();
		int nTiles = AttackQueue.NTiles;
		for (int i = 0; i < nTiles; i++)
		{
			if (i != 0)
			{
				yield return (object)new WaitForSeconds(waitTimeBetweenAttacks);
			}
			Tile tile = AttackQueue.containers[0].Tile;
			AttackHasBeenPlayed(tile.Attack);
			yield return ((MonoBehaviour)this).StartCoroutine(ExecuteTile(tile));
			if (tile.Attack.AttackEffect == AttackEffectEnum.replay)
			{
				yield return ((MonoBehaviour)this).StartCoroutine(ExecuteTile(tile));
			}
			AttackQueue.TileInContainer_0_Played();
		}
		AttackQueue.TilesWerePlayed();
		ActionInProgress = false;
		tileInProgress = null;
		Animator.SetBool("Idle", true);
	}

	private IEnumerator ExecuteTile(Tile tile)
	{
		tileInProgress = tile;
		CellBeforeExecution = Cell;
		AttackInProgress = false;
		bool flag = tileInProgress.Attack.Begin(this);
		tileInProgress.Played(flag);
		if (!flag)
		{
			yield return (object)new WaitForSeconds(unsuccessfullyPlayedTileWaitTime);
		}
		else if (tileInProgress.Attack.HasAnimation)
		{
			AttackInProgress = true;
			Animator.SetTrigger(tileInProgress.Attack.AnimationTrigger);
		}
		while (AttackInProgress)
		{
			yield return null;
		}
		while (tileInProgress.Attack.WaitingForSomethingToFinish())
		{
			yield return null;
		}
		if (tileInProgress.PostExecutionCooldownRecharge > 0)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(tileInProgress.PostExecutionCooldownRechargeSequence());
		}
	}

	protected bool HasOpponentInRange(Attack attack)
	{
		Agent[] array = attack.AgentsInRange(this);
		foreach (Agent other in array)
		{
			if (IsOpponent(other))
			{
				return true;
			}
		}
		return false;
	}

	protected void TriggerDeathAnimation()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		Animator.SetBool("Idle", false);
		if ((Object)(object)LastAttacker != (Object)null && ((((Component)LastAttacker).transform.position.x > ((Component)this).transform.position.x && FacingDir == Dir.left) || (((Component)LastAttacker).transform.position.x < ((Component)this).transform.position.x && FacingDir == Dir.right)))
		{
			Animator.SetTrigger("DieForward");
		}
		else
		{
			Animator.SetTrigger("DieBackwards");
		}
	}

	public void SetIdleAnimation(bool value)
	{
		Animator.SetBool("Idle", value);
	}

	public void PushBackAgentSpriteSortingLayer()
	{
		AgentGraphics.PushBackAgentSpriteSortingLayer();
	}

	public virtual void EndOfTurn()
	{
		tileInProgress = null;
		ProcessEndOfTurnStatusEffects();
	}

	public IEnumerator Pushed(Dir dir, float pushSpeed = 15f)
	{
		float bounceHitAnimationTime = 0.025f;
		float bounceTime = 0.2f;
		Cell moveToCell = Cell.LastFreeCellInDirection(dir);
		Cell cell = moveToCell.Neighbour(dir, 1);
		bool hitAgentBehind = (Object)(object)cell != (Object)null && (Object)(object)cell.Agent != (Object)null;
		Agent collisionTarget = (hitAgentBehind ? cell.Agent : null);
		if (IsAlive)
		{
			Cell = moveToCell;
		}
		Vector3 position = ((Component)this).transform.position;
		Vector3 to = (hitAgentBehind ? ((((Component)moveToCell).transform.position + ((Component)cell).transform.position) / 2f) : ((Component)moveToCell).transform.position);
		SoundEffectsManager.Instance.Play("Dash");
		float time = Vector3.Distance(position, to) / pushSpeed;
		yield return ((MonoBehaviour)this).StartCoroutine(MoveToCoroutine(position, to, time, 0f, createDustEffect: true, createDashEffect: true));
		if (hitAgentBehind)
		{
			SoundEffectsManager.Instance.Play("CombatHit");
			EffectsManager.Instance.ScreenShake();
			collisionTarget.ReceiveAttack(new Hit(1, isDirectional: false, isCollision: true), this);
			ReceiveAttack(new Hit(1, isDirectional: false, isCollision: true), collisionTarget);
			yield return (object)new WaitForSeconds(bounceHitAnimationTime);
			yield return ((MonoBehaviour)this).StartCoroutine(MoveToCoroutine(to, ((Component)moveToCell).transform.position, bounceTime));
		}
		yield return null;
	}

	private void ProcessEndOfTurnStatusEffects()
	{
		if (AgentStats.poison > 0)
		{
			poisonEffect.SendMessage("PoisonDamageEffect");
			ReceiveAttack(new Hit(1, isDirectional: false), null);
			SoundEffectsManager.Instance.Play("CombatHit");
			RegisterActionInProgress(0.1f);
			AgentStats.poison--;
			if (AgentStats.poison <= 0)
			{
				poisonEffect.SendMessage("EndPoison");
				poisonEffect = null;
			}
		}
		if (AgentStats.ice > 0)
		{
			AgentStats.ice--;
			if (AgentStats.ice == 1)
			{
				Defrost();
			}
		}
	}

	protected void PopulateAgentCombatSaveData(AgentCombatSaveData save)
	{
		save.FacingDir = FacingDir;
		save.iCell = Cell.IndexInGrid;
		save.agentStats = AgentStats;
		save.attackQueue = new List<TileSaveData>();
		Tile[] tiles = AttackQueue.TCC.Tiles;
		foreach (Tile tile in tiles)
		{
			save.attackQueue.Add(tile.GetTileSaveData());
		}
	}

	protected void LoadFromSaveData(AgentCombatSaveData save)
	{
		loadedAgentCombatSaveData = save;
		FacingDir = save.FacingDir;
		if (save.agentStats.shield)
		{
			AddShield();
		}
		if (save.agentStats.mark)
		{
			GetMarked();
		}
		if (save.agentStats.poison > 0)
		{
			ApplyPoisonEffect(save.agentStats.poison);
		}
		if (save.agentStats.ice > 0)
		{
			Freeze(save.agentStats.ice);
		}
		AgentCombatInfo.healthBar.Initialize(save.agentStats.maxHP, save.agentStats.HP);
		AgentStats = save.agentStats;
	}

	public void SetVisible(bool value)
	{
		((Component)AgentGraphics).gameObject.SetActive(value);
	}

	public void ShowDialogue(string text)
	{
		EffectsManager.Instance.CreateInGameEffect("DialogueBox", infoBoxActivator.infoBoxTarget).GetComponent<DialogueBox>().Open(text);
	}
}
