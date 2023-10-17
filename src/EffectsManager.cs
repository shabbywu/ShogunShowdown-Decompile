using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectsManager : MonoBehaviour
{
	[SerializeField]
	private ScreenShake screenShake;

	private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

	private float cannotPerformActionEffectCooldown;

	public static EffectsManager Instance { get; private set; }

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
		GameObject[] array = Array.ConvertAll(Resources.LoadAll("Effects"), (Converter<Object, GameObject>)((Object item) => (GameObject)item));
		foreach (GameObject val in array)
		{
			effects.Add(((Object)val).name, val);
		}
	}

	public void CannotPerformActionEffect()
	{
		if (!(cannotPerformActionEffectCooldown > 0f))
		{
			SoundEffectsManager.Instance.Play("CannotPerformAction");
			SmallScreenShake();
			((MonoBehaviour)this).StartCoroutine(LimitCannotPerformActionEffectCooldown());
		}
	}

	public void ScreenShake()
	{
		if (Globals.Options.screenShake)
		{
			screenShake.Enable();
		}
	}

	public void SmallScreenShake()
	{
		if (Globals.Options.screenShake)
		{
			screenShake.Enable(0.5f, 0.5f);
		}
	}

	public void HugeScreenShake()
	{
		if (Globals.Options.screenShake)
		{
			screenShake.Enable(15f, 0.5f);
		}
	}

	public void MediumGamepadRumble()
	{
		GamepadRumble(0.2f, 0.5f);
	}

	public void GamepadRumble(float seconds, float motorSpeed)
	{
		if (Globals.Options.controlScheme == Options.ControlScheme.Gamepad && Gamepad.current != null && Globals.Options.gamepadRumble)
		{
			((MonoBehaviour)this).StartCoroutine(RumbleCoroutine(seconds, motorSpeed));
		}
	}

	public void GenericSpawningEffect()
	{
		ScreenShake();
		SoundEffectsManager.Instance.Play("Spawn");
	}

	public IEnumerator SlowMotionEffect(float slowDownFactor = 0.3f, float durationSeconds = 0.35f)
	{
		Time.timeScale = slowDownFactor;
		yield return (object)new WaitForSeconds(durationSeconds);
		Time.timeScale = 1f;
	}

	public GameObject CreateInGameEffect(string effect, Transform parent, bool flipX = false)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(effects[effect], parent, flipX);
		if (flipX)
		{
			val.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		return val;
	}

	public GameObject CreateInGameEffect(string effect, Transform parent, Vector3 localPosition, bool flipX = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = CreateInGameEffect(effect, parent, flipX);
		obj.transform.localPosition = localPosition;
		return obj;
	}

	public GameObject CreateInGameEffect(string effect, Vector3 position, bool flipX = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(effects[effect], position, Quaternion.identity);
		if (flipX)
		{
			val.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		return val;
	}

	public void WaitAndCreateInGameEffect(string effect, Vector3 position, float wait, bool flipX = false)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StartCoroutine(_WaitAndCreateInGameEffect(effect, position, wait));
	}

	public void WaitAndCreateInGameEffect(string effect, Transform parent, float wait)
	{
		((MonoBehaviour)this).StartCoroutine(_WaitAndCreateInGameEffect(effect, parent, wait));
	}

	private IEnumerator _WaitAndCreateInGameEffect(string effect, Vector3 position, float wait)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		yield return (object)new WaitForSeconds(wait);
		CreateInGameEffect(effect, position);
	}

	private IEnumerator _WaitAndCreateInGameEffect(string effect, Transform parent, float wait)
	{
		yield return (object)new WaitForSeconds(wait);
		CreateInGameEffect(effect, parent);
	}

	private IEnumerator LimitCannotPerformActionEffectCooldown()
	{
		cannotPerformActionEffectCooldown = 0.25f;
		while (cannotPerformActionEffectCooldown > 0f)
		{
			cannotPerformActionEffectCooldown -= Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator RumbleCoroutine(float seconds, float motorSpeed)
	{
		Gamepad.current.SetMotorSpeeds(motorSpeed, motorSpeed);
		yield return (object)new WaitForSecondsRealtime(seconds);
		Gamepad.current.SetMotorSpeeds(0f, 0f);
	}
}
