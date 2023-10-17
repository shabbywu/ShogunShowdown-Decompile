using UnityEngine;

public class AnymSync : MonoBehaviour
{
	protected Animator sync;

	protected Animator Animation;

	private void Start()
	{
		sync = GameObject.Find("AnimSync").GetComponent<Animator>();
		Animation = ((Component)this).GetComponent<Animator>();
	}

	private void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Animator animation = Animation;
		AnimatorStateInfo currentAnimatorStateInfo = sync.GetCurrentAnimatorStateInfo(0);
		animation.Play(0, -1, ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).normalizedTime);
	}
}
