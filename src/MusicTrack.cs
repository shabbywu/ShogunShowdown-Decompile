using System;
using UnityEngine;

[Serializable]
public class MusicTrack
{
	public string name;

	public AudioClip clip;

	public AudioClip loopClip;

	public AudioClip outroClip;

	[Range(0f, 1f)]
	public float volume;
}
