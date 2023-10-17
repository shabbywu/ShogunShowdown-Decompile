using AgentEnums;
using UnityEngine;

public class EliteEnemyIcon : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Sprite shieldedSprite;

	[SerializeField]
	private Sprite doubleStriker;

	[SerializeField]
	private Sprite heavy;

	[SerializeField]
	private Sprite quickWitted;

	public void Initialize(EliteTypeEnum eliteType)
	{
		if (eliteType == EliteTypeEnum.none)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		((Component)spriteRenderer).gameObject.SetActive(true);
		switch (eliteType)
		{
		case EliteTypeEnum.none:
			spriteRenderer.sprite = null;
			break;
		case EliteTypeEnum.reactiveShield:
			spriteRenderer.sprite = shieldedSprite;
			break;
		case EliteTypeEnum.doubleStrike:
			spriteRenderer.sprite = doubleStriker;
			break;
		case EliteTypeEnum.heavy:
			spriteRenderer.sprite = heavy;
			break;
		case EliteTypeEnum.quickWitted:
			spriteRenderer.sprite = quickWitted;
			break;
		default:
			Debug.LogError((object)"Elite type not found");
			break;
		}
	}
}
