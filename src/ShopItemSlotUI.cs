using UnityEngine;

public class ShopItemSlotUI : MonoBehaviour
{
	public SpriteRenderer spirteRenderer;

	public Sprite locked;

	public Sprite empty;

	public void UpdateGraphics(bool unlocked)
	{
		if (unlocked)
		{
			spirteRenderer.sprite = empty;
		}
		else
		{
			spirteRenderer.sprite = locked;
		}
	}

	public void Open()
	{
		((Renderer)spirteRenderer).enabled = true;
	}

	public void Close()
	{
		((Renderer)spirteRenderer).enabled = false;
	}
}
