using System;
using System.Collections;
using Parameters;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutorepeatPrevNextHandler : MonoBehaviour
{
	private enum Dir
	{
		prev,
		next
	}

	private Action onPrevAction;

	private Action onNextAction;

	private IEnumerator autoRepeatCoroutine;

	public void Initialize(Action onPrevAction, Action onNextAction)
	{
		this.onPrevAction = onPrevAction;
		this.onNextAction = onNextAction;
	}

	public void OnPrev(CallbackContext context)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		OnPrevOrNext(context, Dir.prev);
	}

	public void OnNext(CallbackContext context)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		OnPrevOrNext(context, Dir.next);
	}

	private void OnPrevOrNext(CallbackContext context, Dir dir)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (((CallbackContext)(ref context)).canceled)
		{
			Cancelled(context);
		}
		else if (((CallbackContext)(ref context)).performed)
		{
			if (dir == Dir.prev)
			{
				onPrevAction();
			}
			else
			{
				onNextAction();
			}
			PrevOrNextNavigationStarted(dir);
		}
	}

	private void PrevOrNextNavigationStarted(Dir dir)
	{
		if (autoRepeatCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(autoRepeatCoroutine);
			autoRepeatCoroutine = null;
		}
		if (dir == Dir.prev)
		{
			autoRepeatCoroutine = AutoRepeatCoroutine(onPrevAction);
		}
		else
		{
			autoRepeatCoroutine = AutoRepeatCoroutine(onNextAction);
		}
		((MonoBehaviour)this).StartCoroutine(autoRepeatCoroutine);
	}

	private void Cancelled(CallbackContext context)
	{
		if (autoRepeatCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(autoRepeatCoroutine);
			autoRepeatCoroutine = null;
		}
	}

	private IEnumerator AutoRepeatCoroutine(Action action)
	{
		yield return (object)new WaitForSecondsRealtime(GameParams.autoRepeatDelay);
		while (true)
		{
			action();
			yield return (object)new WaitForSecondsRealtime(GameParams.autoRepeatRate);
		}
	}
}
