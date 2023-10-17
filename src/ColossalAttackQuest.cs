using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ColossalAttackQuest", menuName = "SO/Quests/ColossalAttackQuest", order = 1)]
public class ColossalAttackQuest : Quest
{
	[SerializeField]
	private int damageThreshold;

	private int totalDamageDealt;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, damageThreshold);
	}

	public override void Initialize()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(EndOfCombatTurn));
		EventsManager.Instance.HeroDealtDamage.AddListener((UnityAction<int>)HeroDealtDamage);
	}

	public override void FinalizeQuest()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(EndOfCombatTurn));
		EventsManager.Instance.HeroDealtDamage.RemoveListener((UnityAction<int>)HeroDealtDamage);
	}

	private void EndOfCombatTurn()
	{
		if (totalDamageDealt > damageThreshold)
		{
			QuestCompleted();
		}
		totalDamageDealt = 0;
	}

	private void HeroDealtDamage(int damage)
	{
		totalDamageDealt += damage;
	}
}
