using System.Collections;
using UnityEngine;

public class AgentGraphics : MonoBehaviour
{
	public Material solidColorMaterial;

	public Material defaultLitMaterial;

	private static float hitReceivedEffectTime = 0.2f;

	private SpriteRenderer agentSpriteRend;

	private IEnumerator hitReceivedEffectCoroutine;

	public Transform AgentSpriteTransform { get; private set; }

	public Transform AnimationFollowingTransform { get; private set; }

	public TrailRenderer TrailRenderer { get; private set; }

	private void Awake()
	{
		AgentSpriteTransform = ((Component)this).gameObject.transform.Find("Sprites/AgentSprite");
		AnimationFollowingTransform = ((Component)this).gameObject.transform.Find("Sprites/AgentSprite/AimationFollowingTransform");
		TrailRenderer = ((Component)((Component)this).gameObject.transform.Find("Sprites/AgentSprite/Trail")).GetComponent<TrailRenderer>();
		agentSpriteRend = ((Component)AgentSpriteTransform).GetComponent<SpriteRenderer>();
	}

	public void PushBackAgentSpriteSortingLayer()
	{
		((Renderer)agentSpriteRend).sortingLayerName = "BackClose";
		((Renderer)agentSpriteRend).sortingOrder = 0;
	}

	public void HitReceivedEffect()
	{
		if (hitReceivedEffectCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(hitReceivedEffectCoroutine);
		}
		hitReceivedEffectCoroutine = HitReceivedEffectCoroutine();
		if (((Component)this).gameObject.activeInHierarchy)
		{
			((MonoBehaviour)this).StartCoroutine(hitReceivedEffectCoroutine);
		}
	}

	private IEnumerator HitReceivedEffectCoroutine()
	{
		SetMaterialForAgentSprite(solidColorMaterial);
		yield return (object)new WaitForSeconds(hitReceivedEffectTime);
		SetMaterialForAgentSprite(defaultLitMaterial);
	}

	public void SetMaterialForAgentSprite(Material material)
	{
		((Renderer)agentSpriteRend).material = material;
	}
}
