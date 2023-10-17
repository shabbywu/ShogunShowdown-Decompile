using Parameters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionButtonBinder : MonoBehaviour
{
	public enum ButtonAction
	{
		confirm,
		back,
		altAction1,
		altAction2,
		prev,
		next
	}

	public enum ButtonMode
	{
		tap,
		hold
	}

	[SerializeField]
	private MyButton button;

	[SerializeField]
	private ButtonAction inputAction;

	[SerializeField]
	private ButtonMode mode;

	[SerializeField]
	private Image glyphImage;

	[SerializeField]
	private Image holdHighlightImage;

	[SerializeField]
	private InputGlyphHelper inputGlyphHelper;

	private bool? prevInteractable;

	protected float currentHoldTime;

	private bool OnPerformedAlreadyCalled;

	private bool inputActionsRegistered;

	protected bool Interactable => button.Interactable;

	protected bool Holding { get; set; }

	protected bool Enabled => Globals.Options.controlScheme != Options.ControlScheme.MouseAndKeyboard;

	private string InputActionName => inputAction switch
	{
		ButtonAction.confirm => "Combat/Confirm", 
		ButtonAction.back => "Combat/Back", 
		ButtonAction.altAction1 => "Combat/AltAction1", 
		ButtonAction.altAction2 => "Combat/AltAction2", 
		ButtonAction.prev => "Combat/Prev", 
		ButtonAction.next => "Combat/Next", 
		_ => "", 
	};

	protected void Awake()
	{
		((Component)this).gameObject.SetActive(Enabled);
		if (Enabled)
		{
			glyphImage.sprite = inputGlyphHelper.GetButtonSprite(inputAction);
		}
	}

	private void Start()
	{
		EventsManager.Instance.InputActionButtonBindersEnabled.AddListener((UnityAction<bool>)InputActionButtonBindersEnabled);
	}

	~InputActionButtonBinder()
	{
		try
		{
			EventsManager.Instance.InputActionButtonBindersEnabled.RemoveListener((UnityAction<bool>)InputActionButtonBindersEnabled);
		}
		finally
		{
			((object)this).Finalize();
		}
	}

	protected void Update()
	{
		if (mode == ButtonMode.hold)
		{
			UpdateHold();
		}
		if (prevInteractable != Interactable)
		{
			UpdateGraphics();
			prevInteractable = Interactable;
		}
	}

	private void UpdateHold()
	{
		if (Holding)
		{
			if (Interactable)
			{
				currentHoldTime += Time.deltaTime;
			}
			currentHoldTime = Mathf.Clamp(currentHoldTime, 0f, GameParams.holdButtonTime);
			holdHighlightImage.fillAmount = currentHoldTime / GameParams.holdButtonTime;
			if (Mathf.Approximately(currentHoldTime, GameParams.holdButtonTime) && !OnPerformedAlreadyCalled)
			{
				OnPerformedLogic();
				ResetHolding();
				OnPerformedAlreadyCalled = true;
			}
		}
	}

	protected virtual void OnPerformedLogic()
	{
		if (button.Interactable)
		{
			button.Click();
		}
	}

	private void OnPerformed(CallbackContext context)
	{
		OnPerformedLogic();
	}

	protected virtual void OnStarted(CallbackContext context)
	{
		Holding = true;
		button.TriggerOnPointerEnter();
	}

	protected virtual void OnCancelled(CallbackContext context)
	{
		ResetHolding();
		OnPerformedAlreadyCalled = false;
		button.TriggerOnPointerExit();
	}

	private void ResetHolding()
	{
		Holding = false;
		currentHoldTime = 0f;
		holdHighlightImage.fillAmount = 0f;
	}

	private void OnEnable()
	{
		Holding = false;
		RegisterInputActions();
	}

	private void OnDisable()
	{
		Holding = false;
		DeregisterInputActions();
	}

	private void InputActionButtonBindersEnabled(bool value)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Holding && !value)
		{
			OnCancelled(default(CallbackContext));
		}
		((Behaviour)this).enabled = value;
	}

	private void RegisterInputActions()
	{
		if (!inputActionsRegistered)
		{
			if (mode == ButtonMode.tap)
			{
				InputManager.Instance?.RegisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.performed, OnPerformed);
			}
			if (mode == ButtonMode.hold)
			{
				InputManager.Instance?.RegisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.started, OnStarted);
				InputManager.Instance?.RegisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.canceled, OnCancelled);
			}
			inputActionsRegistered = true;
		}
	}

	private void DeregisterInputActions()
	{
		if (inputActionsRegistered)
		{
			if (mode == ButtonMode.tap)
			{
				InputManager.Instance?.DeregisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.performed, OnPerformed);
			}
			if (mode == ButtonMode.hold)
			{
				InputManager.Instance?.DeregisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.started, OnStarted);
				InputManager.Instance?.DeregisterActionToPlayerInput(InputActionName, InputManager.InputActionEnum.canceled, OnCancelled);
			}
			inputActionsRegistered = false;
		}
	}

	protected void UpdateGraphics()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (Interactable)
		{
			((Graphic)glyphImage).color = Color.white;
		}
		else
		{
			((Graphic)glyphImage).color = Color.gray;
		}
	}
}
