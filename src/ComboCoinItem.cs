using PickupEnums;
using SkillEnums;
using UnityEngine.Events;

public class ComboCoinItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.combo_coin;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "ComboCoin";


	public override void PickUp()
	{
		base.PickUp();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).AddListener((UnityAction<Enemy>)ComboKill);
	}

	private void ComboKill(Enemy enemy)
	{
		PickupFactory.Instance.InstantiatePickup(PickupEnum.coin, enemy.Cell);
	}

	public override void Remove()
	{
		base.Remove();
		((UnityEvent<Enemy>)EventsManager.Instance.ComboKill).RemoveListener((UnityAction<Enemy>)ComboKill);
	}
}
