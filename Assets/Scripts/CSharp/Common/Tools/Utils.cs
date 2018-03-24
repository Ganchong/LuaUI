using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Utils
{	
	public static void CheckTargetPath(string targetPath)
	{
		targetPath = targetPath.Replace('\\', '/');
		int num = targetPath.LastIndexOf('.');
		int length = targetPath.LastIndexOf('/');
		if ((num > 0) && (length < num))
		{
			targetPath = targetPath.Substring(0, length);
		}
		if (!Directory.Exists(targetPath))
		{
			char[] separator = new char[] { '/' };
			string[] strArray = targetPath.Split(separator);
			string path = string.Empty;
			int num3 = strArray.Length;
			for (int i = 0; i < num3; i++)
			{
				path = path + strArray[i] + '/';
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
		}
	}
	
	public static void CleanGrid(GameObject grid)
	{
		int index = 0;
		int childCount = grid.transform.childCount;
		while (index < childCount)
		{
			UnityEngine.Object.Destroy(grid.transform.GetChild(index).gameObject);
			index++;
		}
		grid.transform.DetachChildren();
	}
	
	public static void CopyPathFile(string srcPath, string distPath)
	{
		if (Directory.Exists(srcPath))
		{
			CheckTargetPath(distPath);
			foreach (string str in Directory.GetFiles(srcPath))
			{
				File.Copy(str, distPath + "/" + Path.GetFileName(str), true);
			}
		}
	}
	/** 删除文件夹 */
	public static void deleteFolder(string path)
	{
		if (Directory.Exists(path))
		{
			foreach (string str in Directory.GetFiles(path))
			{
				File.Delete(str);
			}
			foreach (string str2 in Directory.GetDirectories(path))
			{
				deleteFolder(str2);
			}
			Directory.Delete(path);
		}
	}
	
	public static float DirClientToServer(Quaternion rotate)
	{
		return (1.570796f - ((rotate.eulerAngles.y * 3.141593f) / 180f));
	}
	
	public static Quaternion DirServerToClient(float rad)
	{
		return Quaternion.Euler(0f, 90f - ((rad * 180f) / 3.141593f), 0f);
	}

	/** 获取文件内容转成整型 */
	public static bool GetFileInt(string path, out int retInt)
	{
		string str = string.Empty;
		GetFileString(path,out str);
		if (!int.TryParse(str, out retInt))
		{
			Debug.LogError("parse int error path:" + path, null);
			return false;
		}
		return true;
	}
	/** 获取文件内容 */
	public static bool GetFileString(string path, out string str)
	{
		try
		{
			if (!File.Exists(path))
			{
				str = string.Empty;
				return false;
			}
			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			StreamReader reader = new StreamReader(stream);
			str = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogError(exception.ToString(), null);
			str = string.Empty;
			return false;
		}
	}
	
	public static int GetIntNumber(int src, int start, int len)
	{
		if ((start < 0) || (start > 9))
		{
			return -1;
		}
		if (len < 1)
		{
			return -1;
		}
		int num = 0;
		for (int i = 0; i < len; i++)
		{
			num += ((src / ((int) Mathf.Pow(10f, (float) (start + i)))) % 10) * ((int) Mathf.Pow(10f, (float) i));
		}
		return num;
	}
	
	public static string GetMD5Hash(string pathName)
	{
		string str = string.Empty;
		FileStream inputStream = null;
		MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
		try
		{
			inputStream = new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] buffer = provider.ComputeHash(inputStream);
			inputStream.Close();
			str = BitConverter.ToString(buffer).Replace("-", string.Empty);
		}
		catch (Exception exception)
		{
			Debug.LogError("read md5 file error :" + pathName + " e: " + exception.ToString(), null);
		}
		return str;
	}
	
	public static int GetStrCharNum(string text)
	{
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] < '\x0080')
			{
				num++;
			}
			else
			{
				num += 2;
			}
		}
		return num;
	}
	
	public static string GetStreamingAssetPath()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (Application.dataPath + "/Raw");
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return Application.streamingAssetsPath;
		}
		return Application.streamingAssetsPath;
	}
	
	public static int GetStrTextNum(string text)
	{
		return Mathf.CeilToInt(((float) GetStrCharNum(text)) / 2f);
	}
	
	public static string GetTimeDiffFormatString(int timeDiff)
	{
		if (timeDiff <= 0)
		{
			return "00:00:00";
		}
		return string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", timeDiff / 0xe10, (timeDiff % 0xe10) / 60, timeDiff % 60);
	}
	
	public static bool IsContainChatLink(string text)
	{
		return (text.Contains("<a>") && text.Contains("</a>"));
	}
	
	public static bool IsContainEmotion(string text)
	{
		return (text.Contains("[em=") && text.Substring(text.IndexOf("[em=")).Contains("]"));
	}
	
	public static string[] MySplit(string str, string[] nTypeList, string regix)
	{
		int num3;
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		string[] strArray = new string[nTypeList.Length];
		int index = 0;
		for (int i = 0; i <= str.Length; i = num3 + 1)
		{
			num3 = str.IndexOf(regix, i);
			if (num3 < 0)
			{
				string str2 = str.Substring(i);
				if (string.IsNullOrEmpty(str2) && (nTypeList[index].ToLower() != "string"))
				{
					strArray[index++] = "--";
					return strArray;
				}
				strArray[index++] = str2;
				return strArray;
			}
			if (i == num3)
			{
				if (nTypeList[index].ToLower() != "string")
				{
					strArray[index++] = "--";
				}
				else
				{
					strArray[index++] = string.Empty;
				}
			}
			else
			{
				strArray[index++] = str.Substring(i, num3 - i);
			}
		}
		return strArray;
	}
	
	public static float NormaliseDirection(float fDirection)
	{
		float num = 6.283185f;
		float num2 = fDirection;
		if (num2 >= num)
		{
			num2 -= ((int) (fDirection / num)) * num;
			num2 = (num2 <= 0f) ? 0f : num2;
			return ((num2 >= num) ? num : num2);
		}
		if (num2 < 0f)
		{
			num2 += (((int) (-fDirection / num)) + 1) * num;
			num2 = (num2 <= 0f) ? 0f : num2;
			num2 = (num2 >= num) ? num : num2;
		}
		return num2;
	}
	
	public static bool SetIntNumber(ref int src, int start, int len, int val)
	{
		if ((start < 0) || (start > 9))
		{
			return false;
		}
		if (len < 1)
		{
			return false;
		}
		if (val >= Mathf.Pow(10f, (float) len))
		{
			return false;
		}
		for (int i = 0; i < len; i++)
		{
			src -= ((src / ((int) Mathf.Pow(10f, (float) (start + i)))) % 10) * ((int) Mathf.Pow(10f, (float) (start + i)));
			src += ((val / ((int) Mathf.Pow(10f, (float) i))) % 10) * ((int) Mathf.Pow(10f, (float) (start + i)));
		}
		return true;
	}


	public static bool WriteStringToFile(string path, string text)
	{
		return WriteStringToFile(path,text,FileMode.Append);
	}

	public static bool WriteStringToFile(string path,string text,FileMode fileMode)
	{
		try
		{
			FileStream stream = new FileStream(path,fileMode);
			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine(text);
			writer.Close();
			stream.Close();
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogError(exception.ToString(), null);
			return false;
		}
	}
}
