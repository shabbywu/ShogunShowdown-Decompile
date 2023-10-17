using System.Collections.Generic;
using Utils;

namespace TileEnums;

public static class TileEnumsUtils
{
	private static Dictionary<AttackEffectEnum, string> AttackEffectNames = new Dictionary<AttackEffectEnum, string>
	{
		[AttackEffectEnum.none] = "NoEffect",
		[AttackEffectEnum.ice] = "Ice",
		[AttackEffectEnum.replay] = "DoubleStrike",
		[AttackEffectEnum.electric] = "Electric",
		[AttackEffectEnum.poison] = "Poison",
		[AttackEffectEnum.perfectStrike] = "PerfectStrike"
	};

	private static Dictionary<TileEffectEnum, string> TileEffectNames = new Dictionary<TileEffectEnum, string>
	{
		[TileEffectEnum.none] = "NoEffect",
		[TileEffectEnum.freePlay] = "FreePlay",
		[TileEffectEnum.preFlip] = "PreFlip"
	};

	public static string LocalizedAttackEffectName(AttackEffectEnum attackEffect)
	{
		return LocalizationUtils.LocalizedString("TileEffects", AttackEffectNames[attackEffect] + "_Name");
	}

	public static string LocalizedAttackEffectDescription(AttackEffectEnum attackEffect)
	{
		return LocalizationUtils.LocalizedString("TileEffects", AttackEffectNames[attackEffect] + "_Description");
	}

	public static string LocalizedTileEffectName(TileEffectEnum tileEffect)
	{
		return LocalizationUtils.LocalizedString("TileEffects", TileEffectNames[tileEffect] + "_Name");
	}

	public static string LocalizedTileEffectDescription(TileEffectEnum tileEffect)
	{
		return LocalizationUtils.LocalizedString("TileEffects", TileEffectNames[tileEffect] + "_Description");
	}
}
