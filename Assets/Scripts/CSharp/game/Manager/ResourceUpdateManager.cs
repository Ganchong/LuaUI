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
		/* static fields */
		public delegate void SetUpdateTips(string tips,float process);
		public delegate void MakeSureDownLoad(string size);
		public delegate void UpdateDownLoadSize(string curSize,string totalSize);
		public delegate void DownLoadFinish();

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

		/** 初始化委托 */
		public void InitDelegate(SetUpdateTips setUpdateTips,MakeSureDownLoad makeSureDownLoad,
			UpdateDownLoadSize updateDownLoadSize,DownLoadFinish downloadFinish) 
		{
			this.setUpdateTips = setUpdateTips;
			this.makeSureDownLoad = makeSureDownLoad;
			this.updateDownLoadSize = updateDownLoadSize;
			this.downLoadFinish = downloadFinish;
			process = 0;
		}
		/** 连接服务器 */
		public void ConnectSDKServer(Action success,Action<string> fail)
		{
			SDKHelper.connectServer((message)=>{
				IJsonNode node=message;
				node=node["error"];
				if(node==null||string.IsNullOrEmpty(node.ToString()))
				{
					SDKHelper.saveState(StateStep.STEP8);
					int nowTime=message["nowTime"].ToInt();
					TimeKit.resetTime(nowTime*1000L);
					success();
				}
				else
				{
					fail(node.ToString());
				}
			});
		}
		/** 开始更新 */
		public void StartUpdate(string result)
		{
			updateHelper = gameObject.AddComponent<UpdateHelper>();
			lastUpdateStep = updateHelper.CurUpdateStep;
			updateHelper.startCheckData(SDKHelper.getUpdateDataUrl(),result);
		}
		/** 连接服务器 */
		public void AccessHttpConnect(string url,Action<string> success,Action fail)
		{
			HttpConnect.access(url,(ans,result)=>{
				if(ans==HttpConnect.OK)
				{
					if(Log.isInfoEnable()) Log.info("serverVersion:"+result);
					success(result);
				}
				else
				{
					if(Log.isInfoEnable())	Log.info("update expcetion!");
					Debug.Log(result);
					fail();
				}
			});
		}
		/** 下载当前文件列表 */
		public void DownLoadCurFileList()
		{
			updateHelper.downloadCurFileList();
			Debug.Log("downloadCurFileList");
		}
		/** 更新 */
		void Update ()
		{
			if (process < 0) return;
			if(updateHelper != null && lastUpdateStep != updateHelper.CurUpdateStep)
			{
				lastUpdateStep = updateHelper.CurUpdateStep;
				switch(lastUpdateStep)
				{
				case UpdateHelper.UpdateStep.CheckVesion:
					process = 0;
					setUpdateTips("TIP_2",process);
					break;
				case UpdateHelper.UpdateStep.GetUpdateList:
					process = 0.1f;
					setUpdateTips("TIP_3",process);
					break;
				case UpdateHelper.UpdateStep.CompareData:
					process = 0.2f;
					setUpdateTips("TIP_4",process);
					break;
				case UpdateHelper.UpdateStep.MakeSureDownload:
					SDKHelper.saveState(StateStep.STEP12);
					if(Application.internetReachability==NetworkReachability.ReachableViaCarrierDataNetwork)
					{
						makeSureDownLoad((updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F"));
						Debug.Log("makesure");
					}
					else DownLoadCurFileList();
					process = 0.3f;
					setUpdateTips("",process);
					break;
				case UpdateHelper.UpdateStep.DownloadData:
					break;
				case UpdateHelper.UpdateStep.CheckData:
					process = 0.8f;
					setUpdateTips("TIP_6",process);
					break;
				case UpdateHelper.UpdateStep.CopyData:
					process = 0.9f;
					setUpdateTips("TIP_7",process);
					break;
				case UpdateHelper.UpdateStep.CleanCache:
					break;
				case UpdateHelper.UpdateStep.Finish:
					if(updateHelper.CurUpdateResult != UpdateHelper.UpdateResult.Success)
					{
						setUpdateTips("TIP_9",process);
						SDKHelper.saveState(StateStep.STEP13);
					}
					else
					{
						SDKHelper.saveState(StateStep.STEP14);
						process=0.9f;
						int count = 0;
						Action loadFinish = ()=>{count++;if(count<1)return;downLoadFinish();};
						List<string> updateList = updateHelper.DownLoadList;
						foreach (var str in updateList) {
							if(str.StartsWith("lua/")){
								LuaPool.Instance.Release(str.Replace(ResourceHelper.SUFFIX,""));
							}
						}
						loadFinish();
					}
					break;
				}
			}
			if(lastUpdateStep == UpdateHelper.UpdateStep.DownloadData) {
				process = 0.3f + 0.4f * updateHelper.CurDownloadSize / updateHelper.DownloadTotalSize;
				setUpdateTips("",process);
				updateDownLoadSize((updateHelper.CurDownloadSize*1f/(1024*1024)).ToString("F"),(updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F"));
			} else if(lastUpdateStep == UpdateHelper.UpdateStep.GetUpdateList) {
				process = 0.1f + 0.1f * updateHelper.CurDownloadSize / 1000;
				setUpdateTips("",process);
			}
			#if UNITY_EDITOR
			if(GameManager.Instance.isSkipFlash)
			{
				setUpdateTips("",process);
			}
			#endif
		}
		public void ChangeScene()
		{
			StartCoroutine(ToLoginScene());
		}
		/** 切换场景 */
		private IEnumerator ToLoginScene()
		{
			UpdateHelper helper= GetComponent<UpdateHelper> ();
			if (helper != null) Destroy (helper);
			yield return null;
			SDKHelper.saveState(StateStep.STEP18);
			GameManager.Instance.ChangeState<LoginState>();
		}

		/** 预加载基础资源 */
		public void InitResource(Action action)
		{
			int count = 0;
			Action action1 = ()=>{count++;if(count>=3)action();};
			ResourceHelper.Instance.LoadMainAssetAsync<GameObject>(ResourceHelper.UIPATH+"LoginWindow",(data)=>{action1();});
			ResourceHelper.Instance.LoadResDataAsync(ResourceHelper.TEXTUREPATH+TextureHelper.BACKGROUND+"loginBack",(data)=>{action1();});
			ResourceHelper.Instance.LoadResDataAsync(ResourceHelper.TEXTUREPATH+TextureHelper.BACKGROUND+"loginBack_3",(data)=>{action1();});
		}
	}
}
