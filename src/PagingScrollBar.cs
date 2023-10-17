using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PagingScrollBar : MonoBehaviour
{
	[SerializeField]
	private GameObject layoutGroupGO;

	[SerializeField]
	private RectTransform dotsContainer;

	[SerializeField]
	private GameObject dotLocalPrefab;

	[SerializeField]
	private Sprite selectedPageSprite;

	[SerializeField]
	private Sprite unselectedPageSprite;

	private MetaProgressionUIPage metaProgressionUIPage;

	private Image[] dotsSpriteRenderer;

	private void Awake()
	{
		metaProgressionUIPage = ((Component)this).GetComponentInParent<MetaProgressionUIPage>();
		if ((Object)(object)metaProgressionUIPage == (Object)null)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		InstantiateDotsBasedOnNumberOfPages();
		((MonoBehaviour)this).StartCoroutine(LayoutUpdateWorkaround());
	}

	private void InstantiateDotsBasedOnNumberOfPages()
	{
		for (int i = 0; i < metaProgressionUIPage.NumberOfPages - 1; i++)
		{
			Object.Instantiate<GameObject>(dotLocalPrefab, (Transform)(object)dotsContainer);
		}
		dotsSpriteRenderer = ((Component)dotsContainer).GetComponentsInChildren<Image>();
	}

	public void NextPage()
	{
		metaProgressionUIPage.NextPage();
		UpdatePagingScrollBar();
	}

	public void PreviousPage()
	{
		metaProgressionUIPage.PreviousPage();
		UpdatePagingScrollBar();
	}

	private void OnEnable()
	{
		UpdatePagingScrollBar();
	}

	public void UpdatePagingScrollBar()
	{
		((Component)this).gameObject.SetActive(metaProgressionUIPage.NumberOfPages > 0);
		for (int i = 0; i < metaProgressionUIPage.NumberOfPages; i++)
		{
			if (i == metaProgressionUIPage.CurrentPage)
			{
				dotsSpriteRenderer[i].sprite = selectedPageSprite;
			}
			else
			{
				dotsSpriteRenderer[i].sprite = unselectedPageSprite;
			}
		}
	}

	private IEnumerator LayoutUpdateWorkaround()
	{
		yield return (object)new WaitForEndOfFrame();
		layoutGroupGO.gameObject.SetActive(false);
		yield return (object)new WaitForEndOfFrame();
		layoutGroupGO.gameObject.SetActive(true);
	}
}
