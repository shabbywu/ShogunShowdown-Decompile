using UnityEngine;

namespace UINavigation;

public interface INavigationTarget
{
	Transform Transform { get; }

	void Select();

	void Deselect();

	void Submit();
}
