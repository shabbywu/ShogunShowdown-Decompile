using UnityEngine;

public class NobunagaVulnerableEffect : MonoBehaviour
{
	public void Disappear()
	{
		((Component)this).GetComponent<Animator>().SetTrigger("Disappear");
	}
}
