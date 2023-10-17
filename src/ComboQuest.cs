using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewComboQuest", menuName = "SO/Quests/ComboQuest", order = 1)]
public class ComboQuest : Quest
{
	[SerializeField]
	private int nCombos;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, nCombos);
	}

	public override void Initialize()
	{
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)UponComboKill);
	}

	public override void FinalizeQuest()
	{
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).RemoveListener((UnityAction<Enemy>)UponComboKill);
	}

	private void UponComboKill(Enemy enemy)
	{
		if (CombatManager.Instance.KillStreak >= nCombos)
		{
			QuestCompleted();
		}
	}
}
