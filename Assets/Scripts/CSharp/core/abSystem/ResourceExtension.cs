using System;
using UnityEngine;

namespace ABSystem
{
	/// <summary>
	/// 内置扩展
	/// </summary>
	public static class ResourceExtension
	{
		/* static methods */
		/** 减少引用 */
		public static void release(this ResData resData)
		{
			resData.retainCount--;
			if (resData.references != null) 
			{
				for (int i = 0; i < resData.references.Length; i++) 
				{
					resData.references [i].Release();
				}
			}
			if (resData.retainCount > 0) return;
			release (resData, true);
		}
		/** 释放 */
		public static void release(this ResData resData,bool force)
		{
			if (force&&!(resData._object is GameObject)) 
			{
				Resources.UnloadAsset (resData._object);
				if(resData.alpha) Resources.UnloadAsset(resData.alpha);
				resData._object=null;
				resData.textures=null;
				resData.alpha=null;
				force=false;
			}
			if (force) 
			{
				resData._object=null;
				resData.textures=null;
				resData.alpha=null;
			}
			if (resData.assetBundle == null) return;
			resData.assetBundle.Unload (force);
			resData.assetBundle = null;
		}
		/** 设置加载完成 */
		public static void Finish(this ResData resData)
		{
			resData._isDone = true;
			resData._progress = 1;
		}
	}
}