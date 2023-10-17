using PickupEnums;
using UnityEngine;

public class PotionPickup : Pickup
{
	[SerializeField]
	private Potion potionPrefab;

	[SerializeField]
	private PickupEnum pickupEnum;

	public override PickupEnum PickupEnum => pickupEnum;

	public Potion PotionPrefab => potionPrefab;

	protected override bool CanPickUp => PotionsManager.Instance.CanPickUpPotion;

	public string Name => potionPrefab.Name;

	public string Description => potionPrefab.Description;

	public override string InfoBoxText => potionPrefab.InfoBoxText;

	protected override void PickUpEffect()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(((Component)potionPrefab).gameObject, Vector3.zero, Quaternion.identity);
		PotionsManager.Instance.TakePotion(val.GetComponent<Potion>());
	}

	public override void ForcePickUp()
	{
		if (!CanPickUp)
		{
			Globals.Coins++;
		}
		base.ForcePickUp();
	}
}
