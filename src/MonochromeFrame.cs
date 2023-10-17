using UnityEngine;

public class MonochromeFrame : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private bool startOpen;

	private Color openColor;

	private Color closedColor = new Color(0f, 0f, 0f, 0f);

	public bool Open
	{
		set
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (value)
			{
				spriteRenderer.color = openColor;
			}
			else
			{
				spriteRenderer.color = closedColor;
			}
		}
	}

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		openColor = spriteRenderer.color;
		Open = startOpen;
	}
}
