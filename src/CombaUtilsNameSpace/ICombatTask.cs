using System.Collections;

namespace CombaUtilsNameSpace;

public interface ICombatTask
{
	bool IsFinished { get; }

	IEnumerator Execute();

	void FinalizeTask();
}
