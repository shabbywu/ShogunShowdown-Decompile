using ProgressionEnums;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShopEnvironment : Environment
{
	[Header("SpriteRenderer:")]
	public SpriteRenderer[] leftLayers;

	public SpriteRenderer[] skyLayers;

	[Header("Sprites:")]
	public Sprite[] skyGreen;

	public Sprite[] skyBrown;

	public Sprite[] skyRed;

	public Sprite[] skyPurple;

	public Sprite[] skyWhite;

	public Sprite[] skyGray;

	public Sprite[] skyShogun;

	private Light2D leftBoxLight2D;

	public void InitializeLeft(TileUpgradeInShop tileUpgradeInShop)
	{
		for (int i = 0; i < leftLayers.Length; i++)
		{
			leftLayers[i].sprite = tileUpgradeInShop.backgrounds[i];
		}
	}

	public void InitializeRight(IslandEnum island)
	{
		Sprite[] array;
		switch (island)
		{
		case IslandEnum.green:
		case IslandEnum.darkGreen:
			array = skyGreen;
			break;
		case IslandEnum.brown:
			array = skyBrown;
			break;
		case IslandEnum.red:
			array = skyRed;
			break;
		case IslandEnum.purple:
			array = skyPurple;
			break;
		case IslandEnum.white:
			array = skyWhite;
			break;
		case IslandEnum.gray:
			array = skyGray;
			break;
		case IslandEnum.shogun:
			array = skyShogun;
			break;
		default:
			Debug.LogWarning((object)"ShopEnvironment initialize right: did not find the id '{id}'. Fallback to green sky.");
			array = skyGreen;
			break;
		}
		for (int i = 0; i < leftLayers.Length; i++)
		{
			skyLayers[i].sprite = array[i];
		}
	}
}
