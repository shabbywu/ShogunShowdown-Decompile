using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

public class RearrangeQueuePrompt : MonoBehaviour
{
	[SerializeField]
	private HeroAttackQueue heroAttackQueue;

	[SerializeField]
	private GameObject graphics;

	[SerializeField]
	private SpriteRenderer grabPrompt;

	[SerializeField]
	private SpriteRenderer upDownPrompt;

	[SerializeField]
	private Sprite grabNormalSprite;

	[SerializeField]
	private Sprite grabHighlightedSprite;

	private bool holdingTileGrabButton;

	private PlayerInput PlayerInput => InputManager.Instance.playerInput;

	private bool IsActive => Globals.Options.controlScheme == Options.ControlScheme.Gamepad;

	private void Start()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		InputManager.Instance?.RegisterActionToPlayerInput("Combat/TileGrab", InputManager.InputActionEnum.started, OnTileGrabStarted);
		InputManager.Instance?.RegisterActionToPlayerInput("Combat/TileGrab", InputManager.InputActionEnum.canceled, OnTileGrabCancelled);
		holdingTileGrabButton = false;
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.InfoModeEnabled.AddListener(new UnityAction(DisableRearrangeQueuePrompt));
		}
	}

	private void OnDestroy()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InputManager.Instance?.DeregisterActionToPlayerInput("Combat/TileGrab", InputManager.InputActionEnum.started, OnTileGrabStarted);
		InputManager.Instance?.DeregisterActionToPlayerInput("Combat/TileGrab", InputManager.InputActionEnum.canceled, OnTileGrabCancelled);
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.InfoModeEnabled.RemoveListener(new UnityAction(DisableRearrangeQueuePrompt));
		}
	}

	private void OnEnable()
	{
		graphics.SetActive(false);
	}

	public void UpdateState(bool visible, bool holdingGrab)
	{
		bool flag = IsActive && visible;
		graphics.SetActive(flag);
		if (flag)
		{
			((Renderer)upDownPrompt).enabled = holdingGrab;
			grabPrompt.sprite = (holdingGrab ? grabHighlightedSprite : grabNormalSprite);
		}
	}

	protected virtual void OnTileGrabStarted(CallbackContext context)
	{
		holdingTileGrabButton = true;
		UpdateRearrangeQueuePrompt();
	}

	protected virtual void OnTileGrabCancelled(CallbackContext context)
	{
		holdingTileGrabButton = false;
		UpdateRearrangeQueuePrompt();
	}

	public void UpdateRearrangeQueuePrompt()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (heroAttackQueue.TCC.NTiles <= 1 || heroAttackQueue.SelectedTarget == null)
		{
			DisableRearrangeQueuePrompt();
			return;
		}
		TileContainer tileContainer = (TileContainer)heroAttackQueue.SelectedTarget;
		MyMath.ModularizeIndex(heroAttackQueue.TCC.Containers.IndexOf(tileContainer), heroAttackQueue.NTiles);
		UpdateState(visible: true, holdingTileGrabButton);
		((Component)this).transform.SetParent(((Component)tileContainer.Tile).transform);
		((Component)this).transform.localPosition = Vector3.zero;
	}

	public void DisableRearrangeQueuePrompt()
	{
		UpdateState(visible: false, holdingGrab: false);
		((Component)this).transform.SetParent(((Component)heroAttackQueue).transform);
	}
}
