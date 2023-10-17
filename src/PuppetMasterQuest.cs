using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PuppetMasterQuest", menuName = "SO/Quests/PuppetMasterQuest", order = 1)]
public class PuppetMasterQuest : Quest
{
	[SerializeField]
	private int nKills;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, nKills);
	}

	public override void Initialize()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EnemyFriendlyKill.AddListener(new UnityAction(EnemyFriendlyKill));
	}

	public override void FinalizeQuest()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EnemyFriendlyKill.RemoveListener(new UnityAction(EnemyFriendlyKill));
	}

	private void EnemyFriendlyKill()
	{
		if (MetricsManager.Instance.runMetrics.runStats.friendlyKills >= nKills)
		{
			QuestCompleted();
		}
	}
}
