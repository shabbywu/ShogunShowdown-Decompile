using System.Collections;
using System.Collections.Generic;
using AudioUtilsNameSpace;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectsManager : MonoBehaviour
{
	public AudioMixer masterMixer;

	public Sound[] soundsUI;

	public Sound[] soundsCombat;

	private Dictionary<string, Sound> soundsDict;

	private AudioSource audioSource;

	private List<string> recentlyPlayedSoundEffects = new List<string>();

	private static float recentlyPlayedSoundEffectsClearTime = 0.1f;

	public static SoundEffectsManager Instance { get; private set; }

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
		List<Sound> list = new List<Sound>();
		Sound[] array = soundsUI;
		foreach (Sound item in array)
		{
			list.Add(item);
		}
		array = soundsCombat;
		foreach (Sound item2 in array)
		{
			list.Add(item2);
		}
		soundsDict = new Dictionary<string, Sound>();
		foreach (Sound item3 in list)
		{
			soundsDict.Add(item3.name, item3);
		}
	}

	private void Start()
	{
		SetVolume(Globals.Options.soundEffectsVolume);
		((MonoBehaviour)this).InvokeRepeating("ClearRecentlyPlayedSoundEffects", 0f, recentlyPlayedSoundEffectsClearTime);
	}

	public void SetVolume(int volume)
	{
		masterMixer.SetFloat("soundEffectsVolume", AudioUtils.VolumeMapping(volume, AudioUtils.maxVolume));
	}

	public void Play(string name)
	{
		if (!soundsDict.ContainsKey(name))
		{
			Debug.LogWarning((object)("SoundEffectsManager: did not find sound with name '" + name + "'"));
			return;
		}
		if (Time.timeScale != 0f)
		{
			if (recentlyPlayedSoundEffects.Contains(name))
			{
				return;
			}
			recentlyPlayedSoundEffects.Add(name);
		}
		audioSource.PlayOneShot(soundsDict[name].clip, soundsDict[name].volume);
	}

	public void PlayRandom(string[] names)
	{
		((Object)this).name = names[Random.Range(0, names.Length)];
		Play(((Object)this).name);
	}

	public void PlayAfterDeltaT(string name, float deltaT)
	{
		((MonoBehaviour)this).StartCoroutine(PlayAfterDeltaTCoroutine(name, deltaT));
	}

	private IEnumerator PlayAfterDeltaTCoroutine(string name, float deltaT)
	{
		yield return (object)new WaitForSeconds(deltaT);
		Play(name);
	}

	private void ClearRecentlyPlayedSoundEffects()
	{
		recentlyPlayedSoundEffects.Clear();
	}
}
