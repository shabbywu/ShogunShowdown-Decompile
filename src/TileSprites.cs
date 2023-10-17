using System;
using System.Collections.Generic;
using TileEnums;
using UnityEngine;

public class TileSprites : MonoBehaviour
{
	private bool debug;

	private static string spritesResourcesPath = "Graphics/Tiles/Tiles";

	public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

	public Dictionary<(AttackEnum, AttackEffectEnum), Sprite> TileSymbolSprites { get; private set; }

	public Dictionary<TileEffectEnum, Sprite> TileBackgroundSprites { get; private set; }

	public Sprite[] TileNumbers { get; private set; }

	private void Awake()
	{
		Sprite[] array = Resources.LoadAll<Sprite>(spritesResourcesPath);
		foreach (Sprite val in array)
		{
			sprites.Add(((Object)val).name, val);
		}
		TileSymbolSprites = LoadTileSymbolSprites();
		TileNumbers = LoadTileNumbers();
		TileBackgroundSprites = LoadTileBackgroundSprites();
	}

	private Dictionary<(AttackEnum, AttackEffectEnum), Sprite> LoadTileSymbolSprites()
	{
		Dictionary<(AttackEnum, AttackEffectEnum), Sprite> dictionary = new Dictionary<(AttackEnum, AttackEffectEnum), Sprite>();
		foreach (AttackEnum value in Enum.GetValues(typeof(AttackEnum)))
		{
			foreach (AttackEffectEnum value2 in Enum.GetValues(typeof(AttackEffectEnum)))
			{
				string text = "Tiles_" + Enum.GetName(typeof(AttackEnum), value);
				if (value2 != 0)
				{
					text = text + "_" + Enum.GetName(typeof(AttackEffectEnum), value2);
				}
				if (debug)
				{
					Debug.Log((object)("TileSprites debug: loading sprite " + text));
				}
				if (sprites.ContainsKey(text))
				{
					dictionary.Add((value, value2), sprites[text]);
				}
			}
		}
		return dictionary;
	}

	private Sprite[] LoadTileNumbers()
	{
		Sprite[] array = (Sprite[])(object)new Sprite[10];
		for (int i = 0; i <= 9; i++)
		{
			array[i] = sprites[$"Tiles_{i}"];
		}
		return array;
	}

	private Dictionary<TileEffectEnum, Sprite> LoadTileBackgroundSprites()
	{
		Dictionary<TileEffectEnum, Sprite> dictionary = new Dictionary<TileEffectEnum, Sprite>();
		foreach (TileEffectEnum value in Enum.GetValues(typeof(TileEffectEnum)))
		{
			string text = "Tile";
			if (value != 0)
			{
				text = text + "_" + Enum.GetName(typeof(TileEffectEnum), value);
			}
			dictionary.Add(value, sprites[text]);
		}
		return dictionary;
	}
}
