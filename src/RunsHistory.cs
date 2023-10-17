using System;
using System.Collections.Generic;

[Serializable]
public class RunsHistory
{
	public List<RunMetrics> runs;

	public RunsHistory()
	{
		runs = new List<RunMetrics>();
	}
}
