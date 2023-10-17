using UnityEngine;

public class SwapSpriteDebug : MonoBehaviour
{
	public Sprite[] sprites;

	private int iSprite;

	public void SwapSprite()
	{
		iSprite++;
		if (iSprite >= sprites.Length)
		{
			iSprite = 0;
		}
		GrapplingEnemy[] array = Object.FindObjectsOfType<GrapplingEnemy>();
		for (int i = 0; i < array.Length; i++)
		{
			((Component)array[i].AgentGraphics.AgentSpriteTransform).GetComponent<SpriteRenderer>().sprite = sprites[iSprite];
		}
	}
}
