using UnityEngine;

public class MetaProgressionUI : MonoBehaviour
{
	public GameObject canvasGO;

	public TabsGroup tabsGroup;

	private static float closedY = 5f;

	private static float smoothTime = 0.1f;

	private Vector3 canvasVelocity;

	private bool open;

	private Vector3 TargetCanvasPosition
	{
		get
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (open)
			{
				return Vector3.zero;
			}
			return new Vector3(0f, closedY, 0f);
		}
	}

	private void Update()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		canvasGO.transform.localPosition = Vector3.SmoothDamp(canvasGO.transform.localPosition, TargetCanvasPosition, ref canvasVelocity, smoothTime);
		if (!open && canvasGO.activeSelf && Vector3.SqrMagnitude(canvasGO.transform.localPosition - TargetCanvasPosition) < 0.1f)
		{
			canvasGO.SetActive(false);
		}
	}

	public void AgentEnters()
	{
		tabsGroup.Initialize();
		canvasGO.SetActive(true);
		open = true;
		EventsManager.Instance.ArchiveOpened.Invoke();
	}

	public void AgentExits()
	{
		open = false;
		EventsManager.Instance.ArchiveClosed.Invoke();
	}
}
