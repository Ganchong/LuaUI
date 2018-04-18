using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ABSystem;

namespace LuaFramework.Core
{
	/// <summary>
	/// 资源更新管理器
	/// </summary>
	public class ResourceUpdateManager : SingletonBehaviour<ResourceUpdateManager>
	{
		/* 更新进度委托 */
		public delegate void SetUpdateTips (string tips, float process);
		/** 确认更新委托 */
		public delegate void MakeSureDownLoad (string size);
		/** 更新下载大小委托 */
		public delegate void UpdateDownLoadSize (string curSize, string totalSize);
		/** 下载完成委托 */
		public delegate void DownLoadFinish ();

		public SetUpdateTips setUpdateTips;

		public MakeSureDownLoad makeSureDownLoad;

		public UpdateDownLoadSize updateDownLoadSize;

		public DownLoadFinish downLoadFinish;

		/* fields */
		/** 更新器 */
		UpdateHelper updateHelper;
		/** 最后更新步骤 */
		UpdateHelper.UpdateStep lastUpdateStep;
		/** 当前进度 */
		float process = -1;
		/** 是否有更新 */
		bool update = false;

		/** 初始化委托 */
		public void InitDelegate (SetUpdateTips setUpdateTips, MakeSureDownLoad makeSureDownLoad,
		                         UpdateDownLoadSize updateDownLoadSize, DownLoadFinish downloadFinish)
		{
			this.process = 0;
			this.setUpdateTips = setUpdateTips;
			this.makeSureDownLoad = makeSureDownLoad;
			this.updateDownLoadSize = updateDownLoadSize;
			this.downLoadFinish = downloadFinish;
		}

		/** 连接服务器 */
		public void ConnectSDKServer (Action success, Action<string> fail)
		{
			SDKHelper.connectServer ((message) => {
				IJsonNode node = message;
				node = node ["error"];
				if (node == null || string.IsNullOrEmpty (node.ToString ())) {
					SDKHelper.saveState (StateStep.STEP8);
					int nowTime = message ["nowTime"].ToInt ();
					TimeKit.resetTime (nowTime * 1000L);
					success ();
				} else {
					fail (node.ToString ());
				}
			});
		}

		/** 连接服务器 */
		public void AccessHttpConnect (string url, Action<string> success, Action fail)
		{
			HttpConnect.access (url, (ans, result) => {
				if (ans == HttpConnect.OK) {
					if (Log.isInfoEnable ())
						Log.info ("serverVersion:" + result);
					success (result);
				} else {
					if (Log.isInfoEnable ())
						Log.info ("update expcetion!");
					fail ();
				}
			});
		}

		/** 开始更新 */
		public void StartUpdate (string result)
		{
			updateHelper = gameObject.AddComponent<UpdateHelper> ();
			lastUpdateStep = updateHelper.CurUpdateStep;
			updateHelper.startCheckData (SDKHelper.getUpdateDataUrl (), result);
		}

		/** 下载当前文件列表 */
		public void DownLoadCurFileList ()
		{
			updateHelper.downloadCurFileList ();
		}

