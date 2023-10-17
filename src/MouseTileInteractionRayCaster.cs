using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTileInteractionRayCaster : MonoBehaviour
{
	public LayerMask mask;

	private RaycastHit hit;

	private Ray ray;

	private MouseTileInteraction mouseTileInteraction;

	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		ray = mainCamera.ScreenPointToRay(new Vector3(((InputControl<float>)(object)((Pointer)Mouse.current).position.x).ReadValue(), ((InputControl<float>)(object)((Pointer)Mouse.current).position.y).ReadValue(), 0f));
		if (Physics.Raycast(ray, ref hit, 100f, LayerMask.op_Implicit(mask)))
		{
			mouseTileInteraction = ((Component)((RaycastHit)(ref hit)).transform).GetComponent<MouseTileInteraction>();
			if ((Object)(object)mouseTileInteraction != (Object)null)
			{
				mouseTileInteraction.PointerOnMe = true;
			}
		}
	}
}
