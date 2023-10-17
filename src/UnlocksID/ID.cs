using System;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;

namespace UnlocksID;

public static class ID
{
	public static Dictionary<UnlockID, AttackEnum> tilesID = new Dictionary<UnlockID, AttackEnum>
	{
		[UnlockID.t_sword] = AttackEnum.sword,
		[UnlockID.t_arrow] = AttackEnum.arrow,
		[UnlockID.t_spear] = AttackEnum.spear,
		[UnlockID.t_swirl] = AttackEnum.swirl,
		[UnlockID.t_dashForward] = AttackEnum.dashForward,
		[UnlockID.t_dashBackward] = AttackEnum.dashBackward,
		[UnlockID.t_shuriken] = AttackEnum.shuriken,
		[UnlockID.t_shadowDash] = AttackEnum.shadowDash,
		[UnlockID.t_bo] = AttackEnum.bo,
		[UnlockID.t_lightning] = AttackEnum.lightning,
		[UnlockID.t_dragon_punch] = AttackEnum.dragonPunch,
		[UnlockID.t_grapplingHook] = AttackEnum.grapplingHook,
		[UnlockID.t_trap] = AttackEnum.trap,
		[UnlockID.t_smokeBomb] = AttackEnum.smokeBomb,
		[UnlockID.t_earthImaple] = AttackEnum.earthImpale,
		[UnlockID.t_backStrike] = AttackEnum.backStrike,
		[UnlockID.t_shadowKama] = AttackEnum.shadowKama,
		[UnlockID.t_tetsubo] = AttackEnum.tetsubo,
		[UnlockID.t_kunai] = AttackEnum.kunai,
		[UnlockID.t_bladeOfPatience] = AttackEnum.bladeOfPatience,
		[UnlockID.t_tanegashima] = AttackEnum.tanegashima,
		[UnlockID.t_crossbow] = AttackEnum.crossbow,
		[UnlockID.t_mark] = AttackEnum.mark,
		[UnlockID.t_turnAround] = AttackEnum.turnAround,
		[UnlockID.t_phantomLeap] = AttackEnum.phantomLeap,
		[UnlockID.t_mirror] = AttackEnum.mirror,
		[UnlockID.t_swapToss] = AttackEnum.swapToss,
		[UnlockID.t_origin] = AttackEnum.origin,
		[UnlockID.t_scarStrike] = AttackEnum.scarStrike,
		[UnlockID.t_signatureMove] = AttackEnum.signatureMove,
		[UnlockID.t_meteorHammer] = AttackEnum.meteorHammer,
		[UnlockID.t_kaitenryuken] = AttackEnum.kaitenryuken
	};

	public static Dictionary<UnlockID, Type> skillsID = new Dictionary<UnlockID, Type>
	{
		[UnlockID.s_consumableslot] = typeof(MorePotionsSlotsItem),
		[UnlockID.s_backstab] = typeof(BackstabberItem),
		[UnlockID.s_sniper] = typeof(SniperItem),
		[UnlockID.s_unfriendlyfire] = typeof(UnfrienflyFriendlyFireItem),
		[UnlockID.s_mindfulness] = typeof(MindfulnessItem),
		[UnlockID.s_monomancer] = typeof(MonomancerItem),
		[UnlockID.s_combocoin] = typeof(ComboCoinItem),
		[UnlockID.s_comborecharge] = typeof(ComboRechargeItem),
		[UnlockID.s_comboheal] = typeof(ComboHealItem),
		[UnlockID.s_combocurse] = typeof(ComboCurseItem),
		[UnlockID.s_combodeal] = typeof(ComboDealItem),
		[UnlockID.s_maxhpup] = typeof(HealthyItem),
		[UnlockID.s_shieldeveryfight] = typeof(ShieldAtBeginningOfCombatItem),
		[UnlockID.s_reactiveshield] = typeof(ReactiveShieldItem),
		[UnlockID.s_shieldretention] = typeof(ShieldRetentionItem),
		[UnlockID.s_karma] = typeof(KarmaItem),
		[UnlockID.s_smovebackwards] = typeof(SpecialMoveBackwardsItem),
		[UnlockID.s_smovecooldown] = typeof(SpecialMoveCooldownItem),
		[UnlockID.s_smovedamage] = typeof(SpecialMoveDamageItem),
		[UnlockID.s_dyno] = typeof(DynamicAttackBoostItem),
		[UnlockID.s_cursingmove] = typeof(CursingMoveItem)
	};

	public static UnlockType GetUnlockType(UnlockID id)
	{
		foreach (UnlockType value in Enum.GetValues(typeof(UnlockType)))
		{
			int num = (int)id - (int)value;
			if (num >= 0 && num <= 1000)
			{
				return value;
			}
		}
		Debug.LogError((object)$"GetUnlockType: I should not get here! unlockID: '{id}' (int value: {(int)id})");
		return UnlockType.quest;
	}
}
