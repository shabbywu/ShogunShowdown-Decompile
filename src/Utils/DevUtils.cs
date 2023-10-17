using System.Linq;
using TileEnums;
using UnityEngine;

namespace Utils;

public static class DevUtils
{
	public static void SimulateUpgrades(int nRewards)
	{
		int[] source = new int[4] { 0, 3, 10, 14 };
		for (int i = 0; i < nRewards; i++)
		{
			if (source.Contains(i))
			{
				TilesManager.Instance.TakeTile(TilesFactory.Instance.Create(TilesFactory.Instance.PseudoRandomAttackEnumsGenerator.GetNext(), 3));
				continue;
			}
			Tile tile = MyRandom.NextFromArray(TilesManager.Instance.Deck.ToArray());
			GiveSimulatedUpgradeToTile(tile);
			UpdateTileAfterSimulatedUpgrade(tile);
		}
	}

	private static void GiveSimulatedUpgradeToTile(Tile tile)
	{
		bool flag = true;
		if (tile.Attack.Level == tile.Attack.MaxLevel)
		{
			if (tile.Attack.MaxLevel < Attack.maxMaxLevel)
			{
				tile.Attack.MaxLevel++;
			}
			if (flag)
			{
				Debug.Log((object)("SimUp: '" + tile.Attack.Name + "' MaxLevelUp"));
			}
			return;
		}
		string[] input = new string[3] { "damage", "cooldown", "effect" };
		float[] probabilities = new float[3] { 0.5f, 0.3f, 0.1f };
		string text = MyRandom.NextFromArray(input, probabilities);
		if (text == "cooldown" && tile.Attack.Cooldown == 0)
		{
			text = "damage";
		}
		if (text == "damage" && !tile.Attack.HasValue)
		{
			text = "cooldown";
		}
		switch (text)
		{
		case "damage":
			tile.Attack.Level++;
			tile.Attack.Value++;
			tile.Attack.Cooldown++;
			if (flag)
			{
				Debug.Log((object)("SimUp: '" + tile.Attack.Name + "' DMG+1,CD+1"));
			}
			break;
		case "cooldown":
			tile.Attack.Level++;
			tile.Attack.Cooldown -= 2;
			if (flag)
			{
				Debug.Log((object)("SimUp: '" + tile.Attack.Name + "' CD-2"));
			}
			break;
		case "effect":
		{
			AttackEffectEnum attackEffectEnum = MyRandom.NextEnum<AttackEffectEnum>();
			if (tile.Attack.CompatibleEffects.Contains(attackEffectEnum))
			{
				tile.Attack.AttackEffect = attackEffectEnum;
				tile.Attack.Level++;
				tile.Attack.Cooldown++;
				if (flag)
				{
					Debug.Log((object)("SimUp: '" + tile.Attack.Name + "' Effect,CD+1"));
				}
			}
			break;
		}
		}
	}

	private static void UpdateTileAfterSimulatedUpgrade(Tile tile)
	{
		tile.CooldownCharge = tile.Attack.Cooldown;
		tile.Graphics.UpdateGraphics();
	}
}
