using AgentEnums;
using UnityEngine;

[RequireComponent(typeof(AgentGraphics))]
public class EliteEnemyMaterialSwapper : MonoBehaviour
{
	[SerializeField]
	private Material doubleStrike;

	[SerializeField]
	private Material heavy;

	[SerializeField]
	private Material quickWitted;

	[SerializeField]
	private Material reactiveShield;

	public void ReplaceAgentDefaultMaterial(EliteTypeEnum eliteType)
	{
		AgentGraphics component = ((Component)this).GetComponent<AgentGraphics>();
		switch (eliteType)
		{
		case EliteTypeEnum.doubleStrike:
			component.defaultLitMaterial = doubleStrike;
			break;
		case EliteTypeEnum.heavy:
			component.defaultLitMaterial = heavy;
			break;
		case EliteTypeEnum.quickWitted:
			component.defaultLitMaterial = quickWitted;
			break;
		case EliteTypeEnum.reactiveShield:
			component.defaultLitMaterial = reactiveShield;
			break;
		}
		component.SetMaterialForAgentSprite(component.defaultLitMaterial);
	}
}
