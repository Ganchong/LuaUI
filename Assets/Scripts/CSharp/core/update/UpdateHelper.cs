using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ABSystem;
/// <summary>
/// 更新检测工具
/// 汪松民
/// </summary>
public class UpdateHelper : MonoBehaviour
{
	/* static fields */
	/** 包版本信息路径 */
	public static string appVersionPath;
	/** 资源加载路径 */
	private static string LocalPathRoot = ResourceHelper.ASEETPATH;
	/** 当前版本信息路径 */
	public static string localVersionPath = (LocalPathRoot + "/VersionData");
	/** 缓存路径 */
	public static string ResCachePath;
	/** 版本信息临时路径 */
	public static string cacheVersionPath;
	/** 更新列表 */
	public static string ResFileListName = "list.settings";
	/** 当前资源版本文件 */
	public static string ResFileVersion = "resourceVersion.txt";

	/* fields */
	/** 下载资源临时目录 */
	string cacheDataPath;
	/** 当前更新结果 */
	UpdateResult curUpdateResult = UpdateResult.None;
	/** 当前更新步骤 */
	UpdateStep curUpdateStep = UpdateStep.None;
	/** 数据下载器 */
	DownloadHelper dataFileDownloader;
	/** 服务器版本号 */
	int serverVersion = -1;
	/** 服务器资源路径 */
	string serverDataPath;
	/** 需要下载资源大小 */
	long totalNeedDownloadSize;
	/** 本地资源列表信息 */
	Dictionary<string,FileInfo> localFileInfoMap = new Dictionary<string, FileInfo>();
	/** 服务器资源列表信息 */
	Dictionary<string,FileInfo> serverFileInfoMap = new Dictionary<string, FileInfo>();

	List<long> listDownloadFileSizes = new List<long>();
	List<string> listDownloadFiles = new List<string>();
	List<string> listDownloadFileUrls = new List<string>();
	List<string> listUpdateErrorFiles = new List<string>();
	List<string> listUpdateFiles = new List<string>();
	string resServerUrl = "https://127.0.0.1:1100/resroot";

	/** 版本内容信息下载器 */
	DownloadHelper versionFileDownloader;

	/** mehtods */
	/** 初始化 */
	private void Awake()
	{
		appVersionPath = (Application.streamingAssetsPath + "/VersionData");
		ResCachePath = (Application.temporaryCachePath + "/temp");
		cacheVersionPath = (ResCachePath + "/VersionData");
		cacheDataPath = ResCachePath;
	}

	/** 更新完成状态 */
	private void updateFinish(UpdateResult result)
	{
		curUpdateStep = UpdateStep.Finish;
		curUpdateResult = result;
	}

	/** 检查更新 */
	public void startCheckData(string resServerUrl,string serverVersionStr)
	{
		if (versionFileDownloader != null)
		{
			versionFileDownloader.stop();
		}
		if (dataFileDownloader != null)
		{
			dataFileDownloader.stop();
		}
		this.resServerUrl = resServerUrl;
		if(Application.platform==RuntimePlatform.Android||Application.platform==RuntimePlatform.WindowsEditor)
		{
			this.resServerUrl+="/android";
		}
		else if(Application.platform==RuntimePlatform.IPhonePlayer||Application.platform==RuntimePlatform.OSXEditor)
		{
			this.resServerUrl+="/ios";
		}
		if(Log.isInfoEnable()) Log.info(Application.platform.ToString());
		serverDataPath = this.resServerUrl;
		serverVersion = TextKit.parseInt(serverVersionStr);
		versionFileDownloader = null;
		totalNeedDownloadSize = 0L;
		curUpdateStep = UpdateStep.CheckVesion;
		onCheckVersion();
	}
	/** 检查版本 */
	private void onCheckVersion()
	{
		Log.debug("comparing version");
		int packageVersion = GameManager.Instance.streamingAssetVersion;
		string path = localVersionPath + "/" + ResFileVersion;
		int retInt=0;
		if (File.Exists(path))
		{
			if (!Utils.GetFileInt(path, out retInt))
			{
				Debug.LogError("parse version error");
			}
			if(packageVersion>retInt)
			{
				retInt=packageVersion;
				try
				{
					Utils.deleteFolder(LocalPathRoot);
				}
				catch (Exception exception)
				{
				}
			}
		}
		else retInt=packageVersion;
		Log.debug(string.Concat(new object[] { "version --local:", retInt.ToString(), " --remote:", serverVersion }));
		if (serverVersion > retInt)
		{
			SDKHelper.saveState(StateStep.STEP11);
			Log.debug("server version is big than local, begin update");
			try
			{
				Utils.deleteFolder(cacheDataPath);
				Utils.deleteFolder(cacheVersionPath);
			}
			catch (Exception exception)
			{
			}
			curUpdateStep = UpdateStep.GetUpdateList;
			versionFileDownloader = DownloadHelper.StartDownload(this, resServerUrl + "/" + ResFileListName, cacheVersionPath + "/" + ResFileListName, 1000, onDownloadDataFileList);
		}
		else
		{
			GameManager.Instance.streamingAssetVersion=retInt;
			updateFinish(UpdateResult.Success);
		}
	}

