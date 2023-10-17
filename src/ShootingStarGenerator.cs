using System.Collections;
using UnityEngine;

public class ShootingStarGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject shootingStarPrefab;

	[SerializeField]
	private float minTimeBeweenStars;

	[SerializeField]
	private float maxTimeBetweenStars;

	[SerializeField]
	private float width;

	[SerializeField]
	private float height;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(WaitAndSpawnNext(0f));
	}

	private IEnumerator WaitAndSpawnNext(float waitTime)
	{
		yield return (object)new WaitForSeconds(waitTime);
		Vector3 val = ((Component)this).transform.position + new Vector3(Random.Range((0f - width) / 2f, width / 2f), Random.Range((0f - height) / 2f, height / 2f), 0f);
		Object.Instantiate<GameObject>(shootingStarPrefab, val, Quaternion.identity, ((Component)this).transform);
		((MonoBehaviour)this).StartCoroutine(WaitAndSpawnNext(Random.Range(minTimeBeweenStars, maxTimeBetweenStars)));
	}
}
