using System;
using System.Collections;
using Parameters;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutorepeatNavigationHandler : MonoBehaviour
{
	private Action<CallbackContext> onNavigateAction;

	private Vector2 previousNavigationInput;

	private IEnumerator autoRepeatCoroutine;

	public void Initialize(Action<CallbackContext> onNavigateAction)
	{
		this.onNavigateAction = onNavigateAction;
	}

	public void OnNavigate(CallbackContext context)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (((CallbackContext)(ref context)).canceled)
		{
			previousNavigationInput = Vector2.zero;
			NavigateCanceled(context);
		}
		else if (((CallbackContext)(ref context)).performed && !(((CallbackContext)(ref context)).ReadValue<Vector2>() == previousNavigationInput))
		{
			previousNavigationInput = ((CallbackContext)(ref context)).ReadValue<Vector2>();
			onNavigateAction(context);
			NavigateStarted(context);
		}
	}

	private void NavigateStarted(CallbackContext context)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (autoRepeatCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(autoRepeatCoroutine);
			autoRepeatCoroutine = null;
		}
		autoRepeatCoroutine = AutoRepeatCoroutine(context, onNavigateAction);
		((MonoBehaviour)this).StartCoroutine(autoRepeatCoroutine);
	}

	private void NavigateCanceled(CallbackContext context)
	{
		if (autoRepeatCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(autoRepeatCoroutine);
			autoRepeatCoroutine = null;
		}
	}

	private IEnumerator AutoRepeatCoroutine(CallbackContext context, Action<CallbackContext> action)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		yield return (object)new WaitForSecondsRealtime(GameParams.autoRepeatDelay);
		while (true)
		{
			action(context);
			yield return (object)new WaitForSecondsRealtime(GameParams.autoRepeatRate);
		}
	}
}
