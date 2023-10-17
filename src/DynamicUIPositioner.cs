using UnityEngine;

public class DynamicUIPositioner : MonoBehaviour
{
	public enum PositioningMode
	{
		leftSideUIMediumShift_1280,
		rightSideUIMediumShift_1280,
		leftSideUISmallShift_1280,
		shopGoButton_gamepad
	}

	[SerializeField]
	private PositioningMode[] positioningModes;

	private void Start()
	{
		PositioningMode[] array = positioningModes;
		foreach (PositioningMode mode in array)
		{
			ProgressionPositioningMode(mode);
		}
	}

	private void ProgressionPositioningMode(PositioningMode mode)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		Resolution currentResolution;
		if (mode == PositioningMode.leftSideUIMediumShift_1280)
		{
			currentResolution = Screen.currentResolution;
			if (((Resolution)(ref currentResolution)).width == 1280)
			{
				Transform transform = ((Component)this).transform;
				transform.position += 0.5f * Vector3.right;
				return;
			}
		}
		if (mode == PositioningMode.rightSideUIMediumShift_1280)
		{
			currentResolution = Screen.currentResolution;
			if (((Resolution)(ref currentResolution)).width == 1280)
			{
				Transform transform2 = ((Component)this).transform;
				transform2.position += 0.5f * Vector3.left;
				return;
			}
		}
		if (mode == PositioningMode.leftSideUISmallShift_1280)
		{
			currentResolution = Screen.currentResolution;
			if (((Resolution)(ref currentResolution)).width == 1280)
			{
				Transform transform3 = ((Component)this).transform;
				transform3.position += 0.25f * Vector3.right;
				return;
			}
		}
		if (mode == PositioningMode.shopGoButton_gamepad && Globals.Options.controlScheme == Options.ControlScheme.Gamepad)
		{
			Transform transform4 = ((Component)this).transform;
			transform4.position += 0.75f * Vector3.left;
		}
	}
}
