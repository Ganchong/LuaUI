using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// 下载器
/// 汪松民
/// </summary>
public class DownloadHelper
{
	/* fields */
	/** 已经下载大小 */
	long alreadyDownloadSize;
	/** 是否停止 */
	bool _stop;
	/** 下载完成回调 */
	Action<bool> action;
	/** 当前下载 */
	int curDownloadIndex;
	/** 已经完整下载文件大小 */
	long alreadyAllDownloadSize;
	/** 本地存放路径 */
	string[] localFilePathArray;
	/** 服务器路径 */
	string[] serverUrlArray;
	/** 下载大小 */
	long[] fileSizeArray;

	/* methods */
	/** 构造方法 */
	private DownloadHelper(List<string> urlList, List<string> fileSavePathList, List<long> fileSizeList, Action<bool> action)
	{
		serverUrlArray = urlList.ToArray();
		localFilePathArray = fileSavePathList.ToArray();
		fileSizeArray = fileSizeList.ToArray();
		this.action = action;
		this._stop = false;
	}
	private DownloadHelper(string url, string fileSavePath, long fileSize, Action<bool> action)
	{
		this.serverUrlArray = new string[]{url};
		this.localFilePathArray = new string[]{fileSavePath};
		this.fileSizeArray = new long[]{fileSize};
		this.action = action;
		this._stop = false;
	}

	/** 下载文件 */
	private IEnumerator downloadFile()
	{
		if ((serverUrlArray == null) || (localFilePathArray == null)||(fileSizeArray==null))
		{
			Debug.LogError("url array or file array is null ");
			if (action != null)
				action(false);
			yield break;
		}
		if (serverUrlArray.Length != localFilePathArray.Length && serverUrlArray.Length!=fileSizeArray.Length)
		{
			Debug.LogError("url array size is not equal file path size");
			if (action != null)
				action(false);
			yield break;
		}
		WWW wwwCurFile = null;
		FileStream fs = null;
		curDownloadIndex = 0;
		while (curDownloadIndex < serverUrlArray.Length)
		{
			if (_stop) break;
			wwwCurFile = new WWW(serverUrlArray[curDownloadIndex]);
			while(!wwwCurFile.isDone)
			{
				alreadyDownloadSize=alreadyAllDownloadSize+(long)(fileSizeArray[curDownloadIndex]*wwwCurFile.progress);
				yield return null;
			}
			if (!string.IsNullOrEmpty(wwwCurFile.error))
			{
				Debug.LogError("download file fail" + serverUrlArray[curDownloadIndex]);
				if (action != null) action(false);
				yield break;
			}
			try
			{
				Utils.CheckTargetPath(localFilePathArray[curDownloadIndex]);
				fs = new FileStream(localFilePathArray[curDownloadIndex], FileMode.OpenOrCreate);
				fs.Write(wwwCurFile.bytes, 0, wwwCurFile.bytesDownloaded);
				fs.Close();
				alreadyAllDownloadSize+=wwwCurFile.bytesDownloaded;
				alreadyDownloadSize=alreadyAllDownloadSize;
			}
			catch (Exception exception)
			{
				Debug.LogError("download file error:" + exception.ToString());
				if (action != null) action(false);
				yield break;
			}
			curDownloadIndex++;
		}
		if (action != null)
		{
			action(!_stop);
		}
	}
	/** 多文件下载 */
	public static DownloadHelper StartDownload(MonoBehaviour monoBehavior, List<string> urlList, List<string> fileSavePathList,List<long> fileSizeList, Action<bool> action)
	{
		DownloadHelper helper = new DownloadHelper(urlList, fileSavePathList, fileSizeList, action);
		monoBehavior.StartCoroutine(helper.downloadFile());
		return helper;
	}
	/** 开始下载 */
	public static DownloadHelper StartDownload(MonoBehaviour monoBehavior, string url, string fileSavePath, long fileSize, Action<bool> action)
	{
		DownloadHelper helper = new DownloadHelper(url, fileSavePath , fileSize, action);
		monoBehavior.StartCoroutine(helper.downloadFile());
		return helper;
	}
	/** 停止下载 */
	public void stop()
	{
		this._stop = true;
	}
	
	public long AlreadyDownloadSize
	{
		get
		{
			return this.alreadyDownloadSize;
		}
	}
}

