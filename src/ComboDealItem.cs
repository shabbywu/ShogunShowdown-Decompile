using System.Collections.Generic;
using PickupEnums;
using SkillEnums;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class ComboDealItem : Item
{
	private int nConsumableUsedInTurn;

	public override SkillEnum SkillEnum { get; } = SkillEnum.combo_deal;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ComboDeal";


	public override void PickUp()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		base.PickUp();
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(ResetNumberOfConsumableUsed));
		EventsManager.Instance.BeginningOfCombat.AddListener(new UnityAction(ResetNumberOfConsumableUsed));
		EventsManager.Instance.PotionUsed.AddListener((UnityAction<Potion>)OnPotionUsed);
	}

	public override void Remove()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		base.Remove();
		EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(ResetNumberOfConsumableUsed));
		EventsManager.Instance.BeginningOfCombat.RemoveListener(new UnityAction(ResetNumberOfConsumableUsed));
		EventsManager.Instance.PotionUsed.RemoveListener((UnityAction<Potion>)OnPotionUsed);
	}

	private void ResetNumberOfConsumableUsed()
	{
		nConsumableUsedInTurn = 0;
	}

	private void OnPotionUsed(Potion potion)
	{
		nConsumableUsedInTurn++;
		if (nConsumableUsedInTurn % 2 == 0)
		{
			SpawnAppropriatePickup();
		}
	}

	private void SpawnAppropriatePickup()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		List<PickupEnum> list = new List<PickupEnum> { PickupEnum.coin };
		PickupEnum pickupEnum;
		do
		{
			pickupEnum = MyRandom.NextEnum<PickupEnum>();
		}
		while (list.Contains(pickupEnum));
		PickupFactory.Instance.InstantiatePickup(pickupEnum, Globals.Hero.Cell, ((Component)Globals.Hero).transform.position + Vector3.up * 2f);
	}
}
