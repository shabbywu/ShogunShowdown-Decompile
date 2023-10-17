using System.Collections.Generic;
using SkillEnums;
using UnityEngine.Events;
using Utils;

public class ComboCurseItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.combo_curse;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ComboCurse";


	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)ComboKill);
	}

	private void ComboKill(Enemy enemy)
	{
		List<Enemy> list = CombatManager.Instance.Enemies.FindAll((Enemy e) => !e.AgentStats.mark && e.IsAlive);
		if (list.Count > 0)
		{
			MyRandom.NextRandomUniform(list).GetMarked();
		}
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).RemoveListener((UnityAction<Enemy>)ComboKill);
	}
}
