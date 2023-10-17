using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Notifications : MonoBehaviour
{
	public TextMeshProUGUI text;

	public GameObject canvas;

	public float closedDeltaX;

	private List<string> notifications = new List<string>();

	private const float notificationTime = 7f;

	private const float speed = 20f;

	private Vector3 initialLocalPosition;

	private bool notificationInProgress;

	private Vector3 closedLocalPosition => initialLocalPosition + Vector3.right * closedDeltaX;

	private void Start()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		EventsManager.Instance.PushNotification.AddListener((UnityAction<string>)PushNotification);
		canvas.SetActive(false);
		initialLocalPosition = canvas.transform.localPosition;
		canvas.transform.localPosition = closedLocalPosition;
	}

	private void PushNotification(string message)
	{
		notifications.Add(message);
		if (!notificationInProgress)
		{
			((MonoBehaviour)this).StartCoroutine(ShowNotifications());
		}
	}

	private IEnumerator ShowNotifications()
	{
		notificationInProgress = true;
		while (notifications.Count != 0)
		{
			((TMP_Text)text).text = notifications[0];
			canvas.SetActive(true);
			yield return ((MonoBehaviour)this).StartCoroutine(MoveCanvasToPosition(initialLocalPosition));
			yield return (object)new WaitForSeconds(7f);
			yield return ((MonoBehaviour)this).StartCoroutine(MoveCanvasToPosition(closedLocalPosition));
			canvas.SetActive(false);
			notifications.RemoveAt(0);
		}
		notificationInProgress = false;
	}

	private IEnumerator MoveCanvasToPosition(Vector3 targetPosition)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		while (Vector3.SqrMagnitude(canvas.transform.localPosition - targetPosition) > 0.01f)
		{
			canvas.transform.localPosition = Vector3.MoveTowards(canvas.transform.localPosition, targetPosition, 20f * Time.unscaledDeltaTime);
			yield return null;
		}
	}
}
