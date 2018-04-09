using System;
using UnityEngine;

namespace ABSystem{
	/// <summary>
	/// 资源池
	/// </summary>
	public class TexturePool : ResourcePool<TexturePool,Texture>
	{
		/* constructor */
		/** 构造方法 */
		public TexturePool()
		{
			Init (15, 1, ResourceHelper.Instance);
		}
		public void Load(string path,Action<Texture,Texture> callBack)
		{
			path = path.ToLower ();
			Texture mainTex,alphaTex;
			CacheResData resData;
			if (resDatas.TryGetValue (path, out resData)) 
			{
				resData.resData.LoadTextureAsset(out mainTex,out alphaTex);
				resData.reference++;
				resData.LastTime=Time.unscaledTime;
				callBack.Invoke (mainTex,alphaTex);
			}
			else 
			{
				HandlePool();
				ResData data=resourceHelper.LoadResData (path);
				if(!resDatas.TryGetValue(path,out resData))
				{
					if(unuseQueue.Count>0)
						resData=unuseQueue.Dequeue();
					else 
						resData=new CacheResData(maxCount);
					resData.resData=data;
					data.Retain();
					resDatas.Add(path,resData);
				}
				resData.reference++;
				resData.resData.LoadTextureAsset(out mainTex,out alphaTex);
				resData.LastTime=Time.unscaledTime;
				callBack.Invoke (mainTex,alphaTex);
			}
		}
	}
}
