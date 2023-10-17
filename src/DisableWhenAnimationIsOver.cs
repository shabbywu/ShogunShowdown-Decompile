using UnityEngine;

public class DisableWhenAnimationIsOver : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		((Component)animator).gameObject.SetActive(false);
	}
}
