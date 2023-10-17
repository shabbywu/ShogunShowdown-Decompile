using UnityEngine;

public class HealthBarUnit : MonoBehaviour
{
	private static Color fullColorStandard = new Color(0.27451f, 0.5098f, 0.19608f);

	private static Color emptyColorStandard = new Color(0.64706f, 0.18823f, 0.18823f);

	private static Color fullColorColorblind = new Color(1f, 0.76078f, 0.03921f);

	private static Color emptyColorColorblind = new Color(0.047058f, 0.48235f, 0.86274f);

	private static Color transitionColor = Color.white;

	private static Color fadedOutColor = new Color(0f, 0f, 0f, 0f);

	private static readonly float transitionTime = 0.75f;

	private static readonly float fadeoutTime = 0.5f;

	private int tweenId = -1;

	[SerializeField]
	private SpriteRenderer sprite;

	private bool full;

	private Color FullColor
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!Globals.Options.colorblindMode)
			{
				return fullColorStandard;
			}
			return fullColorColorblind;
		}
	}

	private Color EmptyColor
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!Globals.Options.colorblindMode)
			{
				return emptyColorStandard;
			}
			return emptyColorColorblind;
		}
	}

	public bool Full
	{
		set
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (full != value)
			{
				full = value;
				sprite.color = transitionColor;
				LeanTween.cancel(((Component)sprite).gameObject, tweenId, false);
				tweenId = LeanTween.color(((Component)sprite).gameObject, full ? FullColor : EmptyColor, transitionTime).id;
			}
		}
	}

	public void Initialize()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (full)
		{
			sprite.color = FullColor;
		}
		else
		{
			sprite.color = EmptyColor;
		}
	}

	public void FadeOut()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		LeanTween.cancel(((Component)sprite).gameObject, tweenId, false);
		tweenId = LeanTween.color(((Component)sprite).gameObject, fadedOutColor, fadeoutTime).id;
	}
}
