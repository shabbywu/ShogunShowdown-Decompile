using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class TanegashimaAttack : Attack
{
	private int runningCoroutinesCounter;

	private readonly float recoilSpeed = 17f;

	private readonly float bounceTime = 0.2f;

	private readonly float bounceHitAnimationTime = 0.025f;

	public override AttackEnum AttackEnum => AttackEnum.tanegashima;

	public override string LocalizationTableKey => "Tanegashima";

	public override int InitialValue => 4;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = Attack.InfiniteForwardRange;


	public override string AnimationTrigger { get; protected set; } = "TanegashimaAttack";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[5]
	{
		AttackEffectEnum.ice,
		AttackEffectEnum.replay,
		AttackEffectEnum.electric,
		AttackEffectEnum.poison,
		AttackEffectEnum.perfectStrike
	};


	public override bool Begin(Agent attackingAgent)
	{
		SoundEffectsManager.Instance.Play("TanegashimaEquip");
		return base.Begin(attackingAgent);
	}

	private IEnumerator Recoil(Agent agent, Dir direction)
	{
		runningCoroutinesCounter++;
		yield return (object)new WaitForSeconds(0.05f);
		Cell cell = agent.Cell.Neighbour(direction, 1);
		if ((Object)(object)cell == (Object)null)
		{
			runningCoroutinesCounter--;
			yield break;
		}
		Agent targetRecoilAgent = cell.Agent;
		if ((Object)(object)targetRecoilAgent == (Object)null)
		{
			if (agent.IsAlive)
			{
				agent.Cell = cell;
			}
			float time = Vector3.Distance(((Component)agent).transform.position, ((Component)cell).transform.position) / recoilSpeed;
			yield return ((MonoBehaviour)this).StartCoroutine(agent.MoveToCoroutine(((Component)agent).transform.position, ((Component)cell).transform.position, time, 0f, createDustEffect: true));
			if ((Object)(object)agent == (Object)(object)Globals.Hero)
			{
				EventsManager.Instance.HeroPerformedMoveAttack.Invoke();
			}
		}
		else
		{
			Vector3 from = ((Component)agent).transform.position;
			Vector3 position = ((Component)cell).transform.position;
			Vector3 hitPoint = (from + position) / 2f;
			float time2 = Vector3.Distance(from, hitPoint) / recoilSpeed;
			yield return ((MonoBehaviour)this).StartCoroutine(agent.MoveToCoroutine(from, hitPoint, time2));
			SoundEffectsManager.Instance.Play("CombatHit");
			EffectsManager.Instance.ScreenShake();
			targetRecoilAgent.ReceiveAttack(new Hit(1, isDirectional: true, isCollision: true), agent);
			agent.ReceiveAttack(new Hit(1, isDirectional: true, isCollision: true), targetRecoilAgent);
			yield return (object)new WaitForSeconds(bounceHitAnimationTime);
			yield return ((MonoBehaviour)this).StartCoroutine(agent.MoveToCoroutine(hitPoint, from, bounceTime));
		}
		yield return (object)new WaitForSeconds(0.15f);
		runningCoroutinesCounter--;
	}

	public override void ApplyEffect()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		Agent agent = AgentInRange(attacker);
		SoundEffectsManager.Instance.Play("TanegashimaFire");
		GameObject val = EffectsManager.Instance.CreateInGameEffect("TanegashimaSmokeEffect", ((Component)attacker).transform.position);
		if (attacker.FacingDir == Dir.left)
		{
			val.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		FastBulletEffect component = EffectsManager.Instance.CreateInGameEffect("FastBulletEffect", ((Component)attacker).transform.position).GetComponent<FastBulletEffect>();
		Vector3 from = ((Component)attacker).transform.position + Vector3.up * 0.5f;
		Vector3 to = (((Object)(object)agent == (Object)null) ? (((Component)attacker).transform.position + Vector3.up * 0.5f + DirUtils.ToVec(attacker.FacingDir) * 10f) : (((Component)agent).transform.position + Vector3.up * 0.5f));
		component.Initialize(from, to);
		runningCoroutinesCounter = 0;
		if ((Object)(object)agent != (Object)null)
		{
			HitTarget(agent);
			if (agent.Movable)
			{
				((MonoBehaviour)this).StartCoroutine(Recoil(agent, attacker.FacingDir));
			}
		}
		((MonoBehaviour)this).StartCoroutine(Recoil(attacker, DirUtils.Opposite(attacker.FacingDir)));
	}

	public override bool WaitingForSomethingToFinish()
	{
		return runningCoroutinesCounter > 0;
	}
}
