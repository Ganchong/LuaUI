using UnityEngine;

namespace LuaFramework.Core{
	/// <summary>
	/// 资源更新管理器
	/// </summary>
	public class ResourceUpdateManager : SingletonBehaviour<ResourceUpdateManager> 
	{
		/** 连接服务器 */
		public void Connect()
		{
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
			
		}

		/** 检查更新 */
		public void CheckUpdate()
		{
			if(SDKHelper.enableUpdate())
			{

				if(GameManager.Instance.openSDK)
				{
					
				}else
				{
					
				}
				
			}else
			{
				InitNoUpdate();
			}
		}
		public void InitNoUpdate()
		{
			
		}
	}
}

