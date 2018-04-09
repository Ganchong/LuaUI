using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 资源池
	/// </summary>
	public class ResourcePool<T,V> : Singleton<T>,IRecoveryPool where T : class,new() where V : Object
	{
		/* fields */
		/** 资源池 */
		protected Dictionary<string,CacheResData> resDatas=new Dictionary<string, CacheResData>();
		/** 未使用的资源池 */
		protected Queue<CacheResData> unuseQueue = new Queue<CacheResData> ();
		/** 资源加载器 */
		protected ResourceHelper resourceHelper;
		/** 池子最大数量 */
		protected int poolMaxCount;
		/** 资源大小 */
		protected int maxCount;

		/* methods */
		/** 初始化 */
		protected void Init(int poolMaxCount,int maxCount,ResourceHelper resourceHelper)
		{
			this.maxCount = maxCount;
			this.poolMaxCount = poolMaxCount;
			this.resourceHelper = resourceHelper;
		}
		/** 加载资源 */
		public virtual void Load(string path,int length,System.Action<V> callBack)
		{
			path = path.ToLower ();
			length = length<this.maxCount?length:this.maxCount;
			CacheResData resData;
			if (resDatas.TryGetValue (path, out resData))
				callBack.Invoke (resData.Get<V> ());
			else 
			{
				resourceHelper.LoadResDataAsync (path, (data) => {
					if(!resDatas.TryGetValue(path,out resData))
					{
						if(unuseQueue.Count>0)
							resData=unuseQueue.Dequeue();
						else
							resData=new CacheResData(length);
						resData.resData=data;
						data.Retain();
						resDatas.Add(path,resData);
					}
					try
					{
						callBack.Invoke (resData.Get<V>());
					}
					catch(System.Exception e)
					{
						Debug.LogError(GetType().FullName+",resource load error,path="+path+",error="+e.ToString());
					}
				});
			}
		}
		/** 回收 */
		public virtual void Release(string path,UnityEngine.Object obj)
		{
			HandlePool ();
			path = path.ToLower ();
			CacheResData resData;
			if (resDatas.TryGetValue (path, out resData)) 
				resData.Recovery (obj);
			else if (obj != null)
				Object.Destroy (obj);
		}

		bool needHandle=false;
		/** 处理池子 */
		protected void HandlePool()
		{
			if (needHandle) return;
			needHandle = true;
			CoroutineHelper.EndOfFrame(ResourceHelper.Instance,HandlePoolSize);
		}

		/** 处理池子大小 */
		private void HandlePoolSize()
		{
			CacheResData resData = null;
			int maxCount = poolMaxCount;
			float lastTime = 0;
			string key = null;
			while (resDatas.Count >= maxCount) 
			{
				resData = GetLastUnUseResData (lastTime,out key);
				if(resData==null) break;
				lastTime = resData.LastTime;
				resDatas.Remove(resData.resData.path);
				resData.Release ();
				unuseQueue.Enqueue(resData);
			}
			needHandle = false;
		}

		/** 获取最后使用的资源 */
		protected CacheResData GetLastUnUseResData(float minTimeLimit,out string key)
		{
			float minTime = float.MaxValue;
			float lastTime = 0;
			CacheResData resData = null;
			key = null;
			foreach (var item in resDatas) 
			{
				if(!item.Value.CheckRelease()) continue;
				lastTime = item.Value.LastTime;
				if (lastTime > minTimeLimit && lastTime < minTime) 
				{
					minTime = lastTime;
					resData = item.Value;
					key=item.Key;
				}
			}
			return resData;
		}
	}
}
