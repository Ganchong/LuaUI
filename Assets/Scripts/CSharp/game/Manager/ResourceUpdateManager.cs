using System;
using UnityEngine;

namespace LuaFramework.Core
{
	/// <summary>
	/// 资源更新管理器
	/// </summary>
	public class ResourceUpdateManager : SingletonBehaviour<ResourceUpdateManager> 
	{
		/** 更新器 */
		UpdateHelper updateHelper;
		/** 最后更新步骤 */
		UpdateHelper.UpdateStep lastUpdateStep;
		/** 当前更新进度 */
		float process = -1;
		/** 是否更新 */
		float update = false;
		/** 是否是第一次 */
		bool firstTime = true;
		/** 更新回调 */
		Action<int,float> action;

		/** 连接服务器 */
		public void Connect(Action<int,float> action)
		{
			this.action = action;
			SDKHelper.saveState(StateStep.STEP7);
			SendMsgToLua(StateStep.STEP7);
			if(GameManager.Instance.openSDK)
			{
				ConnectSDKServer();
			}else
			{
				CheckUpdate();
			}
		}

		/** 连接SDK服务器 */
		public void ConnectSDKServer()
		{
			SDKHelper.connectServer((message)=>{
				IJsonNode node=message;
				node=node["error"];
				if(node==null||string.IsNullOrEmpty(node.ToString()))
				{
					SDKHelper.saveState(StateStep.STEP8);
					SendMsgToLua(StateStep.STEP8);
					int nowTime=message["nowTime"].ToInt();
					TimeKit.resetTime(nowTime*1000L);
					CheckUpdate();
				}
				else
				{
					if(node.ToString()=="version")
					{
						string downUrl=SDKHelper.getDownUrl();
						if(!string.IsNullOrEmpty(downUrl))
							Application.OpenURL(downUrl);
						Application.Quit();
						return;
					}
					ConnectSDKServer();
					SDKHelper.saveState(StateStep.STEP9);
					SendMsgToLua(StateStep.STEP9);
				}
			});
		}

		/** 检查更新 */
		public void CheckUpdate()
		{
			if(SDKHelper.enableUpdate())
			{
				if(GameManager.Instance.openSDK)
				{
					SDKHelper.saveState(StateStep.STEP10);
					SendMsgToLua(StateStep.STEP10);
					updateHelper = gameObject.AddComponent<UpdateHelper>();
					lastUpdateStep = updateHelper.CurUpdateStep;
					updateHelper.startCheckData(SDKHelper.getUpdateDataUrl(),SDKHelper.getInfo(Handler.RESOURCEVERSION));
				}
				else
				{
					string url = SDKHelper.getUpdateDataUrl();
					int index = url.IndexOf('/',8);
					url = url.Substring(0,index)+"/getVersion?1=1";
					if(Log.isInfoEnable())Log.info("version url:"+url);
					HttpConnect.access(url,(ans,result)=>{
						if(ans==HttpConnect.OK)
						{
							if(Log.isInfoEnable())Log.info("serverVersion :"+result);
							updateHelper = gameObject.AddComponent<UpdateHelper>();
							lastUpdateStep = updateHelper.CurUpdateStep;
							updateHelper.startCheckData(SDKHelper.getUpdateDataUrl(),result);
						}
						else
						{
							if(Log.isInfoEnable())Log.info("update expection");
							process = 0.9f;
							InitNoUpdate(()=>{
								process = 1;
							});
						}
					});
				}
				
			}else
			{
				process = 0.9f;
				InitNoUpdate(()=>{
					process = 1;
				});
			}
		}
		public void InitNoUpdate(Action action)
		{
			
		}

		public void UpdateFinish()
		{
			firstTime = false;
			SDKHelper.saveState(StateStep.STEP17);
			SendMsgToLua(StateStep.STEP17);
		}

		public void SendMsgToLua(StateStep step)
		{
			if(action!=null)action((int)step,process);
		}
	}
}

