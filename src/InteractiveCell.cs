using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveCell : Cell
{
	[SerializeField]
	private Transform cameraTarget;

	[SerializeField]
	private TextMeshProUGUI textUnderCell;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginRun.AddListener(new UnityAction(UponBeginRun));
	}

	public void AgentEnters(Agent agent)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StartCoroutine(CombatSceneManager.Instance.CameraFollow.ScrollsToX(cameraTarget.position.x));
	}

	public void AgentExits(Agent agent)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		CombatSceneManager.Instance.CameraFollow.EnableSmoothDampFollow(((Component)CombatSceneManager.Instance.Room).transform.position.x);
	}

	private void UponBeginRun()
	{
		((Component)textUnderCell).gameObject.SetActive(false);
	}
}
