using System.Collections;
using TileEnums;
using UnityEngine;
using Utils;

public class SignatureMoveAttack : Attack
{
	public override AttackEnum AttackEnum => AttackEnum.signatureMove;

	public override string LocalizationTableKey { get; } = "SignatureMove";


	public override int InitialValue => -1;

	public override int InitialCooldown => 7;

	public override int[] Range { get; protected set; } = new int[0];


	public override string AnimationTrigger { get; protected set; } = "";


	public override AttackEffectEnum[] CompatibleEffects { get; protected set; } = new AttackEffectEnum[0];


	public override void Initialize(int maxLevel)
	{
		base.Initialize(maxLevel);
		base.TileEffect = TileEffectEnum.freePlay;
	}

	public override bool Begin(Agent attackingAgent)
	{
		base.Begin(attackingAgent);
		Hero hero = (Hero)attackingAgent;
		Dir dir = ((hero.SpecialMove.DefaultRelativeDir == RelativeDir.forward) ? hero.FacingDir : DirUtils.Opposite(hero.FacingDir));
		if (hero.SpecialMove.Allowed(hero, dir))
		{
			((MonoBehaviour)this).StartCoroutine(PerformAttack(hero, dir));
			return true;
		}
		if (hero.SpecialMove.CanDoBackwards && hero.SpecialMove.Allowed(hero, DirUtils.Opposite(dir)))
		{
			((MonoBehaviour)this).StartCoroutine(PerformAttack(hero, DirUtils.Opposite(dir)));
			return true;
		}
		return false;
	}

	private IEnumerator PerformAttack(Hero hero, Dir dir)
	{
		hero.AttackInProgress = true;
		yield return ((MonoBehaviour)this).StartCoroutine(hero.SpecialMove.Perform(hero, dir, depleteSpecialMoveCooldown: false));
		hero.AttackInProgress = false;
	}
}
