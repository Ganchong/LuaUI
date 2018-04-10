using System;
using UnityEngine;
using System.Collections;

namespace LuaFramework.Core
{
	/// <summary>
	/// 资源更新管理器
	/// </summary>
	public class ResourceUpdateManager : SingletonBehaviour<ResourceUpdateManager> 
	{
		/* static fields */
		public delegate void SetUpdateTips(string tips);
		public delegate void SetUpdateProcess(float process);

		public SetUpdateTips setUpdateTips;
		public SetUpdateProcess setUpdateProcess;
		/* fields */
		/** 更新器 */
		UpdateHelper updateHelper;
		/** 最后更新步骤 */
		UpdateHelper.UpdateStep lastUpdateStep;
		/** 当前进度 */
		float process = -1;
		/** 是否更新 */
		bool update = false;
		/** 第一次 */
		bool firstTime = true;

//		/** 更新 */
//		protected override void Update ()
//		{
//			if (process < 0) return;
//			if(updateHelper != null && lastUpdateStep != updateHelper.CurUpdateStep)
//			{
//				lastUpdateStep = updateHelper.CurUpdateStep;
//				switch(lastUpdateStep)
//				{
//				case UpdateHelper.UpdateStep.CheckVesion:
//					tipLabel.text = Language.get("TIP_2");
//					update=false;
//					process = 0;
//					break;
//				case UpdateHelper.UpdateStep.GetUpdateList:
//					tipLabel.text = Language.get("TIP_3");;
//					process = 0.1f;
//					break;
//				case UpdateHelper.UpdateStep.CompareData:
//					tipLabel.text = Language.get("TIP_4");;
//					process = 0.2f;
//					break;
//				case UpdateHelper.UpdateStep.MakeSureDownload:
//					SDKHelper.saveState(StateStep.STEP12);
//					if(Application.internetReachability==NetworkReachability.ReachableViaCarrierDataNetwork)
//					{
//						Alert.Show((ans)=>{
//							updateHelper.downloadCurFileList();
//							MaskWindow.unLockUI();
//						},Language.get("network_1",(updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F")),false);
//					}
//					else updateHelper.downloadCurFileList();
//					update=true;
//					process = 0.3f;
//					break;
//				case UpdateHelper.UpdateStep.DownloadData:
//					break;
//				case UpdateHelper.UpdateStep.CheckData:
//					tipLabel.text = Language.get("TIP_6");
//					process = 0.8f;
//					break;
//				case UpdateHelper.UpdateStep.CopyData:
//					tipLabel.text = Language.get("TIP_7");
//					process = 0.9f;
//					break;
//				case UpdateHelper.UpdateStep.CleanCache:
//					break;
//				case UpdateHelper.UpdateStep.Finish:
//					if(updateHelper.CurUpdateResult != UpdateHelper.UpdateResult.Success)
//					{
//						tipLabel.text = Language.get("TIP_9");
//						SDKHelper.saveState(StateStep.STEP13);
//					}
//					else
//					{
//						SDKHelper.saveState(StateStep.STEP14);
//						tipLabel.text = Language.get("TIP_7");
//						process=0.9f;
//						Action initFinish=()=>{
//							SDKHelper.saveState(StateStep.STEP16);
//							process = 1;
//							tipLabel.text = Language.get("TIP_8");
//						};
//						if(update)
//						{
//							SDKHelper.saveState(StateStep.STEP15);
//							int count=0;
//							Action loadFinish=()=>{count++;if(count<4)return;initFinish();};
//							List<string> updateList=updateHelper.DownLoadList;
//							if(updateList.Contains((Language.LANGUAGE+ResourceHelper.SUFFIX).ToLower()))
//								Language.InitLanguage(loadFinish);
//							else loadFinish();
//							if(firstTime||updateList.Contains((ConfigHelper.NORMALCONFIG+ResourceHelper.SUFFIX).ToLower()))
//								ConfigHelper.Instance.InitConfig(loadFinish,setResourceProcess);
//							else loadFinish();
//							UiManager.Instance.initResDataAndLoad(loadFinish,updateList);
//							StartCoroutine(SampleFactory.ResetAll(loadFinish,updateList));
//							ResourceHelper.Instance.InitManifest(()=>{initResource(loadFinish);});
//						}
//						else
//						{
//							if(firstTime) initNoUpdate(initFinish);
//							else initFinish();
//						}
//					}
//					break;
//				}
//			}
//			if(lastUpdateStep == UpdateHelper.UpdateStep.DownloadData) {
//				tipLabel.text = TextKit.parse(Language.get("TIP_5"),(updateHelper.CurDownloadSize*1f/(1024*1024)).ToString("F"),(updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F"));
//				process = 0.3f + 0.4f * updateHelper.CurDownloadSize / updateHelper.DownloadTotalSize;
//			} else if(lastUpdateStep == UpdateHelper.UpdateStep.GetUpdateList) {
//				process = 0.1f + 0.1f * updateHelper.CurDownloadSize / 1000;
//			}
//
//			if(slider.value < process)
//			{
//				slider.value=Mathf.Lerp(slider.value,process,LoadingWindow.Speed);
//			}
//
//			#if UNITY_EDITOR
//			if(GameManager.Instance.fast)
//			{
//				slider.value=process;
//			}
//			#endif
//			if(slider.value >= 0.99f)
//			{
//				updateFinish();
//				process = -1;
//			}
//		}
		/** 更新完成 */
		private void updateFinish()
		{
//			firstTime = false;
//			tipLabel.text = Language.get("TIP_8");
//			StartCoroutine (ToLoginScene());
//			SDKHelper.saveState(StateStep.STEP17);
		}
		/** 切换场景 */
		private IEnumerator ToLoginScene()
		{
			UpdateHelper helper= GetComponent<UpdateHelper> ();
			if (helper != null) Destroy (helper);
			yield return null;
//			if (GameManager.Instance.ScenceID != LoadingWindow.LOGIN) 
//			{
//				GameManager.Instance.ScenceID = LoadingWindow.LOGIN;
//				var operation=SceneManager.LoadSceneAsync(LoadingWindow.LOGIN);
//				yield return operation;
//			}
			SDKHelper.saveState(StateStep.STEP18);
			GameManager.Instance.ChangeState<LoginState>();
		}

		/** 进度 */
		private void setResourceProcess(float process)
		{
			this.process = 0.9f + process / 10;
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
		public void AccessHttpConnect(string url,Action success,Action fail)
		{
			HttpConnect.access(url,(ans,result)=>{
				if(ans==HttpConnect.OK)
				{
					if(Log.isInfoEnable()) Log.info("serverVersion:"+result);
					success();
				}
				else
				{
					if(Log.isInfoEnable())	Log.info("update expcetion!");
					fail();
				}
			});
		}
	}
}
