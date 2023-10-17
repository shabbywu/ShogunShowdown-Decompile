using UnityEngine;

public class Environment : MonoBehaviour
{
	public void Initialize()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Parallax[] componentsInChildren = ((Component)this).GetComponentsInChildren<Parallax>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Initialize(((Component)this).transform.position.x);
		}
	}
}
