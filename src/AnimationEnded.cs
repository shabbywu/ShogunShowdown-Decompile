using UnityEngine;

public class AnimationEnded : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		((Component)animator).gameObject.SendMessage("AttackAnimationEnded");
	}
}
