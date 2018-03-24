using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 缓存资源数据
	/// </summary>
	public class CacheResData
	{
		/* static fields */
		/** UI分层 */
		public static int UI=LayerMask.NameToLayer("UI");
		/** 父节点 */
		public static Transform CACHEROOT;

		/* fields */
		/** 资源 */
		public ResData resData;
		/** 引用数量 */
		public int reference;

		/** 缓存列表 */
		Queue<GameObject> unuseObjects=new Queue<GameObject>();
		/** 最大数量限制 */
		int maxCount;
		/** 使用时间 */
		float lastTime;

		/* properties */
		/** 上次使用时间 */
		public float LastTime
		{
			get
			{ 
				return lastTime;
			}
			set
			{
				this.lastTime=value;
			}
		}

		/* constuctor */
		/** 构造方法 */
		public CacheResData(int maxCount)
		{
			this.maxCount = maxCount;
		}

		/* methods */
		/** 回收 */
		public void Recovery(Object obj)
		{
			GameObject gameObj = obj as GameObject;
			if (gameObj != null) 
			{
				if (unuseObjects.Count < maxCount) {
					FinalizeObject (gameObj);
					unuseObjects.Enqueue (gameObj);
				} else {
					Object.Destroy (gameObj);
					reference--;
				}
			}
			else 
			{
				reference--;
			}
		}
		/** 获取 */
		public T Get<T> () where T : Object
		{
			lastTime = Time.unscaledTime;
			if (unuseObjects.Count > 0) 
			{
				GameObject gameObj = unuseObjects.Dequeue ();
				InitializeObject (gameObj);
				return gameObj as T;
			}
			T t= resData.LoadMainAsset<T> ();
			if(t!=null) reference++;
			if (t is GameObject) 
			{
				t=Object.Instantiate<T> (t);
				Object.DontDestroyOnLoad(t);
			}
			return t;
		}
		/** 释放 */
		public void Release()
		{
			GameObject gameObj = null;
			ResData resData = this.resData;
			while(unuseObjects.Count>0)
			{
				gameObj = unuseObjects.Dequeue ();
				Object.Destroy (gameObj);
			}
			reference = 0;
			resData.Release ();
			this.resData = null;
		}
		/** 检查能否被释放 */
		public bool CheckRelease()
		{
			return reference <= unuseObjects.Count;
		}
		/** 处理粒子系统 */
		private void InitializeObject(GameObject gameObj)
		{
			if (null != gameObj)
			{
				if (!gameObj.activeSelf) gameObj.SetActive(true);
				ParticleSystem[] pss = gameObj.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem item in pss) 
				{
					if (null != item && item.main.playOnAwake) 
					{
						item.Play(false);
					}
				}
			}
		}
		/** 处理粒子系统 */
		private void FinalizeObject(GameObject gameObj)
		{
			ParticleSystem[] pss = gameObj.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem ps in pss) 
			{
				if (null != ps) 
				{
					ps.Stop(false);
				}
			}
			#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;
			#endif
			if (gameObj.activeSelf)
				gameObj.SetActive(false);
			if (gameObj.transform.parent!=CACHEROOT)
				gameObj.transform.parent=CACHEROOT;
		}
		/** 字符串 */
		public override string ToString ()
		{
			return string.Format ("[CacheResData: resData={0}, reference={1}, count={2}]", resData, reference, unuseObjects.Count);
		}

	}
}
