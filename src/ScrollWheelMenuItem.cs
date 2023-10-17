using System;
using TMPro;
using Utils;

public class ScrollWheelMenuItem : OptionsMenuItem
{
	private enum State
	{
		natural,
		inverted,
		disabled
	}

	public override void OnSubmit()
	{
		SelectNext(1);
	}

	public override void OnLeft()
	{
		SelectNext(-1);
	}

	public override void OnRight()
	{
		SelectNext(1);
	}

	public override void UpdateState()
	{
		switch (GetStateFromOptions(Globals.Options))
		{
		case State.natural:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scroll_Natural");
			break;
		case State.inverted:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scroll_Inverted");
			break;
		case State.disabled:
			((TMP_Text)text).text = LocalizationUtils.LocalizedString("Menu", "Controls_Scroll_Disabled");
			break;
		}
	}

	private State GetStateFromOptions(Options options)
	{
		if (options.mouseScrollDisabled)
		{
			return State.disabled;
		}
		if (options.invertedScroll)
		{
			return State.inverted;
		}
		return State.natural;
	}

	private void SelectNext(int delta)
	{
		State stateFromOptions = GetStateFromOptions(Globals.Options);
		switch ((State)MyMath.ModularizeIndex((int)(stateFromOptions + delta), Enum.GetValues(typeof(State)).Length))
		{
		case State.natural:
			Globals.Options.invertedScroll = false;
			Globals.Options.mouseScrollDisabled = false;
			break;
		case State.inverted:
			Globals.Options.invertedScroll = true;
			Globals.Options.mouseScrollDisabled = false;
			break;
		case State.disabled:
			Globals.Options.invertedScroll = false;
			Globals.Options.mouseScrollDisabled = true;
			break;
		}
		InteractionEffect();
		UpdateState();
	}
}
