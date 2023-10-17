using System.Collections.Generic;
using AgentEnums;
using TileEnums;
using UnlocksID;

public class RoninHero : Hero
{
	public override string TechnicalName { get; } = "Ronin";


	public override HeroEnum HeroEnum { get; } = HeroEnum.ronin;


	public override UnlockID UnlockID => UnlockID.h_ronin;

	public override List<AttackEnum[]> Decks
	{
		get
		{
			List<AttackEnum[]> list = new List<AttackEnum[]>();
			list.Add(new AttackEnum[2]
			{
				AttackEnum.spear,
				AttackEnum.smokeBomb
			});
			list.Add(new AttackEnum[2]
			{
				AttackEnum.crossbow,
				AttackEnum.swapToss
			});
			return list;
		}
	}
}
