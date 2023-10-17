using System.Collections.Generic;
using UnityEngine;

namespace UINavigation;

public static class UINavigationHelper
{
	public static NavigationDirection DirectionFromXY(int x, int y)
	{
		if (x > 0)
		{
			return NavigationDirection.right;
		}
		if (x < 0)
		{
			return NavigationDirection.left;
		}
		if (y > 0)
		{
			return NavigationDirection.up;
		}
		if (y < 0)
		{
			return NavigationDirection.down;
		}
		return NavigationDirection.none;
	}

	public static Vector2 XYFromDirection(NavigationDirection navigationDirection)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		return (Vector2)(navigationDirection switch
		{
			NavigationDirection.up => new Vector2(0f, 1f), 
			NavigationDirection.down => new Vector2(0f, -1f), 
			NavigationDirection.left => new Vector2(-1f, 0f), 
			NavigationDirection.right => new Vector2(1f, 0f), 
			_ => new Vector2(0f, 0f), 
		});
	}

	public static void SelectNewTarget(INavigationGroup navigationGroup, INavigationTarget newTarget)
	{
		if (newTarget != navigationGroup.SelectedTarget)
		{
			navigationGroup.SelectedTarget?.Deselect();
			navigationGroup.SelectedTarget = newTarget;
			navigationGroup.SelectedTarget?.Select();
			EventsManager.Instance.NavigationTargetChanged.Invoke(newTarget);
		}
	}

	public static INavigationGroup HandleOutOfGroupNavigation(INavigationGroup group, NavigationDirection direction, NavigationDirection? entryDirection = null)
	{
		if (group.ConnectedGroups.ContainsKey(direction))
		{
			INavigationGroup navigationGroup = group.ConnectedGroups[direction];
			if (navigationGroup == null)
			{
				return group;
			}
			if (!navigationGroup.CanBeNavigatedTo)
			{
				INavigationGroup navigationGroup2 = HandleOutOfGroupNavigation(navigationGroup, direction);
				if (navigationGroup2 == navigationGroup)
				{
					return group;
				}
				navigationGroup = navigationGroup2;
			}
			group.SelectedTarget?.Deselect();
			navigationGroup.OnEntry(entryDirection ?? direction, group.SelectedTarget);
			group.SelectedTarget = null;
			return navigationGroup;
		}
		return group;
	}

	public static void InitializeConnectedGroups(INavigationGroup navigationGroup, INavigationGroup up = null, INavigationGroup down = null, INavigationGroup left = null, INavigationGroup right = null)
	{
		navigationGroup.ConnectedGroups = new Dictionary<NavigationDirection, INavigationGroup>
		{
			[NavigationDirection.up] = up,
			[NavigationDirection.down] = down,
			[NavigationDirection.left] = left,
			[NavigationDirection.right] = right,
			[NavigationDirection.none] = navigationGroup
		};
	}

	public static NavigationAxis Axis(NavigationDirection direction)
	{
		switch (direction)
		{
		case NavigationDirection.up:
		case NavigationDirection.down:
			return NavigationAxis.vertical;
		case NavigationDirection.left:
		case NavigationDirection.right:
			return NavigationAxis.horizontal;
		default:
			Debug.LogError((object)$"Axis: invalid direction {direction}");
			return NavigationAxis.vertical;
		}
	}

	public static INavigationTarget GetNavigationTargetFromDirection(INavigationTarget initial, List<INavigationTarget> targets, NavigationDirection direction, float minDelta = 0.1f)
	{
		List<INavigationTarget> targets2 = FindTargetsInDirection(initial, targets, direction, minDelta);
		return FindClosetsTargetWithDirectionCompression(initial, targets2, direction);
	}

	private static List<INavigationTarget> FindTargetsInDirection(INavigationTarget initial, List<INavigationTarget> targets, NavigationDirection direction, float minDelta)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = XYFromDirection(direction);
		List<INavigationTarget> list = new List<INavigationTarget>();
		foreach (INavigationTarget target in targets)
		{
			Vector2 val2 = Vector2.Scale(Vector2.op_Implicit(target.Transform.position - initial.Transform.position), val);
			if (val2.x > minDelta || val2.y > minDelta)
			{
				list.Add(target);
			}
		}
		return list;
	}

	public static (INavigationGroup, INavigationTarget) FindClosestGroupAndTarget(INavigationTarget origin, List<INavigationGroup> groups)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		INavigationGroup item = null;
		INavigationTarget item2 = null;
		float num = float.PositiveInfinity;
		foreach (INavigationGroup group in groups)
		{
			INavigationTarget navigationTarget = FindClosetsTarget(origin, group.Targets);
			if (navigationTarget != null)
			{
				float num2 = Vector3.SqrMagnitude(navigationTarget.Transform.position - origin.Transform.position);
				if (num2 < num)
				{
					num = num2;
					item = group;
					item2 = navigationTarget;
				}
			}
		}
		return (item, item2);
	}

	public static INavigationTarget FindClosetsTarget(INavigationTarget initial, List<INavigationTarget> targets)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		INavigationTarget result = null;
		float num = float.PositiveInfinity;
		foreach (INavigationTarget target in targets)
		{
			float num2 = Vector3.SqrMagnitude(target.Transform.position - initial.Transform.position);
			if (num2 < num)
			{
				num = num2;
				result = target;
			}
		}
		return result;
	}

	private static INavigationTarget FindClosetsTargetWithDirectionCompression(INavigationTarget initial, List<INavigationTarget> targets, NavigationDirection direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 direction2 = XYFromDirection(direction);
		INavigationTarget result = null;
		float num = float.PositiveInfinity;
		foreach (INavigationTarget target in targets)
		{
			Vector2 val = CompressAlongDirection(Vector2.op_Implicit(target.Transform.position - initial.Transform.position), direction2);
			float magnitude = ((Vector2)(ref val)).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
				result = target;
			}
		}
		return result;
	}

	public static INavigationTarget GetClosestNavigationTargetToPosition(List<INavigationTarget> targets, Vector3 position)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		float num = float.PositiveInfinity;
		INavigationTarget result = null;
		foreach (INavigationTarget target in targets)
		{
			float num2 = Vector3.SqrMagnitude(target.Transform.position - position);
			if (num2 < num)
			{
				num = num2;
				result = target;
			}
		}
		return result;
	}

	private static Vector2 CompressAlongDirection(Vector2 vector, Vector2 direction, float factor = 0.2f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Vector2 one = Vector2.one;
		if (direction.x != 0f)
		{
			one.x = factor;
		}
		if (direction.y != 0f)
		{
			one.y = factor;
		}
		return Vector2.Scale(vector, one);
	}
}
