using SkillEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class BackstabberItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.backstabber;


	public override int MaxLevel { get; protected set; } = 10;


	public override string LocalizationTableKey { get; } = "BackStabber";


	private int ExtraDamage => base.Level;

	protected override string ProcessDescription(string description)
	{
		return string.Format(description, ExtraDamage);
	}

	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).AddListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Agent, Agent, Hit>)EventsManager.Instance.Attack).RemoveListener((UnityAction<Agent, Agent, Hit>)ProcessAttack);
	}

	private void ProcessAttack(Agent attacker, Agent defender, Hit hit)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (hit.IsDirectional && !((Object)(object)attacker != (Object)(object)Globals.Hero))
		{
			if ((Object)(object)attacker.CellBeforeExecution == (Object)null)
			{
				Debug.LogError((object)"Issue with BackstabberItem: attacker.CellBeforeExecution is null");
			}
			else if (Vector3.Dot(((Component)attacker.CellBeforeExecution).transform.position - ((Component)defender.Cell).transform.position, DirUtils.ToVec(defender.FacingDir)) < 0f)
			{
				hit.Damage += ExtraDamage;
				SoundEffectsManager.Instance.Play("SpecialHit");
			}
		}
	}
}
