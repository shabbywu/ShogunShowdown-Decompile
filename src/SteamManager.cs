using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	protected static bool s_EverInitialized;

	protected static SteamManager s_instance;

	protected bool m_bInitialized;

	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	protected static SteamManager Instance
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)s_instance == (Object)null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return s_instance;
		}
	}

	public static bool Initialized => Instance.m_bInitialized;

	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning((object)pchDebugText);
	}

	[RuntimeInitializeOnLoadMethod(/*Could not decode attribute arguments.*/)]
	private static void InitOnPlayMode()
	{
		s_EverInitialized = false;
		s_instance = null;
	}

	protected virtual void Awake()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		if (s_EverInitialized)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
			return;
		}
		if ((Object)(object)s_instance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
			return;
		}
		s_instance = this;
		if (s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError((object)"[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", (Object)(object)this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError((object)"[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", (Object)(object)this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(2084000u)))
			{
				Debug.Log((object)"[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError((object)("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex), (Object)(object)this);
			Application.Quit();
			return;
		}
		m_bInitialized = SteamAPI.Init();
		if (!m_bInitialized)
		{
			Debug.LogError((object)"[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", (Object)(object)this);
		}
		else
		{
			s_EverInitialized = true;
		}
	}

	protected virtual void OnEnable()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if ((Object)(object)s_instance == (Object)null)
		{
			s_instance = this;
		}
		if (m_bInitialized && m_SteamAPIWarningMessageHook == null)
		{
			m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}

	protected virtual void OnDestroy()
	{
		if (!((Object)(object)s_instance != (Object)(object)this))
		{
			s_instance = null;
			if (m_bInitialized)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	protected virtual void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}
}
