using System;
using System.Collections.Generic;
using UnlocksID;

[Serializable]
public class UnlocksSaveData
{
	public List<UnlockID> unlockables = new List<UnlockID>();

	public List<UnlockID> unlocked = new List<UnlockID>();

	public List<UnlockID> recentlyUnlockedTiles = new List<UnlockID>();

	public List<UnlockID> unlockedDuringThisRun = new List<UnlockID>();

	public UnlocksSaveData()
	{
		foreach (UnlockID value in Enum.GetValues(typeof(UnlockID)))
		{
			unlockables.Add(value);
		}
		unlocked = new List<UnlockID>
		{
			UnlockID.h_wanderer,
			UnlockID.t_sword,
			UnlockID.t_arrow,
			UnlockID.t_spear,
			UnlockID.t_swirl,
			UnlockID.t_dashForward,
			UnlockID.t_dashBackward,
			UnlockID.t_shadowDash,
			UnlockID.t_shuriken,
			UnlockID.s_consumableslot,
			UnlockID.s_backstab,
			UnlockID.s_sniper,
			UnlockID.s_mindfulness,
			UnlockID.s_unfriendlyfire,
			UnlockID.s_combocoin,
			UnlockID.s_comborecharge,
			UnlockID.s_combocurse,
			UnlockID.s_comboheal,
			UnlockID.s_maxhpup,
			UnlockID.s_shieldretention,
			UnlockID.s_shieldeveryfight,
			UnlockID.s_reactiveshield,
			UnlockID.s_smovebackwards,
			UnlockID.s_smovecooldown,
			UnlockID.s_dyno,
			UnlockID.s_smovedamage
		};
	}
}
