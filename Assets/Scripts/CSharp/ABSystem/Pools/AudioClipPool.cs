using System;
using UnityEngine;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 资源池
	/// </summary>
	public class AudioClipPool : ResourcePool<AudioClipPool,AudioClip>
	{
		/* fields */
		/** 地址池 */
		Dictionary<AudioClip,string> map=new Dictionary<AudioClip, string>();

		/* constructor */
		/** 构造方法 */
		public AudioClipPool()
		{
			Init (10, 1, ResourceHelper.Instance);
		}

		/* methods */
		/** 加载 */
		public override void Load (string path,int length, Action<AudioClip> callBack)
		{
			base.Load (path,length, (audio)=>{
				#if UNITY_EDITOR
				if(audio==null)
				{
					Debug.LogError("audio is null,path:"+path);
				}
				#endif
				if(audio!=null&&!map.ContainsKey(audio))
					map.Add(audio,path);
				callBack(audio);
			});
		}
		/** 释放 */
		public void Release(AudioClip audio)
		{
			Release (map [audio], audio);
			if (audio == null)map.Remove (audio);
		}
	}
}
