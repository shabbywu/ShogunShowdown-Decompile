using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DamageNumberEffect : MonoBehaviour
{
	public float initialSpeed;

	public float angleSpread;

	private Rigidbody2D rb;

	private TextMeshProUGUI textTMPro;

	public string Text
	{
		set
		{
			((TMP_Text)textTMPro).text = value;
		}
	}

	private void Awake()
	{
		rb = ((Component)this).GetComponent<Rigidbody2D>();
		textTMPro = ((Component)this).GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		float num = MathF.PI / 180f * Random.Range((0f - angleSpread) / 2f, angleSpread / 2f);
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(Mathf.Sin(num), Mathf.Cos(num));
		rb.velocity = initialSpeed * val;
	}
}
