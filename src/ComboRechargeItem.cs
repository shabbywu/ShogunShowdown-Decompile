using SkillEnums;
using UnityEngine;
using UnityEngine.Events;

public class ComboRechargeItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.combo_recharge;


	public override int MaxLevel { get; protected set; } = 2;


	public override string LocalizationTableKey { get; } = "ComboRecharge";


	private int CooldownRecharged => 4 * base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, CooldownRecharged);
	}

	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)OnComboKill);
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).RemoveListener((UnityAction<Enemy>)OnComboKill);
	}

	private void OnComboKill(Enemy enemy)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Globals.Hero.tileInProgress != (Object)null)
		{
			Globals.Hero.tileInProgress.PostExecutionCooldownRecharge += CooldownRecharged;
			Globals.Hero.tileInProgress.OriginOfPostExecutionCooldownRechargeEffect = ((Component)enemy).transform.position;
		}
	}
}
