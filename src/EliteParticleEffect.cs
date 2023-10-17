using AgentEnums;
using UnityEngine;
using Utils;

public class EliteParticleEffect : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem[] particleSystems;

	public void Initialize(EliteTypeEnum eliteType)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		string hexString = "ffffff";
		switch (eliteType)
		{
		case EliteTypeEnum.reactiveShield:
			hexString = "73bed3";
			break;
		case EliteTypeEnum.doubleStrike:
			hexString = "cf573c";
			break;
		case EliteTypeEnum.heavy:
			hexString = "757575";
			break;
		case EliteTypeEnum.quickWitted:
			hexString = "e8c170";
			break;
		}
		ParticleSystem[] array = particleSystems;
		for (int i = 0; i < array.Length; i++)
		{
			ColorOverLifetimeModule colorOverLifetime = array[i].colorOverLifetime;
			((ColorOverLifetimeModule)(ref colorOverLifetime)).enabled = true;
			Gradient val = new Gradient();
			val.SetKeys((GradientColorKey[])(object)new GradientColorKey[1]
			{
				new GradientColorKey(Colors.FromHex(hexString), 0f)
			}, (GradientAlphaKey[])(object)new GradientAlphaKey[5]
			{
				new GradientAlphaKey(0.5f, 0f),
				new GradientAlphaKey(1f, 0.25f),
				new GradientAlphaKey(0.5f, 0.5f),
				new GradientAlphaKey(1f, 0.75f),
				new GradientAlphaKey(0f, 1f)
			});
			((ColorOverLifetimeModule)(ref colorOverLifetime)).color = new MinMaxGradient(val);
		}
	}

	public void Stop()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
