using System.Collections;
using InfoBoxUtils;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour, IInfoBoxable
{
	[SerializeField]
	private InfoBoxActivator infoBoxActivator;

	private string text;

	public string InfoBoxText => text;

	public bool InfoBoxEnabled => true;

	public BoxWidth BoxWidth => BoxWidth.auto;

	public void Open(string text, float openTime = 2f)
	{
		this.text = text;
		infoBoxActivator.Open();
		((MonoBehaviour)this).StartCoroutine(WaitCloseAndDestroySelf(openTime));
	}

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginningOfCombatTurn.AddListener(new UnityAction(OnBeginningOfCombatTurn));
	}

	private void OnDestroy()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.BeginningOfCombatTurn.RemoveListener(new UnityAction(OnBeginningOfCombatTurn));
	}

	private void OnBeginningOfCombatTurn()
	{
		if (infoBoxActivator.InfoBoxIsOpen)
		{
			((MonoBehaviour)this).StartCoroutine(CloseAndDestroySelf());
		}
	}

	private IEnumerator WaitCloseAndDestroySelf(float delay)
	{
		yield return (object)new WaitForSeconds(delay);
		yield return ((MonoBehaviour)this).StartCoroutine(CloseAndDestroySelf());
	}

	private IEnumerator CloseAndDestroySelf()
	{
		infoBoxActivator.Close();
		yield return (object)new WaitForSeconds(1f);
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
