using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

/// <summary>
/// 文件帮助器
/// </summary>
public static class FileHelper
{
	/* static fields */
	/** 文件分隔符 */
	public const char FILESPEARATOR='/';
	/** 文件后缀 */
	public const char SUFFIXSPEARATOR='.';

	/* static methods */
	/** 检测目标路径 */
	public static void CheckPath(string targetPath)
	{
		targetPath = targetPath.Replace('\\', FILESPEARATOR);
		int num = targetPath.LastIndexOf('.');
		int length = targetPath.LastIndexOf(FILESPEARATOR);
		if ((num > 0) && (length < num))
		{
			targetPath = targetPath.Substring(0, length);
		}
		if (!Directory.Exists(targetPath))
		{
			char[] separator = new char[] { FILESPEARATOR };
			string[] strArray = targetPath.Split(separator);
			string path = string.Empty;
			int num3 = strArray.Length;
			for (int i = 0; i < num3; i++)
			{
				path = path + strArray[i] + FILESPEARATOR;
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
		}
	}
	/** 复制文件 */
	public static void CopyFile(string srcPath, string distPath)
	{
		if (Directory.Exists(srcPath))
		{
			CheckPath(distPath);
			foreach (string str in Directory.GetFiles(srcPath))
			{
				File.Copy(str, distPath + FILESPEARATOR + Path.GetFileName(str), true);
			}
		}
	}
	/** 删除文件夹 */
	public static void DeleteFolder(string path)
	{
		if (Directory.Exists(path))
		{
			foreach (string str in Directory.GetFiles(path))
			{
				File.Delete(str);
			}
			foreach (string str2 in Directory.GetDirectories(path))
			{
				DeleteFolder(str2);
			}
			Directory.Delete(path);
		}
	}
	/** 获取文件内容转成整型 */
	public static bool GetFileInt(string path, out int retInt)
	{
		string str = string.Empty;
		GetFileString(path,out str);
		if (!int.TryParse(str, out retInt))
		{
			Debug.LogError ("parse int error, str:"+str);
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
				Debug.LogError ("file not exist,path:"+path);
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
			str = string.Empty;
			Debug.LogError ("path:"+path+","+exception.ToString ());
			return false;
		}
	}
	/** 获取文件MD5码 */
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
			Debug.LogError("read md5 file error :" + pathName + " e: " + exception.ToString());
		}
		return str;
	}
	/** 写入文件 */
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
			Debug.LogError("path:"+path+",text:"+text+exception.ToString());
			return false;
		}
	}
	/** 文件替换 */
	public static bool ReplaceFile(string path,string srcText,string desText)
	{
		string content;
		if (GetFileString (path, out content)) 
		{
			if(content.Contains(srcText))
			{
				content=content.Replace(srcText,desText);
				WriteStringToFile(path,content,FileMode.OpenOrCreate);
				return true;
			}
		}
		return false;
	}

	/** 读取所有字节 */
	public static byte[] ReadFileToByteArray(string path)
	{
		try
		{
			if(File.Exists(path))
			{
				var fileInfo=new FileInfo(path);
				byte[] array=new byte[fileInfo.Length];
				using(var steam=File.OpenRead(path))
				{
					steam.Read(array,0,array.Length);
					return array;
				}
			}
			return null;
		}
		catch (Exception exception)
		{
			Debug.LogError("path:"+path+exception.ToString());
			return null;
		}
	}
	/** 获取文件大小 */
	public static long GetFileLength(string path)
	{
		if(File.Exists(path))
		{
			var fileInfo=new FileInfo(path);
			return fileInfo.Length;
		}
		return 0;
	}
	/** 获取文件名称 */
	public static string GetFileName(string filePath)
	{
		filePath = filePath.Replace('\\', FILESPEARATOR);
		int index = filePath.LastIndexOf (FILESPEARATOR);
		if (index < 0) return filePath;
		return filePath.Substring (index+1);
	}
	/** 获取文件名称不包含后缀 */
	public static string GetFileNameNoSuffix(string filePath)
	{
		string fileName = GetFileName (filePath);
		int index=fileName.LastIndexOf (SUFFIXSPEARATOR);
		if (index < 0) return fileName;
		return fileName.Substring (0, index);
	}
	/** 剔除后缀 */
	public static string StripSuffix(string filePath)
	{
		int index=filePath.LastIndexOf (SUFFIXSPEARATOR);
		if (index < 0) return filePath;
		return filePath.Substring (0, index);
	}
	/** 获取后缀 */
	public static string GetSuffix(string filePath)
	{
		int index=filePath.LastIndexOf (SUFFIXSPEARATOR);
		if (index < 0) return string.Empty;
		return filePath.Substring (index);
	}
}