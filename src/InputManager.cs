using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public enum InputActionEnum
	{
		started,
		performed,
		canceled
	}

	public PlayerInput playerInput;

	public PhysicsRaycaster cameraPhysicsRaycaster;

	private MouseTileInteractionRayCaster mouseTileInteractionRayCaster;

	public List<(string, InputActionEnum, Action<CallbackContext>)> registeredActionCallbacks;

	public static InputManager Instance { get; private set; }

	public bool CurrentActionMapIsMenu => playerInput.currentActionMap.name == "Menu";

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
		mouseTileInteractionRayCaster = ((Component)this).GetComponent<MouseTileInteractionRayCaster>();
		registeredActionCallbacks = new List<(string, InputActionEnum, Action<CallbackContext>)>();
	}

	public void SwitchToCombatMode()
	{
		playerInput.SwitchCurrentActionMap("Combat");
		SetInGameMouseInteractionEnabled(value: true);
	}

	public void SwitchToMenuMode()
	{
		playerInput.SwitchCurrentActionMap("Menu");
		SetInGameMouseInteractionEnabled(value: false);
	}

	public void SetInGameMouseInteractionEnabled(bool value)
	{
		((Behaviour)cameraPhysicsRaycaster).enabled = value;
		((Behaviour)mouseTileInteractionRayCaster).enabled = value;
	}

	public void RegisterActionToPlayerInput(string actionName, InputActionEnum inputActionEnum, Action<CallbackContext> action)
	{
		switch (inputActionEnum)
		{
		case InputActionEnum.started:
			playerInput.actions.FindAction(actionName, false).started += action;
			break;
		case InputActionEnum.performed:
			playerInput.actions.FindAction(actionName, false).performed += action;
			break;
		case InputActionEnum.canceled:
			playerInput.actions.FindAction(actionName, false).canceled += action;
			break;
		}
		registeredActionCallbacks.Add((actionName, inputActionEnum, action));
	}

	public void DeregisterActionToPlayerInput(string actionName, InputActionEnum inputActionEnum, Action<CallbackContext> action)
	{
		int num = -1;
		for (int i = 0; i < registeredActionCallbacks.Count; i++)
		{
			var (text, inputActionEnum2, action2) = registeredActionCallbacks[i];
			if (text == actionName && inputActionEnum2 == inputActionEnum && action2 == action)
			{
				num = i;
				break;
			}
		}
		if (num != -1 && !((Object)(object)playerInput == (Object)null))
		{
			registeredActionCallbacks.RemoveAt(num);
			switch (inputActionEnum)
			{
			case InputActionEnum.started:
				playerInput.actions.FindAction(actionName, false).started -= action;
				break;
			case InputActionEnum.performed:
				playerInput.actions.FindAction(actionName, false).performed -= action;
				break;
			case InputActionEnum.canceled:
				playerInput.actions.FindAction(actionName, false).canceled -= action;
				break;
			}
		}
	}

	private void OnDestroy()
	{
		int count = registeredActionCallbacks.Count;
		for (int i = 0; i < count; i++)
		{
			var (actionName, inputActionEnum, action) = registeredActionCallbacks[0];
			DeregisterActionToPlayerInput(actionName, inputActionEnum, action);
		}
	}
}
