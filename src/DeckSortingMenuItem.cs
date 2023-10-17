using TMPro;

public class DeckSortingMenuItem : OptionsMenuItem
{
	public override void OnSubmit()
	{
		if (Globals.Options.handSorting == Options.HandSorting.cooldown)
		{
			Globals.Options.handSorting = Options.HandSorting.fixOrder;
		}
		else if (Globals.Options.handSorting == Options.HandSorting.fixOrder)
		{
			Globals.Options.handSorting = Options.HandSorting.cooldown;
		}
		TilesManager.Instance.hand.UpdateHandState();
	}

	public override void UpdateState()
	{
		if (Globals.Options.handSorting == Options.HandSorting.cooldown)
		{
			((TMP_Text)text).text = "DECK SORTING: BY COOLDOWN";
		}
		else if (Globals.Options.handSorting == Options.HandSorting.fixOrder)
		{
			((TMP_Text)text).text = "DECK SORTING: FIX ORDER";
		}
	}

	public override void OnLeft()
	{
	}

	public override void OnRight()
	{
	}
}
