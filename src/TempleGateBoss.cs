using System.Collections.Generic;
using AgentEnums;
using CombatEnums;
using TileEnums;
using UnityEngine;
using Utils;

public class TempleGateBoss : Boss
{
	[SerializeField]
	private Enemy summonerPrefab;

	private Enemy summoner;

	private PseudoRandomWithMemory<AttackEnum> attacksGen;

	private List<AttackEnum> longRangeAttacks = new List<AttackEnum>
	{
		AttackEnum.arrow,
		AttackEnum.shuriken,
		AttackEnum.grapplingHook
	};

	private int attackCountdown;

	public override string TechnicalName { get; } = "StatueBoss";


	public override EnemyEnum EnemyEnum { get; } = EnemyEnum.temple_gate_boss;


	public override bool IsPurelyRangedEnemy { get; }

	protected override int DefaultInitialHP { get; } = 30;


	protected override int HigerInitialHP { get; } = 40;


	public override bool Movable => false;

	protected override EnemyTraitsEnum[] EnemyTraits { get; } = new EnemyTraitsEnum[1] { EnemyTraitsEnum.heavy };


	public override void Start()
	{
		base.Start();
		base.FacingDir = Dir.left;
		(AttackEnum, float)[] choicesAndBaseProbabilities = new(AttackEnum, float)[8]
		{
			(AttackEnum.arrow, 1f),
			(AttackEnum.shuriken, 1f),
			(AttackEnum.grapplingHook, 1f),
			(AttackEnum.sword, 1f),
			(AttackEnum.spear, 1f),
			(AttackEnum.shadowKama, 1f),
			(AttackEnum.earthImpale, 1f),
			(AttackEnum.tetsubo, 1f)
		};
		attacksGen = new PseudoRandomWithMemory<AttackEnum>(choicesAndBaseProbabilities);
	}

	public override void FirstTimeBossFightInitializations(Room room)
	{
		InstantiateSummoner(room);
	}

	private void InstantiateSummoner(Room room)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		summoner = Object.Instantiate<GameObject>(((Component)summonerPrefab).gameObject, ((Component)room).transform).GetComponent<Enemy>();
		Cell cell = room.Grid.Cells[0];
		((Component)summoner).transform.position = ((Component)cell).transform.position;
		summoner.Cell = cell;
		summoner.FacingDir = Dir.right;
		CombatManager.Instance.Enemies.Add(summoner);
	}

	protected override ActionEnum AIPickAction()
	{
		if (previousAction == ActionEnum.playTile)
		{
			return ActionEnum.wait;
		}
		if (previousAction == ActionEnum.attack)
		{
			return ActionEnum.wait;
		}
		if (base.AttackQueue.NTiles == 3)
		{
			if (attackCountdown <= 0)
			{
				return ActionEnum.attack;
			}
			attackCountdown--;
		}
		if (base.AttackQueue.NTiles < 3)
		{
			attackCountdown = 1;
			do
			{
				tileToPlay = attacksGen.GetNext();
			}
			while (base.AttackQueue.NTiles > 0 && longRangeAttacks.Contains(tileToPlay));
			return ActionEnum.playTile;
		}
		return ActionEnum.wait;
	}
}
