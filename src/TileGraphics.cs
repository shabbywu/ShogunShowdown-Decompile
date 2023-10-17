using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TileGraphics : MonoBehaviour
{
	public SpriteRenderer background;

	public SpriteRenderer symbol;

	public SpriteRenderer number;

	public SpriteRenderer dragShadow;

	public Light2D attackEffectSymbol;

	public Light2D attackEffectNumber;

	public CooldownGraphics cooldownGraphics;

	public TileLevelGraphics tileLevelGraphics;

	public Transform infoBoxTargetLow;

	public Transform infoBoxTargetMedium;

	public Transform infoBoxTargetHigh;

	private Tile Tile { get; set; }

	public void Initialize(Tile tile)
	{
		Tile = tile;
		cooldownGraphics = ((Component)this).GetComponentInChildren<CooldownGraphics>();
		UpdateGraphics();
	}

	public void ShowLevel(bool value)
	{
		tileLevelGraphics.SetVisible(value);
		if (value && Tile.Attack.MaxLevel > 4)
		{
			Tile.InfoBoxActivator.infoBoxTarget = infoBoxTargetHigh;
		}
		else if (value && Tile.Attack.MaxLevel <= 4)
		{
			Tile.InfoBoxActivator.infoBoxTarget = infoBoxTargetMedium;
		}
		else
		{
			Tile.InfoBoxActivator.infoBoxTarget = infoBoxTargetLow;
		}
	}

	public void ShowDragShadow(bool value)
	{
		((Renderer)dragShadow).enabled = value;
	}

	public void UpdateGraphics()
	{
		symbol.sprite = TilesFactory.Instance.Sprites.TileSymbolSprites[(Tile.Attack.AttackEnum, Tile.Attack.AttackEffect)];
		attackEffectSymbol.lightCookieSprite = symbol.sprite;
		background.sprite = TilesFactory.Instance.Sprites.TileBackgroundSprites[Tile.Attack.TileEffect];
		cooldownGraphics.Cooldown = Tile.Attack.Cooldown;
		cooldownGraphics.Charge = Tile.Attack.Cooldown;
		UpdateValueGraphics();
		tileLevelGraphics.UpdateGraphics(Tile.Attack.Level, Tile.Attack.MaxLevel);
	}

	public void UpdateValueGraphics()
	{
		if (Tile.Attack.HasValue)
		{
			number.sprite = TilesFactory.Instance.Sprites.TileNumbers[Tile.Attack.Value];
			attackEffectNumber.lightCookieSprite = number.sprite;
		}
	}
}
