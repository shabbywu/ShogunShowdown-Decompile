using TileEnums;
using UnityEngine;

namespace Utils;

public static class Colors
{
	public static string whiteHex = "ffffff";

	public static string redHex = "a53030";

	public static string orangeHex = "cf573c";

	public static string birghtYellowHex = "f8c53a";

	public static string paleYellowHex = "e8c170";

	public static string lightCobaltHex = "c7cfcc";

	public static string midCobaltHex = "819796";

	public static string pinkishHex = "df84a5";

	public static string celesteHex = "a4dddb";

	public static string greenHex = "a8ca58";

	public static string darkRedHex = "752438";

	public static string darkGrayHex = "363636";

	public static string grayHex = "757575";

	public static string lightGrayHex = "b3b3b3";

	public static string brigthRedHex = "d61101";

	public static Color FromHex(string hexString)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Color result = default(Color);
		ColorUtility.TryParseHtmlString("#" + hexString, ref result);
		return result;
	}

	public static string HexColorForAttackEffect(AttackEffectEnum attackEffect)
	{
		switch (attackEffect)
		{
		case AttackEffectEnum.none:
			return whiteHex;
		case AttackEffectEnum.ice:
			return "253a5e";
		case AttackEffectEnum.replay:
			return "411d31";
		case AttackEffectEnum.electric:
			return "be772b";
		case AttackEffectEnum.poison:
			return "25562e";
		default:
			Debug.LogError((object)$"HexColorForAttackEffect: no color defined for '{attackEffect}'");
			return "";
		}
	}

	public static string HexColorForTileEffect(TileEffectEnum tileEffect)
	{
		switch (tileEffect)
		{
		case TileEffectEnum.none:
			return whiteHex;
		case TileEffectEnum.freePlay:
			return "411d31";
		case TileEffectEnum.preFlip:
			return redHex;
		default:
			Debug.LogError((object)$"HexColorForTileEffect: no color defined for '{tileEffect}'");
			return "";
		}
	}
}
