using UnityEngine;

public class InputGlyphHelper : MonoBehaviour
{
	[SerializeField]
	private ButtonActionGlyph[] buttonActionGlyphs;

	public Sprite GetButtonSprite(InputActionButtonBinder.ButtonAction buttonAction)
	{
		ButtonActionGlyph[] array = buttonActionGlyphs;
		foreach (ButtonActionGlyph buttonActionGlyph in array)
		{
			if (buttonActionGlyph.buttonAction == buttonAction)
			{
				return buttonActionGlyph.sprite;
			}
		}
		return null;
	}
}
