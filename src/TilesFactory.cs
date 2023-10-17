using System;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;
using Utils;

public class TilesFactory : MonoBehaviour
{
	public GameObject tilePrefab;

	public static TilesFactory Instance { get; private set; }

	public TileSprites Sprites { get; private set; }

	public PseudoRandomWithMemory<AttackEnum> PseudoRandomAttackEnumsGenerator { get; private set; }

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
		Sprites = ((Component)this).GetComponent<TileSprites>();
	}

	public Tile Create(AttackEnum attackEnum, int maxLevel = 0, AttackEffectEnum attackEffect = AttackEffectEnum.none, TileEffectEnum tileEffect = TileEffectEnum.none)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(tilePrefab, ((Component)this).transform.position, Quaternion.identity);
		if (!Enum.IsDefined(typeof(AttackEnum), attackEnum))
		{
			Debug.LogWarning((object)$"TileFactory => Create: attackEnum '{attackEnum}' not defined. Creating a Sword instead.");
			attackEnum = AttackEnum.sword;
		}
		Attack attack;
		switch (attackEnum)
		{
		case AttackEnum.sword:
			attack = val.AddComponent<MeleeAttack>();
			break;
		case AttackEnum.arrow:
			attack = val.AddComponent<ArrowAttack>();
			break;
		case AttackEnum.spear:
			attack = val.AddComponent<SpearAttack>();
			break;
		case AttackEnum.swirl:
			attack = val.AddComponent<SwirlAttack>();
			break;
		case AttackEnum.bo:
			attack = val.AddComponent<BoAttack>();
			break;
		case AttackEnum.shield:
			attack = val.AddComponent<ShieldAttack>();
			break;
		case AttackEnum.lightning:
			attack = val.AddComponent<LightningAttack>();
			break;
		case AttackEnum.dragonPunch:
			attack = val.AddComponent<DragonPunchAttack>();
			break;
		case AttackEnum.grapplingHook:
			attack = val.AddComponent<GrapplingHookAttack>();
			break;
		case AttackEnum.trap:
			attack = val.AddComponent<TrapAttack>();
			break;
		case AttackEnum.bomb:
			attack = val.AddComponent<BombAttack>();
			break;
		case AttackEnum.wind:
			attack = val.AddComponent<WindAttack>();
			break;
		case AttackEnum.dashForward:
			attack = val.AddComponent<DashForwardAttack>();
			break;
		case AttackEnum.dashBackward:
			attack = val.AddComponent<DashBackwardAttack>();
			break;
		case AttackEnum.shadowDash:
			attack = val.AddComponent<ShadowDashAttack>();
			break;
		case AttackEnum.smokeBomb:
			attack = val.AddComponent<SmokeBombAttack>();
			break;
		case AttackEnum.curse:
			attack = val.AddComponent<CurseAttack>();
			break;
		case AttackEnum.blessing:
			attack = val.AddComponent<BlessingAttack>();
			break;
		case AttackEnum.shuriken:
			attack = val.AddComponent<ShurikenAttack>();
			break;
		case AttackEnum.shurikens:
			attack = val.AddComponent<ShurikensAttack>();
			break;
		case AttackEnum.mark:
			attack = val.AddComponent<MarkAttack>();
			break;
		case AttackEnum.turnAround:
			attack = val.AddComponent<TurnAroundAttack>();
			break;
		case AttackEnum.earthImpale:
			attack = val.AddComponent<EarthImpaleAttack>();
			break;
		case AttackEnum.mirror:
			attack = val.AddComponent<MirrorAttack>();
			break;
		case AttackEnum.backStrike:
			attack = val.AddComponent<BackStrikeAttack>();
			break;
		case AttackEnum.shadowKama:
			attack = val.AddComponent<ShadowKamaAttack>();
			break;
		case AttackEnum.tetsubo:
			attack = val.AddComponent<TetsuboAttack>();
			break;
		case AttackEnum.kunai:
			attack = val.AddComponent<KunaiAttack>();
			break;
		case AttackEnum.summon:
			attack = val.AddComponent<SummonAttack>();
			break;
		case AttackEnum.bladeOfPatience:
			attack = val.AddComponent<BladeOfPatienceAttack>();
			break;
		case AttackEnum.phantomLeap:
			attack = val.AddComponent<PhantomLeap>();
			break;
		case AttackEnum.crossbow:
			attack = val.AddComponent<CrossbowAttack>();
			break;
		case AttackEnum.swapToss:
			attack = val.AddComponent<SwapTossAttack>();
			break;
		case AttackEnum.origin:
			attack = val.AddComponent<OriginAttack>();
			break;
		case AttackEnum.tanegashima:
			attack = val.AddComponent<TanegashimaAttack>();
			break;
		case AttackEnum.shieldAllied:
			attack = val.AddComponent<ShieldAlliedAttack>();
			break;
		case AttackEnum.blazingBarrage:
			attack = val.AddComponent<BlazingBarrageAttack>();
			break;
		case AttackEnum.scarStrike:
			attack = val.AddComponent<ScarStrikeAttack>();
			break;
		case AttackEnum.signatureMove:
			attack = val.AddComponent<SignatureMoveAttack>();
			break;
		case AttackEnum.meteorHammer:
			attack = val.AddComponent<MeteorHammerAttack>();
			break;
		case AttackEnum.kaitenryuken:
			attack = val.AddComponent<KaitenryukenAttack>();
			break;
		case AttackEnum.volley:
			attack = val.AddComponent<VolleyAttack>();
			break;
		case AttackEnum.barricade:
			attack = val.AddComponent<BarricadeAttack>();
			break;
		default:
			attack = null;
			Debug.LogError((object)"TileFactory => Create: type not found");
			break;
		}
		attack.AttackEffect = attackEffect;
		attack.TileEffect = tileEffect;
		attack.Initialize(maxLevel);
		Tile component = val.GetComponent<Tile>();
		component.Initialize(attack);
		return component;
	}

	public Tile Create(TileSaveData tileSaveData)
	{
		Tile tile = Create(tileSaveData.attackEnum, tileSaveData.maxLevel, tileSaveData.attackEffect, tileSaveData.tileEffect);
		tile.Attack.Cooldown = tileSaveData.cooldown;
		tile.Attack.Value = tileSaveData.value;
		tile.Attack.BaseValue = tileSaveData.baseValue;
		tile.Attack.Level = tileSaveData.level;
		tile.Initialize(tile.Attack);
		tile.CooldownCharge = tileSaveData.cooldownCharge;
		tile.TileIsEnabled = tileSaveData.cooldown == tileSaveData.cooldownCharge;
		return tile;
	}

	public List<Tile> Create(List<TileSaveData> tileSaveDatas)
	{
		List<Tile> list = new List<Tile>();
		foreach (TileSaveData tileSaveData in tileSaveDatas)
		{
			list.Add(Create(tileSaveData));
		}
		return list;
	}

	public void InitializeRandomGenerator()
	{
		List<(AttackEnum, float)> list = new List<(AttackEnum, float)>();
		foreach (AttackEnum unlockedTile in UnlocksManager.Instance.GetUnlockedTiles())
		{
			list.Add((unlockedTile, 1f));
		}
		PseudoRandomAttackEnumsGenerator = new PseudoRandomWithMemory<AttackEnum>(list.ToArray(), 1.5f, allowSameConsecutiveResults: false);
	}
}
