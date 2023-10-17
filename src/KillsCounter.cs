public class KillsCounter : CurrencyCounter
{
	protected override void SetUpdateValueEvent()
	{
		base.UpdateValueEvent = EventsManager.Instance.MetaCurrencyUpdate;
	}
}
