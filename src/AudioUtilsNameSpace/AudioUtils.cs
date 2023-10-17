using UnityEngine;

namespace AudioUtilsNameSpace;

public static class AudioUtils
{
	public static int maxVolume = 11;

	public static float VolumeMapping(int volume, int maxVolume)
	{
		if (volume == 0)
		{
			return -80f;
		}
		int num = maxVolume / 3;
		return Mathf.Log10((float)(maxVolume / 2 + volume + num)) * (80f / Mathf.Log10((float)(maxVolume + num))) - 80f;
	}
}
