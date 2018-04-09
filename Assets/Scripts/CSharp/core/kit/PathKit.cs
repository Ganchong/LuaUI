using UnityEngine;
using System.Collections;
/// <summary>
/// Path kit.路径工具
/// </summary>
public class PathKit
{
	/* static fields */
	/** 前缀 */
	public const string PREFIX = "file://";
	/** 包裹后缀 */
	private const string FORMAT = ".unity3d";
	/** 可变资源路径 */
	private static string VARIABLERESPATH = Application.persistentDataPath + "/";
	/** 固定资源路径 */
	private static string STREAMASSETPAHT = Application.streamingAssetsPath + "/";

	/* static methods */
	/** 获取资源路径 */
	public static string getStreamBundleUrl (string subFolder, string fileName)
	{
		string url = "";
		if(string.IsNullOrEmpty(fileName))	subFolder = getFileDir (subFolder);
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer )
			url = PREFIX + STREAMASSETPAHT + subFolder + fileName + FORMAT;
		else if (Application.platform == RuntimePlatform.Android )
			url = STREAMASSETPAHT + subFolder + fileName + FORMAT;
		else if (  Application.platform == RuntimePlatform.OSXEditor ||  Application.platform == RuntimePlatform.IPhonePlayer)
			url = PREFIX + STREAMASSETPAHT + subFolder + fileName + FORMAT;
		return url;
	}
	/** 更具路径获取文件名称(可能带后缀) */
	public static string getFileName(string path)
	{
		int index = path.LastIndexOf("/");
		if (index < 0 || path.Length == index + 1) return "";
		return path.Substring(index + 1);
	}
	/** 获取文件路径 */
	public static string getFileDir(string path)
	{
		path = path.Replace("\\","/");
		path = path.Substring(0,path.LastIndexOf("/"));
		if (path.StartsWith (PREFIX)) path = path.Substring (6);
		return path;
	}
	/** 创建文件如果不存在 */
	public static void createDirIfNotExists(string path)
	{
		string dir = getFileDir(path);
		if(!System.IO.Directory.Exists(dir))
		{
			System.IO.Directory.CreateDirectory(dir);
		}
	}
	
}
