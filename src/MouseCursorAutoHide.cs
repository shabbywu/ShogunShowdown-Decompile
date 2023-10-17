using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MouseCursorAutoHide : MonoBehaviour
{
	[SerializeField]
	private PlayerInput playerInput;

	[SerializeField]
	private GameObject mouseRaycastBlocker;

	private float t;

	private static float hideAfterSeconds = 2f;

	private bool actionCallbackRegistered;

	private void Start()
	{
		EventsManager.Instance.ControlsSchemeUpdated.AddListener((UnityAction<Options.ControlScheme>)OnControlsSchemeUpdated);
		((Behaviour)this).enabled = false;
		if (Globals.Options.controlScheme == Options.ControlScheme.Gamepad)
		{
			SetCursorActive(value: false);
			RegisterOnMouseDeltaPerformed();
		}
	}

	private void OnDestroy()
	{
		DeregesterOnMouseDeltaPerformed();
	}

	private void OnMouseDeltaPerformed(CallbackContext context)
	{
		t = hideAfterSeconds;
		((Behaviour)this).enabled = true;
	}

	private void Update()
	{
		if (t > 0f)
		{
			SetCursorActive(value: true);
			t -= Time.unscaledDeltaTime;
		}
		else
		{
			SetCursorActive(value: false);
			((Behaviour)this).enabled = false;
		}
	}

	private void SetCursorActive(bool value)
	{
		Cursor.visible = value;
		if ((Object)(object)mouseRaycastBlocker != (Object)null)
		{
			mouseRaycastBlocker.SetActive(!value);
		}
		if ((Object)(object)InputManager.Instance != (Object)null)
		{
			if (InputManager.Instance.CurrentActionMapIsMenu)
			{
				InputManager.Instance.SetInGameMouseInteractionEnabled(value: false);
			}
			else
			{
				InputManager.Instance.SetInGameMouseInteractionEnabled(value);
			}
		}
	}

	private void OnControlsSchemeUpdated(Options.ControlScheme controlsScheme)
	{
		switch (controlsScheme)
		{
		case Options.ControlScheme.MouseAndKeyboard:
			SetCursorActive(value: true);
			DeregesterOnMouseDeltaPerformed();
			break;
		case Options.ControlScheme.Gamepad:
			SetCursorActive(value: false);
			RegisterOnMouseDeltaPerformed();
			break;
		}
		((Behaviour)this).enabled = false;
	}

	private void RegisterOnMouseDeltaPerformed()
	{
		if (!actionCallbackRegistered)
		{
			playerInput.actions["Menu/MouseDelta"].performed += OnMouseDeltaPerformed;
			playerInput.actions["Combat/MouseDelta"].performed += OnMouseDeltaPerformed;
			actionCallbackRegistered = true;
		}
	}

	private void DeregesterOnMouseDeltaPerformed()
	{
		if (actionCallbackRegistered)
		{
			playerInput.actions["Menu/MouseDelta"].performed -= OnMouseDeltaPerformed;
			playerInput.actions["Combat/MouseDelta"].performed -= OnMouseDeltaPerformed;
			actionCallbackRegistered = false;
		}
	}
}
