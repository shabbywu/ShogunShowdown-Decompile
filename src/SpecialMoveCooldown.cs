using Parameters;
using UnityEngine;
using UnityEngine.Events;

public class SpecialMoveCooldown : MonoBehaviour
{
	public SpriteRenderer frameSpriteRenderer;

	public SpriteRenderer highlightlFrameSpriteRenderer;

	private Animator animator;

	private bool justMoved;

	private CooldownGraphics cooldownGraphics;

	private int _cooldown;

	private int _charge;

	public bool IsCharged => _cooldown == _charge;

	public int Cooldown
	{
		get
		{
			return _cooldown;
		}
		set
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			_cooldown = value;
			cooldownGraphics.Cooldown = Cooldown;
			float num = (float)(3 * Cooldown + 7) * TechParams.pixelSize;
			frameSpriteRenderer.size = new Vector2(num, frameSpriteRenderer.size.y);
			highlightlFrameSpriteRenderer.size = new Vector2(num, highlightlFrameSpriteRenderer.size.y);
			((Component)frameSpriteRenderer).transform.localPosition = ((Component)cooldownGraphics).transform.localPosition;
			((Component)highlightlFrameSpriteRenderer).transform.localPosition = ((Component)cooldownGraphics).transform.localPosition;
			Charge = Cooldown;
		}
	}

	public int Charge
	{
		get
		{
			return _charge;
		}
		set
		{
			if (IsCharged && value < Cooldown)
			{
				animator.SetTrigger("Discharge");
			}
			_charge = value;
			cooldownGraphics.Charge = _charge;
		}
	}

	public void Initialize(int cooldown)
	{
		cooldownGraphics = ((Component)this).GetComponentInChildren<CooldownGraphics>();
		animator = ((Component)this).GetComponent<Animator>();
		Cooldown = cooldown;
	}

	public void EnterCombatMode()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		EventsManager.Instance.EndOfCombatTurn.AddListener(new UnityAction(EndOfCombatTurn));
	}

	public void FullyRecharge()
	{
		if (Charge < Cooldown)
		{
			Charge = Cooldown;
			animator.SetTrigger("Recharge");
		}
	}

	public void ExitCombatMode()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if ((Object)(object)EventsManager.Instance != (Object)null)
		{
			EventsManager.Instance.EndOfCombatTurn.RemoveListener(new UnityAction(EndOfCombatTurn));
		}
		FullyRecharge();
	}

	private void EndOfCombatTurn()
	{
		if (justMoved)
		{
			justMoved = false;
		}
		else if (Charge < Cooldown)
		{
			Charge++;
			if (Charge == Cooldown)
			{
				animator.SetTrigger("Recharge");
			}
		}
	}

	public void SpecialMoveActionPerformed()
	{
		Charge = 0;
		justMoved = true;
	}

	public void CannotPerformSpecialMoveEffect()
	{
		animator.SetTrigger("CannotPerform");
	}

	public void CreateRechargeEffect()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		EffectsManager.Instance.CreateInGameEffect("TileRechargeEffect", ((Component)this).transform.position + 0.25f * Vector3.up);
	}
}
