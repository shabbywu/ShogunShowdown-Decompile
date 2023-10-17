namespace CombatEnums;

public static class CombatEnumsUtils
{
	public static string EnemyActionDescription(ActionEnum action)
	{
		string result = "";
		switch (action)
		{
		case ActionEnum.wait:
			result = "waiting...";
			break;
		case ActionEnum.moveLeft:
		case ActionEnum.moveRight:
			result = "about to move";
			break;
		case ActionEnum.attack:
			result = "about to attack!";
			break;
		case ActionEnum.playTile:
			result = "about to play a tile";
			break;
		case ActionEnum.flipLeft:
		case ActionEnum.flipRight:
			result = "about to turn around";
			break;
		}
		return result;
	}
}
