using UnityEngine;
using UnityEngine.UI;

public class CutOutMaskUI : Image
{
	public override Material materialForRendering
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			Material val = new Material(((Graphic)this).materialForRendering);
			val.SetInt("_StencilComp", 6);
			return val;
		}
	}
}
