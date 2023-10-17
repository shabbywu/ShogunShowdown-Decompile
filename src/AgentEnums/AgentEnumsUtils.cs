using Utils;

namespace AgentEnums;

public static class AgentEnumsUtils
{
	public static string EliteDescription(EliteTypeEnum eliteType)
	{
		return eliteType switch
		{
			EliteTypeEnum.quickWitted => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_QuickWitted"), 
			EliteTypeEnum.heavy => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_Heavy"), 
			EliteTypeEnum.reactiveShield => LocalizationUtils.LocalizedString("Agents", "Elite_ReactiveShield"), 
			EliteTypeEnum.doubleStrike => LocalizationUtils.LocalizedString("Agents", "Elite_DoubleStrike"), 
			_ => "ERROR: enemy trait description not found", 
		};
	}

	public static string EnemyTraitDescription(EnemyTraitsEnum enemyTrait)
	{
		return enemyTrait switch
		{
			EnemyTraitsEnum.quickWitted => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_QuickWitted"), 
			EnemyTraitsEnum.unrelenting => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_Unrelenting"), 
			EnemyTraitsEnum.heavy => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_Heavy"), 
			EnemyTraitsEnum.unfreezable => LocalizationUtils.LocalizedString("Agents", "EnemyTrait_Unfreezable"), 
			_ => "ERROR: enemy trait description not found", 
		};
	}
}
