using System.Collections;
using Parameters;
using PickupEnums;
using UnityEngine;

public abstract class Boss : Enemy
{
	protected BossRoom bossRoom;

	private string[] deathSounds = new string[2] { "BossDeath_1", "BossDeath_2" };

	protected virtual int MetaCurrencyReward => GameParams.BossKillMetacurrencyReward(Progression.Instance.CurrentLocation.sector, Globals.Day);

	protected virtual int CoinReward { get; } = 5;


	public override bool CanBeElite { get; }

	public override void Start()
	{
		base.Start();
		base.Summoned = true;
		((Component)base.AgentCombatInfo.healthBar).gameObject.SetActive(false);
		bossRoom = (BossRoom)CombatSceneManager.Instance.Room;
		PostHealthUpdateEvents(0);
	}

	public virtual void FirstTimeBossFightInitializations(Room room)
	{
	}

	public override void Die()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		base.Die();
		EventsManager.Instance.BossDied.Invoke(this);
		((MonoBehaviour)this).StartCoroutine(BossDeathSequence());
		EffectsManager.Instance.CreateInGameEffect("MetacurrencyRewardEffect", ((Component)this).transform.position).GetComponent<MetacurrencyRewardEffect>().SetValue(MetaCurrencyReward);
		Globals.KillCount += MetaCurrencyReward;
		for (int i = 0; i < CoinReward; i++)
		{
			PickupFactory.Instance.InstantiatePickup(PickupEnum.coin, base.Cell);
		}
	}

	protected IEnumerator BossDeathSequence()
	{
		((MonoBehaviour)this).StartCoroutine(EffectsManager.Instance.SlowMotionEffect(0.2f, 0.1f));
		SoundEffectsManager.Instance.PlayRandom(deathSounds);
		EffectsManager.Instance.CreateInGameEffect("BloodTypeAEffect", ((Component)this).transform).GetComponent<SpriteRenderer>().flipX = (Object)(object)base.LastAttacker != (Object)null && ((Component)base.LastAttacker).transform.position.x > ((Component)this).transform.position.x;
		base.Animator.SetFloat("AnimationSpeed", 0f);
		yield return (object)new WaitForSeconds(0.7f);
		base.Animator.SetFloat("AnimationSpeed", 0.75f);
	}

	protected override void PostHealthUpdateEvents(int actualDeltaHealth)
	{
		bossRoom.bossHealthBar.UpdateHealth(base.AgentStats.maxHP, base.AgentStats.HP);
		if (base.AgentStats.HP <= 0 && base.IsAlive)
		{
			Die();
			bossRoom.BossDied();
		}
	}

	public virtual IEnumerator ProcessTurn()
	{
		yield return null;
	}
}
