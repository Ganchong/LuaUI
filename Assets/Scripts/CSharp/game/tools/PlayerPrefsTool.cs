using System.Collections.Generic;
using System;

public class PlayerPrefsTool
{
    static string GetProjectID(string key)
    {
        string prefsKey = string.Empty;
        prefsKey = key;//string.Format("Pixone.Nonine.{0}", key);
        return prefsKey;
    }
    public static void SetString(string key, string value)
    {
        try
        {
            UnityEngine.PlayerPrefs.SetString(GetProjectID(key), value);
            UnityEngine.PlayerPrefs.Save();
        }
        catch (UnityEngine.PlayerPrefsException e)
        {
//            Debug.LogError(_logTAG, "can not save to pref!!!" + e.Message);
        }
    }
    public static void SetFloat(string key, float value)
    {
        try
        {
            UnityEngine.PlayerPrefs.SetFloat(GetProjectID(key), value);
            UnityEngine.PlayerPrefs.Save();
        }
        catch (UnityEngine.PlayerPrefsException e)
        {
//            Debug.LogError(_logTAG, "can not save to pref!!!" + e.Message);
        }
    }

    public static void SetInt(string key, int value)
    {
        try
        {
            UnityEngine.PlayerPrefs.SetInt(GetProjectID(key), value);
            UnityEngine.PlayerPrefs.Save();
        }
        catch (UnityEngine.PlayerPrefsException e)
        {
//            MDebug.LogError(_logTAG, "can not save to pref!!!" + e.Message);
        }
    }
	public static void SetBool(string key, bool value)
	{
		SetInt(key, value ? 1 : 0);
	}

    public static float GetFloat(string key, float vDefault = 1f)
    {
        return UnityEngine.PlayerPrefs.GetFloat(GetProjectID(key), vDefault);
    }

    public static string GetString(string key)
    {
        return UnityEngine.PlayerPrefs.GetString(GetProjectID(key), "");
    }
    public static int GetInt(string key, int vDefault = 1)
    {
        return UnityEngine.PlayerPrefs.GetInt(GetProjectID(key), vDefault);
    }
   
    public static bool GetBool(string key,bool defaultv)
    {
        return UnityEngine.PlayerPrefs.GetInt(GetProjectID(key), defaultv ? 1 : 0) == 1;
    }

    public static void DeleteLocalData(string key)
    {
        UnityEngine.PlayerPrefs.DeleteKey(GetProjectID(key));
    }
    public static void ClearLoacalData()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
    }
    public static bool HasKey(string key)
    {
        return UnityEngine.PlayerPrefs.HasKey(GetProjectID(key));
    }
}