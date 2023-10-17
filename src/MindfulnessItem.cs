using SkillEnums;
using UnityEngine.Events;

public class MindfulnessItem : Item
{
	public override SkillEnum SkillEnum { get; } = SkillEnum.mindfulness;


	public override int MaxLevel { get; protected set; } = 1;


	public override string LocalizationTableKey { get; } = "Mindfulness";


	public override void PickUp()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		base.PickUp();
		EventsManager.Instance.HeroWaited.AddListener(new UnityAction(UponHeroWaited));
	}

	public override void Remove()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		base.Remove();
		EventsManager.Instance.HeroWaited.RemoveListener(new UnityAction(UponHeroWaited));
	}

	private void UponHeroWaited()
	{
		Tile[] tiles = TilesManager.Instance.hand.TCC.Tiles;
		foreach (Tile tile in tiles)
		{
			if (!tile.FullyCharged)
			{
				tile.EndOfTurnCooldownRecharge++;
			}
		}
		if (!Globals.Hero.SpecialMove.Cooldown.IsCharged)
		{
			Globals.Hero.SpecialMove.Cooldown.Charge++;
		}
	}
}
