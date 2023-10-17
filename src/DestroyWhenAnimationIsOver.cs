using UnityEngine;

public class DestroyWhenAnimationIsOver : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Object.Destroy((Object)(object)((Component)animator).gameObject, ((AnimatorStateInfo)(ref stateInfo)).length);
	}
}
