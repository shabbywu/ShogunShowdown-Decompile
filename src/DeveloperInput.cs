using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeveloperInput : MonoBehaviour
{
	[SerializeField]
	private DevModeInfo devModeInfo;

	public void DevToggleQuickMode(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			Globals.Quick = !Globals.Quick;
			devModeInfo.UpdateInfo();
		}
	}

	public void DevToggleAutoSave(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			Globals.AutoSave = !Globals.AutoSave;
			devModeInfo.UpdateInfo();
		}
	}

	public void DevNext(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			if (CombatManager.Instance.CombatInProgress)
			{
				CombatManager.Instance.KillEnemies();
				EventsManager.Instance.EndOfCombat.Invoke();
			}
			else if (CombatSceneManager.Instance.Room is RewardRoom)
			{
				((RewardRoom)CombatSceneManager.Instance.Room).SkipButtonPressed();
			}
		}
	}

	public void DevQuickRestart(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			SceneManager.LoadScene("ResetGameState");
		}
	}

	public void DevExtraMoney(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			Globals.Coins += 100;
		}
	}

	public void DevExtraSkulls(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			Globals.KillCount += 10;
		}
	}

	public void DevKillEnemies(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed && (Object)(object)CombatManager.Instance != (Object)null)
		{
			CombatManager.Instance.KillEnemies();
		}
	}

	public void DevHeal(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed && (Object)(object)CombatManager.Instance != (Object)null)
		{
			Globals.Hero.FullHeal();
		}
	}

	public void DevMapUnlock(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed && MapManager.Instance.mapScreen.IsOpen)
		{
			MapManager.Instance.map.DeveloperUncoverAllLocations();
		}
	}

	public void DevShortLocations(CallbackContext context)
	{
		if (Globals.Developer && ((CallbackContext)(ref context)).performed)
		{
			Globals.ShortLocations = !Globals.ShortLocations;
			devModeInfo.UpdateInfo();
		}
	}
}
