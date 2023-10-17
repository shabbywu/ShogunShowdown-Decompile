using CombatEnums;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class CombatInput : MonoBehaviour
{
	private bool attack;

	private bool wait;

	private bool move;

	private bool potentiallyAttack;

	private bool scrollWheelMove;

	private Dir moveDir;

	private bool leftMouse;

	private bool rightMouse;

	private bool leftStickReadyForNewInput;

	private void LateUpdate()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (wait)
		{
			OnWait();
		}
		else if (attack)
		{
			OnAttack();
		}
		if (move && !leftMouse && !rightMouse)
		{
			if (moveDir == Dir.right)
			{
				CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.moveRight, !scrollWheelMove);
			}
			else if (moveDir == Dir.left)
			{
				CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.moveLeft, !scrollWheelMove);
			}
		}
		if (!leftStickReadyForNewInput)
		{
			int num;
			if (Gamepad.current != null)
			{
				Vector2 val = ((InputControl<Vector2>)(object)Gamepad.current.leftStick).ReadValue();
				num = ((((Vector2)(ref val)).magnitude < 0.2f) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			leftStickReadyForNewInput = (byte)num != 0;
		}
		wait = false;
		attack = false;
		move = false;
		leftMouse = false;
		rightMouse = false;
		scrollWheelMove = false;
	}

	private bool LeftStickInputAttempt()
	{
		if (leftStickReadyForNewInput)
		{
			leftStickReadyForNewInput = false;
			return true;
		}
		return false;
	}

	public void OnMoveLeft(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && LeftStickInputAttempt())
		{
			CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.moveLeft);
		}
	}

	public void OnMoveRight(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && LeftStickInputAttempt())
		{
			CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.moveRight);
		}
	}

	public void OnMove(CallbackContext context)
	{
		if (Globals.Options.mouseScrollDisabled)
		{
			return;
		}
		if (((CallbackContext)(ref context)).ReadValue<float>() > 0.1f)
		{
			move = true;
			scrollWheelMove = true;
			if (Globals.Hero.FacingDir == Dir.right)
			{
				moveDir = Dir.right;
			}
			else
			{
				moveDir = Dir.left;
			}
		}
		else if (((CallbackContext)(ref context)).ReadValue<float>() < -0.1f)
		{
			move = true;
			scrollWheelMove = true;
			if (Globals.Hero.FacingDir == Dir.right)
			{
				moveDir = Dir.left;
			}
			else
			{
				moveDir = Dir.right;
			}
		}
		if (Globals.Options.invertedScroll)
		{
			moveDir = DirUtils.Opposite(moveDir);
		}
	}

	public void OnTileInteract(CallbackContext context)
	{
		leftMouse = true;
	}

	public void OnFlip(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && LeftStickInputAttempt())
		{
			if (Globals.Hero.FacingDir == Dir.right)
			{
				CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.flipLeft);
			}
			else
			{
				CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.flipRight);
			}
		}
	}

	public void OnAttack(CallbackContext context)
	{
		if (Globals.InCamp)
		{
			return;
		}
		if (((CallbackContext)(ref context)).control.device == Gamepad.current && ((CallbackContext)(ref context)).performed)
		{
			OnAttack();
			return;
		}
		rightMouse = true;
		if (((CallbackContext)(ref context)).performed)
		{
			potentiallyAttack = true;
		}
		if (((CallbackContext)(ref context)).canceled)
		{
			attack = potentiallyAttack;
			potentiallyAttack = false;
		}
	}

	public void OnAttack()
	{
		CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.attack);
	}

	public void OnWait()
	{
		CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.wait);
	}

	public void OnWait(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).control.device == Gamepad.current)
		{
			if (((CallbackContext)(ref context)).performed && LeftStickInputAttempt())
			{
				CombatManager.Instance.PlayerInputsCombatAction(ActionEnum.wait);
			}
			return;
		}
		if (((CallbackContext)(ref context)).performed)
		{
			potentiallyAttack = false;
		}
		if (((CallbackContext)(ref context)).canceled)
		{
			wait = true;
			potentiallyAttack = false;
		}
	}

	public void OnEscapeButton(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed)
		{
			CombatSceneManager.Instance.TogglePause();
		}
	}

	public void OnInfoModeToggle(CallbackContext context)
	{
		if (CombatSceneManager.Instance.CanEnableInfoMode && ((CallbackContext)(ref context)).performed)
		{
			if (Globals.FullInfoMode)
			{
				CombatSceneManager.Instance.DisableInfoMode();
			}
			else
			{
				CombatSceneManager.Instance.EnableInfoMode();
			}
		}
	}

	public void OnMapToggle(CallbackContext context)
	{
		if (((CallbackContext)(ref context)).performed && !Globals.InCamp && !Globals.Tutorial)
		{
			MapManager.Instance.OpenOrCloseMap();
		}
	}
}
