using System.Collections.Generic;
using AgentEnums;
using TileEnums;
using UnlocksID;

public class JujitsukaHero : Hero
{
	public override string TechnicalName { get; } = "Jujitsuka";


	public override HeroEnum HeroEnum { get; } = HeroEnum.jujitsuka;


	public override UnlockID UnlockID => UnlockID.h_jujitsuka;

	public override List<AttackEnum[]> Decks
	{
		get
		{
			List<AttackEnum[]> list = new List<AttackEnum[]>();
			list.Add(new AttackEnum[2]
			{
				AttackEnum.dragonPunch,
				AttackEnum.scarStrike
			});
			list.Add(new AttackEnum[2]
			{
				AttackEnum.kaitenryuken,
				AttackEnum.dashBackward
			});
			return list;
		}
	}
}
