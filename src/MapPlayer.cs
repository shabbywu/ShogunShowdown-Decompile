using UnityEngine;

public class MapPlayer : MonoBehaviour
{
	public InfoBox infoBox;

	private Animator animator;

	private void Awake()
	{
		animator = ((Component)this).GetComponent<Animator>();
	}

	public void UpdateState()
	{
		infoBox.Open();
		infoBox.SetText($"hp: {Globals.Hero.AgentStats.HP}/{Globals.Hero.AgentStats.maxHP}");
	}

	public void WalkToLocation()
	{
		animator.SetBool("Walking", true);
	}

	public void ArrivedAtLocation()
	{
		animator.SetBool("Walking", false);
	}
}
