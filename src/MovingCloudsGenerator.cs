using UnityEngine;
using Utils;

public class MovingCloudsGenerator : MonoBehaviour
{
	public MovingCloud movingCloudPrefab;

	public Sprite[] cloudSprites;

	public float deltaT;

	public float deltaY;

	public float[] velocities;

	public bool prewarm = true;

	public int sortingOrder;

	private float tGeneration;

	private float nPrewarmClouds = 10f;

	private float NextDeltaT => Random.Range(0.75f * deltaT, 1.25f * deltaT);

	private void Start()
	{
		if (prewarm)
		{
			float num = 0f;
			for (int i = 0; (float)i < nPrewarmClouds; i++)
			{
				num += NextDeltaT;
				InstantiateCloud(num);
			}
		}
	}

	private void Update()
	{
		if (!(deltaT <= 0f))
		{
			if (tGeneration <= 0f)
			{
				tGeneration = NextDeltaT;
				InstantiateCloud();
			}
			tGeneration -= Time.deltaTime;
		}
	}

	private void InstantiateCloud(float prewarmTime = 0f)
	{
		Object.Instantiate<GameObject>(((Component)movingCloudPrefab).gameObject, ((Component)this).transform).GetComponent<MovingCloud>().Initialize(Random.Range((0f - deltaY) / 2f, deltaY / 2f), MyRandom.NextFromArray(velocities), MyRandom.NextFromArray(cloudSprites), prewarmTime, sortingOrder);
	}
}
