using TMPro;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI bossName;

	[SerializeField]
	private SpriteRenderer barFill;

	[SerializeField]
	private SpriteRenderer barHitFill;

	private float width;

	private float height;

	private float timeSinceLastHealthUpdate;

	private Vector2 barFillVelocity;

	private bool initialized;

	public string Name
	{
		set
		{
			((TMP_Text)bossName).text = value;
		}
	}

	private void Update()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (timeSinceLastHealthUpdate < 0.25f)
		{
			timeSinceLastHealthUpdate += Time.deltaTime;
		}
		else
		{
			barHitFill.size = Vector2.SmoothDamp(barHitFill.size, barFill.size, ref barFillVelocity, 0.1f);
		}
	}

	public void UpdateHealth(int maxHP, int hp)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!initialized)
		{
			Initialize();
		}
		float num = (float)hp / (float)maxHP;
		barFill.size = new Vector2(num * width, height);
		timeSinceLastHealthUpdate = 0f;
	}

	private void Initialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		width = barFill.size.x;
		height = barFill.size.y;
		initialized = true;
	}
}
