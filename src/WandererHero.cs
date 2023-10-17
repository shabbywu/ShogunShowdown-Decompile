using System.Collections.Generic;
using AgentEnums;
using TileEnums;
using UnlocksID;

public class WandererHero : Hero
{
	public override string TechnicalName { get; } = "Wanderer";


	public override HeroEnum HeroEnum { get; }

	public override UnlockID UnlockID => UnlockID.h_wanderer;

	public override List<AttackEnum[]> Decks
	{
		get
		{
			List<AttackEnum[]> list = new List<AttackEnum[]>();
			list.Add(new AttackEnum[2]
			{
				AttackEnum.swirl,
				AttackEnum.arrow
			});
			list.Add(new AttackEnum[2]
			{
				AttackEnum.lightning,
				AttackEnum.earthImpale
			});
			return list;
		}
	}
}
