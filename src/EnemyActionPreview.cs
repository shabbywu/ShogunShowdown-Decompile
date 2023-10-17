using System.Collections.Generic;
using CombatEnums;
using UnityEngine;

public class EnemyActionPreview : MonoBehaviour
{
	public GameObject moveLeft;

	public GameObject moveRight;

	public GameObject flipLeft;

	public GameObject flipRight;

	[SerializeField]
	private SpriteRenderer attackOrderSpriteRenderer;

	[SerializeField]
	private Sprite[] attackOrderSprites;

	private Dictionary<ActionEnum, GameObject> preview = new Dictionary<ActionEnum, GameObject>();

	private void Awake()
	{
		preview.Add(ActionEnum.moveLeft, moveLeft);
		preview.Add(ActionEnum.moveRight, moveRight);
		preview.Add(ActionEnum.flipLeft, flipLeft);
		preview.Add(ActionEnum.flipRight, flipRight);
	}

	public void PreviewAction(ActionEnum baseAction)
	{
		foreach (KeyValuePair<ActionEnum, GameObject> item in preview)
		{
			item.Value.SetActive(item.Key == baseAction);
		}
	}

	public void HidePreview()
	{
		foreach (KeyValuePair<ActionEnum, GameObject> item in preview)
		{
			item.Value.SetActive(false);
		}
	}

	public void ShowAttackOrder(int order)
	{
		((Component)attackOrderSpriteRenderer).gameObject.SetActive(true);
		attackOrderSpriteRenderer.sprite = attackOrderSprites[order];
	}

	public void HideAttackOrder()
	{
		((Component)attackOrderSpriteRenderer).gameObject.SetActive(false);
	}
}
