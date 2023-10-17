using System;
using System.Text;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
	public SaveData saveData;

	public RunSaveData runSaveData;

	private bool debug;

	public static SaveDataManager Instance { get; private set; }

	private string OptionsFileName => "Options.dat";

	private string SaveDataFileName => "SaveData.dat";

	private string RunSaveDataFileName => "RunSaveData.dat";

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
		LoadSaveData();
		LoadRunSaveData();
		LoadOptions();
	}

	public void StoreOptions()
	{
		if (FileManager.WriteToFile(OptionsFileName, Encode(JsonUtility.ToJson((object)Globals.Options))) && debug)
		{
			Debug.Log((object)"Options.dat save successful.");
		}
	}

	public void LoadOptions()
	{
		if (FileManager.FileExists(OptionsFileName))
		{
			if (FileManager.LoadFromFile(OptionsFileName, out var result))
			{
				JsonUtility.FromJsonOverwrite(Decode(result), (object)Globals.Options);
				if (debug)
				{
					Debug.Log((object)"Options.dat load successful.");
				}
			}
		}
		else if (FileManager.FileExists(SaveDataFileName))
		{
			SaveData saveData = new SaveData();
			if (FileManager.LoadFromFile(SaveDataFileName, out var result2))
			{
				JsonUtility.FromJsonOverwrite(Decode(result2), (object)saveData);
				if (debug)
				{
					Debug.Log((object)"Loading Options from SaveData.dat (backwards compatibility)");
				}
			}
			Globals.Options = saveData.legacyOptions;
		}
		else
		{
			Globals.Options = new Options();
			if (debug)
			{
				Debug.Log((object)"No save data containing options was found. Creating default options.");
			}
		}
	}

	public void DeleteStoredOptions()
	{
		FileManager.DeleteFile(OptionsFileName);
	}

	private void PopulateDataFromGlobals()
	{
		saveData.metaCurrency = Globals.KillCount;
		saveData.nRuns = Globals.NRuns;
	}

	public void StoreSaveData()
	{
		saveData.version = Application.version;
		PopulateDataFromGlobals();
		if (FileManager.WriteToFile(SaveDataFileName, Encode(JsonUtility.ToJson((object)saveData))) && debug)
		{
			Debug.Log((object)"Save successful");
		}
	}

	public void LoadSaveData()
	{
		saveData = new SaveData();
		bool flag = false;
		bool flag2 = FileManager.FileExists(SaveDataFileName);
		if (flag2)
		{
			try
			{
				Globals.FirstEverRun = false;
				if (FileManager.LoadFromFile(SaveDataFileName, out var result))
				{
					JsonUtility.FromJsonOverwrite(Decode(result), (object)saveData);
					flag = true;
				}
			}
			catch (Exception arg)
			{
				Debug.Log((object)$"Load not successfull! Error: {arg}.");
			}
			if (!flag)
			{
				Debug.Log((object)"Making back-up of potentially corrupted SaveData.dat file.");
				FileManager.CopyFile(SaveDataFileName, "Corrupted_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + SaveDataFileName);
				saveData = new SaveData();
			}
		}
		if (!flag2 || !flag)
		{
			Globals.FirstEverRun = true;
		}
	}

	public void DeleteStoredSaveData()
	{
		FileManager.DeleteFile(SaveDataFileName);
	}

	public void StoreRunSaveData()
	{
		runSaveData.version = Application.version;
		FileManager.WriteToFile(RunSaveDataFileName, Encode(JsonUtility.ToJson((object)runSaveData)));
	}

	public void LoadRunSaveData()
	{
		runSaveData = new RunSaveData();
		if (FileManager.FileExists(RunSaveDataFileName) && FileManager.LoadFromFile(RunSaveDataFileName, out var result))
		{
			JsonUtility.FromJsonOverwrite(Decode(result), (object)runSaveData);
		}
	}

	public void DeleteStoredRunSaveData()
	{
		FileManager.DeleteFile(RunSaveDataFileName);
	}

	private string Encode(string data)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
	}

	private string Decode(string data)
	{
		return Encoding.UTF8.GetString(Convert.FromBase64String(data));
	}
}
