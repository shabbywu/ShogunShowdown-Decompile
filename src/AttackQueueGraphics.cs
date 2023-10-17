using System;
using Parameters;
using UnityEngine;

public class AttackQueueGraphics : MonoBehaviour
{
	public SpriteRenderer body;

	public SpriteRenderer blinkingAttackBody;

	private readonly float verticalOscillationAmplitude = TechParams.pixelSize;

	private readonly float verticalOscillationOmega = MathF.PI * 2f / 3f;

	private readonly float blinkingAttackPeriod = 0.3f;

	private readonly float shakeSmothTime = 0.05f;

	private readonly float shakeRandomPositionUpdateTime = 0.12f;

	private int[] shakeSequenceX = new int[6] { 0, 1, -1, 1, -1, 0 };

	private int[] shakeSequenceY = new int[6] { 1, -1, 0, 0, 0, -1 };

	private Animator animator;

	private bool aboutToAttack;

	private float blinkTime;

	private float shakeTime;

	private Vector3 velocity;

	private Vector3 targetPosition;

	private int iShake;

	private int NContainers { get; set; }

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	private void Update()
	{
		if (aboutToAttack)
		{
			AboutToAttackAnimation();
		}
		else
		{
			IdleOscillation();
		}
	}

	public void SetNumberOfContainers(int nActiveContainers)
	{
		animator.SetInteger("NActive", nActiveContainers);
		NContainers = nActiveContainers;
		if (nActiveContainers == 0)
		{
			aboutToAttack = false;
			((Renderer)blinkingAttackBody).enabled = false;
		}
	}

	public void AboutToAttack()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		blinkTime = blinkingAttackPeriod;
		aboutToAttack = true;
		((Renderer)blinkingAttackBody).enabled = true;
		blinkingAttackBody.size = body.size;
	}

	public void Idle()
	{
		aboutToAttack = false;
		((Renderer)blinkingAttackBody).enabled = false;
	}

	private void IdleOscillation()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = new Vector3(0f, verticalOscillationAmplitude * Mathf.Sin(verticalOscillationOmega * Time.time), 0f);
	}

	private void AboutToAttackAnimation()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		blinkTime -= Time.deltaTime;
		if (blinkTime <= 0f)
		{
			blinkTime = blinkingAttackPeriod;
			((Renderer)blinkingAttackBody).enabled = !((Renderer)blinkingAttackBody).enabled;
		}
		shakeTime -= Time.deltaTime;
		if (shakeTime <= 0f)
		{
			targetPosition = NextRandomPosition();
			shakeTime = shakeRandomPositionUpdateTime;
		}
		((Component)this).transform.localPosition = Vector3.SmoothDamp(((Component)this).transform.localPosition, targetPosition, ref velocity, shakeSmothTime);
		blinkingAttackBody.size = body.size;
	}

	private Vector3 NextRandomPosition()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		iShake = (iShake + 1) % shakeSequenceX.Length;
		return new Vector3((float)shakeSequenceX[iShake] * TechParams.pixelSize, (float)shakeSequenceY[iShake] * TechParams.pixelSize, 0f);
	}
}
