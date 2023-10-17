using UnityEngine;

public class GoToFight : MonoBehaviour
{
	public MyButton button;

	public bool disappearUponStart;

	private void Start()
	{
		if (disappearUponStart)
		{
			button.Disappear();
		}
	}

	public void GoToFightSelected()
	{
		button.Disappear();
		if (CombatManager.Instance.CombatInProgress)
		{
			EventsManager.Instance.EndOfCombat.Invoke();
		}
		else
		{
			CombatSceneManager.Instance.Room.End();
		}
	}

	public void Enable()
	{
		button.Appear();
		button.Interactable = true;
	}

	public void Disable()
	{
		button.Disappear();
	}
}
