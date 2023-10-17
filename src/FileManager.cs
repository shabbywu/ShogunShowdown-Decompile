using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
	public static bool WriteToFile(string a_FileName, string a_FileContents)
	{
		string text = Path.Combine(Application.persistentDataPath, a_FileName);
		try
		{
			File.WriteAllText(text, a_FileContents);
			return true;
		}
		catch (Exception arg)
		{
			Debug.LogWarning((object)$"Failed to write to {text} with exception {arg}");
			return false;
		}
	}

	public static bool LoadFromFile(string a_FileName, out string result)
	{
		string text = Path.Combine(Application.persistentDataPath, a_FileName);
		try
		{
			result = File.ReadAllText(text);
			return true;
		}
		catch (Exception arg)
		{
			Debug.LogWarning((object)$"Failed to read from {text} with exception {arg}");
			result = "";
			return false;
		}
	}

	public static bool DeleteFile(string a_FileName)
	{
		string text = Path.Combine(Application.persistentDataPath, a_FileName);
		try
		{
			File.Delete(text);
			return true;
		}
		catch (Exception arg)
		{
			Debug.LogWarning((object)$"Failed to delete file {text} with exception {arg}");
			return false;
		}
	}

	public static bool FileExists(string a_FileName)
	{
		return File.Exists(Path.Combine(Application.persistentDataPath, a_FileName));
	}

	public static bool CopyFile(string sourceFileName, string destinationFileName)
	{
		string text = Path.Combine(Application.persistentDataPath, sourceFileName);
		string text2 = Path.Combine(Application.persistentDataPath, destinationFileName);
		try
		{
			File.Copy(text, text2, overwrite: true);
			return true;
		}
		catch (Exception arg)
		{
			Debug.LogWarning((object)$"Failed to copy file from {text} to {text2} with exception {arg}");
			return false;
		}
	}
}
