using UINavigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUINavigationManager : MonoBehaviour
{
	private AutorepeatNavigationHandler navigationAutorepeatHandler;

	public static MenuUINavigationManager Instance { get; private set; }

	private INavigationGroup CurrentGroup { get; set; }

	private bool ShouldProcessInput
	{
		get
		{
			if (NavigationEnabled)
			{
				return Application.isFocused;
			}
			return false;
		}
	}

	public bool NavigationEnabled { get; private set; }

	public Options.ControlScheme CurrentMenuControlScheme { get; set; }

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
		navigationAutorepeatHandler = ((Component)this).GetComponent<AutorepeatNavigationHandler>();
		navigationAutorepeatHandler.Initialize(ProcessNavigationInput);
		CurrentMenuControlScheme = Globals.Options.controlScheme;
	}

	public void OnControlsChanged(PlayerInput playerInput)
	{
		if (playerInput.currentControlScheme == "Keyboard&Mouse")
		{
			CurrentMenuControlScheme = Options.ControlScheme.MouseAndKeyboard;
			DisableMenuNavigation();
		}
		else if (playerInput.currentControlScheme == "Gamepad")
		{
			CurrentMenuControlScheme = Options.ControlScheme.Gamepad;
			EnableMenuNavigation();
		}
	}

	public void SetCurrentNavigationGroup(INavigationGroup navigationGroup)
	{
		CurrentGroup = navigationGroup;
	}

	public void OnSubmit(CallbackContext context)
	{
		if (ShouldProcessInput && ((CallbackContext)(ref context)).performed && CurrentGroup != null)
		{
			CurrentGroup.SubmitCurrentTarget();
		}
	}

	public void OnBack(CallbackContext context)
	{
		if (ShouldProcessInput && ((CallbackContext)(ref context)).performed && CurrentGroup is MenuPage menuPage && (Object)(object)menuPage.BackMenuItem != (Object)null)
		{
			menuPage.BackMenuItem.Submit();
		}
	}

	public void OnNavigate(CallbackContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (ShouldProcessInput)
		{
			navigationAutorepeatHandler.OnNavigate(context);
		}
	}

	private void ProcessNavigationInput(CallbackContext context)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (ShouldProcessInput && CurrentGroup != null)
		{
			int x = (int)((CallbackContext)(ref context)).ReadValue<Vector2>().x;
			int y = (int)((CallbackContext)(ref context)).ReadValue<Vector2>().y;
			CurrentGroup = CurrentGroup.Navigate(UINavigationHelper.DirectionFromXY(x, y));
		}
	}

	public void DisableMenuNavigation()
	{
		NavigationEnabled = false;
		CurrentGroup?.SelectedTarget?.Deselect();
	}

	public void EnableMenuNavigation()
	{
		NavigationEnabled = true;
		if (CurrentGroup is MenuPage menuPage)
		{
			menuPage.InitializeNavigation();
		}
	}
}
