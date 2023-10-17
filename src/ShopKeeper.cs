using System.Collections;
using InfoBoxUtils;
using UnityEngine;
using Utils;

public class ShopKeeper : MonoBehaviour, IInfoBoxable
{
	[SerializeField]
	private Transform consumableAppearenceTransform;

	[SerializeField]
	private string[] giftLocalizationStringsKeys;

	[SerializeField]
	private string allSoldLocalizationKey;

	private InfoBoxActivator infoBoxActivator;

	public Transform ConsumableAppearanceTransform => consumableAppearenceTransform;

	public string AllSoldText => LocalizationUtils.LocalizedString("ShopAndNPC", allSoldLocalizationKey);

	public string InfoBoxText { get; protected set; }

	public bool InfoBoxEnabled => false;

	public BoxWidth BoxWidth => BoxWidth.auto;

	protected virtual void Awake()
	{
		infoBoxActivator = ((Component)this).GetComponentInChildren<InfoBoxActivator>();
	}

	public virtual void BeginInteraction(string text)
	{
		InfoBoxText = text;
		infoBoxActivator.Open();
	}

	public virtual void BeginInteraction(string text, float time)
	{
		BeginInteraction(text);
		((MonoBehaviour)this).StartCoroutine(WaitAndEndInteraction(time));
	}

	public virtual void EndInteraction()
	{
		infoBoxActivator.Close();
	}

	private IEnumerator WaitAndEndInteraction(float time)
	{
		yield return (object)new WaitForSeconds(time);
		EndInteraction();
	}

	public string GetRandomTextForPotionGift()
	{
		return LocalizationUtils.LocalizedString("ShopAndNPC", MyRandom.NextRandomUniform(giftLocalizationStringsKeys));
	}
}
