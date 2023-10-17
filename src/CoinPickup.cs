using PickupEnums;

public class CoinPickup : Pickup
{
	public int value;

	public override PickupEnum PickupEnum { get; }

	protected override bool CanPickUp => true;

	public override string InfoBoxText => "Coin should not have an infobox";

	protected override void PickUpEffect()
	{
		Globals.Coins += value;
	}
}
