using System;

[Serializable]
public class Options
{
	public enum HandSorting
	{
		cooldown,
		fixOrder
	}

	public enum ControlScheme
	{
		MouseAndKeyboard,
		Keyboard,
		Gamepad
	}

	public enum ControlSchemePreference
	{
		AutoDetect,
		MouseAndKeyboard,
		Gamepad
	}

	public enum Resolution
	{
		_1920x1080,
		_1440x810,
		_1280x720,
		_1280x800
	}

	public HandSorting handSorting;

	public bool invertedScroll;

	public bool mouseScrollDisabled;

	public int musicVolume;

	public int soundEffectsVolume;

	public ControlScheme controlScheme;

	public ControlSchemePreference controlSchemePreference;

	public bool gamepadRumble;

	public bool screenShake;

	public bool colorblindMode;

	public Resolution resolution;

	public Options()
	{
		controlSchemePreference = ControlSchemePreference.AutoDetect;
		controlScheme = ControlScheme.MouseAndKeyboard;
		gamepadRumble = true;
		invertedScroll = false;
		mouseScrollDisabled = false;
		handSorting = HandSorting.cooldown;
		musicVolume = 6;
		soundEffectsVolume = 6;
		screenShake = true;
		colorblindMode = false;
		resolution = Resolution._1920x1080;
	}
}
