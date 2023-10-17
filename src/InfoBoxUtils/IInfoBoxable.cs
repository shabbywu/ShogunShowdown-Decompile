namespace InfoBoxUtils;

public interface IInfoBoxable
{
	string InfoBoxText { get; }

	bool InfoBoxEnabled { get; }

	BoxWidth BoxWidth { get; }

	int MaxWidth => -1;
}