		/** 更新 */
		void Update ()
		{
			if (process < 0)
				return;
			if (updateHelper != null && lastUpdateStep != updateHelper.CurUpdateStep) {
				lastUpdateStep = updateHelper.CurUpdateStep;
				switch (lastUpdateStep) {
				case UpdateHelper.UpdateStep.CheckVesion:
					update = false;
					process = 0;
					setUpdateTips ("TIP_2", process);
					break;
				case UpdateHelper.UpdateStep.GetUpdateList:
					process = 0.1f;
					setUpdateTips ("TIP_3", process);
					break;
				case UpdateHelper.UpdateStep.CompareData:
					process = 0.2f;
					setUpdateTips ("TIP_4", process);
					break;
				case UpdateHelper.UpdateStep.MakeSureDownload:
					SDKHelper.saveState (StateStep.STEP12);
					makeSureDownLoad ((updateHelper.DownloadTotalSize * 1f / (1024 * 1024)).ToString ("F"));
					update = true;
					process = 0.3f;
					setUpdateTips ("", process);
					break;
				case UpdateHelper.UpdateStep.DownloadData:
					break;
				case UpdateHelper.UpdateStep.CheckData:
					process = 0.8f;
					setUpdateTips ("TIP_6", process);
					break;
				case UpdateHelper.UpdateStep.CopyData:
					process = 0.9f;
					setUpdateTips ("TIP_7", process);
					break;
				case UpdateHelper.UpdateStep.CleanCache:
					break;
				case UpdateHelper.UpdateStep.Finish:
					if (updateHelper.CurUpdateResult != UpdateHelper.UpdateResult.Success) {
						setUpdateTips ("TIP_9", process);
						SDKHelper.saveState (StateStep.STEP13);
					} else {
						SDKHelper.saveState (StateStep.STEP14);
						process = 0.9f;
						int count = 0;
						Action loadFinish = () => {
							count++;
							if (count < 3)
								return;
							downLoadFinish ();
						};
						if(update){
							ReleaseLuaCache(loadFinish);
							ReInitCache(loadFinish);
							ResourceHelper.Instance.InitManifest(()=>{InitResource(loadFinish);});
						}else
							downLoadFinish();
					}
					break;
				}
			}
			if (lastUpdateStep == UpdateHelper.UpdateStep.DownloadData) {
				process = 0.3f + 0.4f * updateHelper.CurDownloadSize / updateHelper.DownloadTotalSize;
				setUpdateTips ("", process);
				updateDownLoadSize ((updateHelper.CurDownloadSize * 1f / (1024 * 1024)).ToString ("F"), (updateHelper.DownloadTotalSize * 1f / (1024 * 1024)).ToString ("F"));
			} else if (lastUpdateStep == UpdateHelper.UpdateStep.GetUpdateList) {
				process = 0.1f + 0.1f * updateHelper.CurDownloadSize / 1000;
				setUpdateTips ("", process);
			}
		}
		/** 释放lua缓存 */
		private void ReleaseLuaCache(Action action)
		{
			List<string> updateList = updateHelper.DownLoadList;
			foreach (var str in updateList) {
				if (str.StartsWith ("lua/")) {
					LuaPool.Instance.Release (str.Replace (ResourceHelper.SUFFIX, ""));
				}
			}
			if(action!= null)action();
		}
		/** 重新加载resourceCofig中的资源 */
		private void ReInitCache(Action action)
		{
			List<string> updateList = updateHelper.DownLoadList;
			string[] paths = GameManager.BASERESOURCES;
			List<string> tmpPaths = new List<string>();
			foreach (var path in paths) {
				if(updateList.Contains(path)){
					tmpPaths.Add(path);
					ResourceHelper.Instance.Release(path);
				}
			}
			if(tmpPaths.Count>0){
				ResourcesManager.Instance.cache(tmpPaths.ToArray(),action);
			}else{
				action();
			}
		}

		/** 切换场景 */
		public void ChangeScene ()
		{
			StartCoroutine (ToLoginScene ());
		}

		/** 切换场景 */
		private IEnumerator ToLoginScene ()
		{
			SDKHelper.saveState(StateStep.STEP17);
			UpdateHelper helper = GetComponent<UpdateHelper> ();
			if (helper != null)
				Destroy (helper);
			yield return null;
			SDKHelper.saveState (StateStep.STEP18);
			if(update){
				StateManager.Instance.LauncherState<LoginState> ();
			}
			StateManager.Instance.DoLuaFunction(LuaFuncName.Main);
		}

		/** 预加载基础资源 */
		public void InitResource (Action action)
		{
			int count = 0;
			Action action1 = () => {
				count++;
				if (count >= 3)
					action ();
			};
			ResourceHelper.Instance.LoadMainAssetAsync<GameObject> (ResourceHelper.UIPATH + "LoginWindow", (data) => {
				action1 ();
			});
			ResourceHelper.Instance.LoadResDataAsync (ResourceHelper.TEXTUREPATH + TextureHelper.BACKGROUND + "loginBack", (data) => {
				action1 ();
			});
			ResourceHelper.Instance.LoadResDataAsync (ResourceHelper.TEXTUREPATH + TextureHelper.BACKGROUND + "loginBack_3", (data) => {
				action1 ();
			});
		}
	}
}
