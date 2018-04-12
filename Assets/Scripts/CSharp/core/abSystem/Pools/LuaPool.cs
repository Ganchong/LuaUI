using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABSystem
{
	/// <summary>
	/// Lua文件缓存池
	/// </summary>
	public class LuaPool : ResourcePool<LuaPool,TextAsset>
	{
		/* constructor */
		/** 构造方法 */
		public LuaPool ()
		{
			Init (10, 8, ResourceHelper.Instance);
		}

		/// <summary>
		/// 同步加载lua资源
		/// </summary>
		public override void Load (string path, int length, System.Action<TextAsset> callBack)
		{
			int index = path.LastIndexOf ('/');
			string fileName = index > 0 ? path.Substring (index + 1) + ".lua" : path + ".lua";
			path = path.ToLower ();
			path = "lua/" + path;
			length = length < this.maxCount ? length : this.maxCount;
			CacheResData resData;
			if (resDatas.TryGetValue (path, out resData)) {
				resData.reference++;
				resData.LastTime = Time.unscaledTime;
				callBack.Invoke (resData.resData.LoadAsset<TextAsset> (fileName));
			} else {
				HandlePool ();
				ResData data = resourceHelper.LoadResData (path);
				if (!resDatas.TryGetValue (path, out resData)) {
					if (unuseQueue.Count > 0)
						resData = unuseQueue.Dequeue ();
					else
						resData = new CacheResData (maxCount);
					resData.resData = data;
					data.Retain ();
					resDatas.Add (path, resData);
				}
				resData.reference++;
				resData.LastTime = Time.unscaledTime;
				callBack.Invoke (resData.resData.LoadAsset<TextAsset> (fileName));
			}
		}

		/** 释放 */
		public void Release (string path)
		{
			CacheResData resData;
			if (resDatas.TryGetValue (path, out resData)) {
				resData.Release ();
				resDatas.Remove (path);
				unuseQueue.Enqueue (resData);
			}
		}
	}

}
