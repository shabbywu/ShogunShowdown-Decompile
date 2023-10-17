using System;
using System.Collections;
using System.Collections.Generic;
using AudioUtilsNameSpace;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	[SerializeField]
	private AudioMixer masterMixer;

	[SerializeField]
	private float musicVolumeMultiplier;

	[SerializeField]
	private MusicTrack[] tracks;

	private Dictionary<string, MusicTrack> tracksDict;

	private AudioSource audioSource;

	public const float defaultSwitchTime = 0.3f;

	private IEnumerator waitEndAndLoopNextCoroutine;

	public static MusicManager Instance { get; private set; }

	public string CurrentTrackName { get; private set; }

	public MusicTrack CurrentTrack { get; private set; }

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null && (Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else
		{
			Instance = this;
		}
		audioSource = ((Component)this).GetComponent<AudioSource>();
		tracksDict = new Dictionary<string, MusicTrack>();
		MusicTrack[] array = tracks;
		foreach (MusicTrack musicTrack in array)
		{
			tracksDict.Add(musicTrack.name, musicTrack);
		}
	}

	private void Start()
	{
		SetVolume(Globals.Options.musicVolume);
	}

	public void SmoothSetVolume(int from, int to, float transitionTime)
	{
		LeanTween.value(((Component)this).gameObject, (Action<float>)SetVolume, (float)from, (float)to, transitionTime);
	}

	public void SetVolume(int volume)
	{
		masterMixer.SetFloat("musicVolume", AudioUtils.VolumeMapping(volume, AudioUtils.maxVolume));
	}

	public void Play(string name, float switchTime = 0.3f, bool loop = true)
	{
		if (!tracksDict.ContainsKey(name))
		{
			Debug.LogWarning((object)("MusicManager: did not find track with name '" + name + "'"));
		}
		else if (!audioSource.isPlaying || CurrentTrack == null || !(CurrentTrack.name == name))
		{
			StopPotentialLoopNext();
			CurrentTrack = tracksDict[name];
			((MonoBehaviour)this).StartCoroutine(SmoothTrackSwitching(CurrentTrack, switchTime));
			if (loop && (Object)(object)CurrentTrack.loopClip != (Object)null)
			{
				audioSource.loop = false;
				waitEndAndLoopNextCoroutine = WaitEndAndLoopNext();
				((MonoBehaviour)this).StartCoroutine(waitEndAndLoopNextCoroutine);
			}
			else
			{
				audioSource.loop = loop;
			}
		}
	}

	public void Stop()
	{
		StopPotentialLoopNext();
		if ((Object)(object)CurrentTrack.outroClip == (Object)null)
		{
			audioSource.Stop();
			return;
		}
		audioSource.clip = CurrentTrack.outroClip;
		audioSource.loop = false;
		audioSource.Play();
	}

	private IEnumerator SmoothTrackSwitching(MusicTrack track, float switchTime)
	{
		if (audioSource.isPlaying && switchTime > 0f)
		{
			float rate = audioSource.volume / switchTime;
			float t = 0f;
			while (t < switchTime)
			{
				AudioSource obj = audioSource;
				obj.volume -= Mathf.Clamp(rate * Time.deltaTime, 0f, 1f);
				t += Time.deltaTime;
				yield return null;
			}
		}
		audioSource.volume = track.volume * musicVolumeMultiplier;
		audioSource.clip = track.clip;
		audioSource.Play();
	}

	private IEnumerator WaitEndAndLoopNext()
	{
		if (audioSource.loop)
		{
			Debug.LogError((object)"WaitEndAndPlayNext: Loop is on...");
		}
		while (audioSource.isPlaying)
		{
			yield return null;
		}
		audioSource.clip = CurrentTrack.loopClip;
		audioSource.Play();
		audioSource.loop = true;
		waitEndAndLoopNextCoroutine = null;
	}

	private void SetVolume(float volume)
	{
		SetVolume((int)volume);
	}

	private void StopPotentialLoopNext()
	{
		if (waitEndAndLoopNextCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(waitEndAndLoopNextCoroutine);
			waitEndAndLoopNextCoroutine = null;
		}
	}
}