	/** 下载版本文件完成 */
	private void onDownloadDataFileList(bool bSuccess)
	{
		if (bSuccess)
		{
			curUpdateStep = UpdateStep.CompareData;
			Log.debug("load file success: " + cacheVersionPath + "/" + ResFileListName);
			string msg = localVersionPath + "/" + ResFileListName;
			string path = appVersionPath + "/" + ResFileListName;
			string versionPath = cacheVersionPath + "/" + ResFileListName;
			Log.debug(msg);
			if (!File.Exists(msg) && File.Exists(path))
			{
				Log.debug("copy file " + msg + "  " + path);
				Utils.CheckTargetPath(msg);
				File.Copy(path, msg);
				Log.debug(path);
			}
			localFileInfoMap.Clear();
			serverFileInfoMap.Clear();
			listUpdateFiles.Clear();
			if(File.Exists(msg)) ReadVersionFileToDic(msg, localFileInfoMap);
			if (!ReadVersionFileToDic(versionPath, serverFileInfoMap))
			{
				updateFinish(UpdateResult.LoadServerFailListError);
			}
			else
			{
				foreach (KeyValuePair<string, FileInfo> pair in serverFileInfoMap)
				{
					if (localFileInfoMap.ContainsKey(pair.Key))
					{
						if (localFileInfoMap[pair.Key].Md5 != pair.Value.Md5)
						{
							listUpdateFiles.Add(pair.Key);
						}
					}
					else {
						listUpdateFiles.Add(pair.Key);
					}
				}
				if (listUpdateFiles.Count > 0)
				{
					listDownloadFileUrls.Clear();
					listDownloadFiles.Clear();
					listDownloadFileSizes.Clear();
					totalNeedDownloadSize = 0L;
					foreach (string str4 in listUpdateFiles)
					{
						string str5 = cacheDataPath + "/" + str4;
						if (!File.Exists(str5) || (Utils.GetMD5Hash(str5).ToLower() != serverFileInfoMap[str4].Md5.ToLower()))
						{
							listDownloadFileUrls.Add(serverDataPath + "/" + str4);
							listDownloadFiles.Add(str5);
							listDownloadFileSizes.Add(serverFileInfoMap[str4].Size);
							totalNeedDownloadSize += serverFileInfoMap[str4].Size;
						}
					}
					if (listDownloadFiles.Count > 0)
					{
						curUpdateStep = UpdateStep.MakeSureDownload;
						return;
					}
				}
				downloadData(true);
			}
		}
		else
		{
			updateFinish(UpdateResult.GetUpdateListFail);
		}
	}

	/** 下载当前文件列表 */
	public void downloadCurFileList()
	{
		curUpdateStep = UpdateStep.DownloadData;
		Log.debug("have diff files");
		dataFileDownloader = DownloadHelper.StartDownload(this, listDownloadFileUrls, listDownloadFiles, listDownloadFileSizes, downloadData);
	}

	/** 下载完成 */
	private void downloadData(bool bSuccess)
	{
		if (bSuccess)
		{
			Log.debug("check res md5");
			curUpdateStep = UpdateStep.CheckData;
			listUpdateErrorFiles.Clear();
			foreach (string str in listUpdateFiles)
			{
				string path = cacheDataPath + "/" + str;
				if (!File.Exists(path))
				{
					Debug.LogError("download file fail " + str);
					listUpdateErrorFiles.Add(str);
				}
				else
				{
					string str3 = Utils.GetMD5Hash(path);
					if (!serverFileInfoMap.ContainsKey(str) || (str3.ToLower() != serverFileInfoMap[str].Md5.ToLower()))
					{
						Debug.LogError("download file md5 error : romote:" + serverFileInfoMap[str].Md5 + " local :" + str3);
						listUpdateErrorFiles.Add(str);
					}
					else if (localFileInfoMap.ContainsKey(str))
					{
						localFileInfoMap[str].copyData(serverFileInfoMap[str]);
					}
					else
					{
						localFileInfoMap.Add(str, serverFileInfoMap[str]);
					}
				}
			}
			if (listUpdateErrorFiles.Count == 0)
			{
				Log.debug("check res success");
				copyDataToDataPath();
			}
			else
			{
				Debug.LogError("check res fail");
				updateFinish(UpdateResult.CheckDataFail);
			}
		}
		else
		{
			Debug.LogError("download res  error");
			updateFinish(UpdateResult.DownloadFail);
		}
	}

