public class CoinsCounter : CurrencyCounter
{
	protected override void SetUpdateValueEvent()
	{
		base.UpdateValueEvent = EventsManager.Instance.CoinsUpdate;
	}
}