	/** 复制下载文件到加载路径下 */
	private void copyDataToDataPath()
	{
		try
		{
			Log.debug("copy res");
			curUpdateStep = UpdateStep.CopyData;
			foreach (string str in listUpdateFiles)
			{
				string targetPath = LocalPathRoot + "/" + str;
				Utils.CheckTargetPath(targetPath);
				File.Copy(cacheDataPath + "/" + str, targetPath, true);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("copy res error!" + exception.ToString());
			updateFinish(UpdateResult.CopyDataFail);
			return;
		}
		saveLocalFileList();
	}
	/** 保存资源列表信息 */
	private void saveLocalFileList()
	{
		try
		{
			Log.debug("copy filelist");
			if (!Utils.saveDataFileList(localVersionPath + "/" + ResFileListName, localFileInfoMap))
			{
				Debug.LogError("generate version file fail!");
				updateFinish(UpdateResult.SaveFileListFail);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("generate version file fail!" + exception.ToString());
			updateFinish(UpdateResult.SaveFileListFail);
			return;
		}
		copyVersionFile();
	}
	/** 保存版本分文信息 */
	private void copyVersionFile()
	{
		try
		{
			Log.debug("copy version file");
			string targetPath = localVersionPath + "/" + ResFileVersion;
			Utils.CheckTargetPath(targetPath);
			FileStream stream = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine(serverVersion.ToString());
			writer.Close();
			stream.Close();
			GameManager.Instance.streamingAssetVersion=serverVersion;
		}
		catch (Exception exception)
		{
			Debug.LogError("generate version file fail!" + exception.ToString());
			updateFinish(UpdateResult.SaveVersionFileFail);
			return;
		}
		cleanCacheFiles();
	}

	/** 清楚缓存文件 */
	private void cleanCacheFiles()
	{
		Log.debug("clean cache");
		curUpdateStep = UpdateStep.CleanCache;
		try
		{
			Utils.deleteFolder(cacheDataPath);
			Utils.deleteFolder(cacheVersionPath);
		}
		catch (Exception exception)
		{
			Debug.LogError("clean cache fail!" + exception.ToString());
			updateFinish(UpdateResult.CleanCacheFail);
			return;
		}
		curUpdateStep = UpdateStep.Finish;
		updateFinish(UpdateResult.Success);
	}

	

	
	private bool ReadVersionFileToDic(string versionPath, Dictionary<string, FileInfo> curDic)
	{
		try
		{
			if (!File.Exists(versionPath)) return false;
			string content = string.Empty;
			Utils.GetFileString(versionPath,out content);
			if(string.IsNullOrEmpty(content)) return false;
			content = content.Replace("\r\n","\n");
			string[] lines = content.Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
			FileInfo info = null;
			foreach(var str in lines)
			{
				info = new FileInfo(str);
				curDic.Add(info.Path,info);
			}
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogError("read version file error :" + versionPath + " e: " + exception.ToString());
			return false;
		}
	}
	
	public long CurDownloadSize
	{
		get
		{
			return ((dataFileDownloader == null) ? 0L : dataFileDownloader.AlreadyDownloadSize);
		}
	}
	
	public UpdateResult CurUpdateResult
	{
		get
		{
			return curUpdateResult;
		}
	}
	
	public UpdateStep CurUpdateStep
	{
		get
		{
			return curUpdateStep;
		}
	}
	
	public long DownloadTotalSize
	{
		get
		{
			return totalNeedDownloadSize;
		}
	}
	/** 下载列表 */
	public List<string> DownLoadList
	{
		get
		{
			return listUpdateFiles;
		}
	}

	public class FileInfo
	{
		private const char SPLITCHAR = '|';
		int id;
		string md5;
		long size;
		string path;
		
		public FileInfo(string str)
		{
			string[] strs = str.Split(SPLITCHAR);
			id = TextKit.parseInt(strs[0]);
			md5 = strs[1];
			size = TextKit.parseLong(strs[2]);
			path = strs[3];
		}

		public void copyData(FileInfo info)
		{
			id = info.id;
			md5 = info.md5;
			size = info.size;
			path = info.path;
		}
		public int Id {
			get {
				return this.id;
			}
		}

		public string Md5 {
			get {
				return this.md5;
			}
		}

		public long Size {
			get {
				return this.size;
			}
		}

		public string Path {
			get {
				return this.path;
			}
		}
		public override string ToString ()
		{
			return string.Join(SPLITCHAR.ToString(),new string[]{id.ToString(),md5,size.ToString(),path});
		}
	}
	/** 下载结果 */
	public enum UpdateResult
	{
		None,						//未开始
		Success,					//成功
		GetVersionFail,				//获取版本信息失败
		GetUpdateListFail,			//获取需更新列表失败
		CheckDataFail,				//检查资源失败
		DownloadFail,				//下载失败
		CopyDataFail,				//复制文件失败
		SaveVersionFileFail,		//保存版本信息
		SaveFileListFail,			//保存文件信息
		CleanCacheFail,				//清理缓存
		LoadServerFailListError		//读取服务器文件信息
	}
	/** 下载步骤 */
	public enum UpdateStep
	{
		None,				//未开始
		CheckVesion,		//检查版本
		GetUpdateList,		//获取文件列表
		CompareData,		//比较列表
		MakeSureDownload,	//询问是否下载
		DownloadData,		//下载资源
		CheckData,			//校验资源
		CopyData,			//复制资源到加载目录
		CleanCache,			//清空临时目录
		Finish				//完成
	}
}

