using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 全局资源扩展
	/// </summary>
	public static class ResourcePublicExtension
	{
		/* static methods */
		/** 添加引用 */
		public static void Retain(this ResData[] resDatas)
		{
			for (int i = 0; i < resDatas.Length; i++) 
			{
				resDatas [i].Retain ();
			}
		}
		/** 释放引用 */
		public static void Release(this ResData[] resDatas)
		{
			ResourceHelper.Instance.Release (resDatas);
		}
		/** 进度 */
		public static float GetProgress(this ResData[] resDatas)
		{
			float progress = 0;
			for (int i = 0; i < resDatas.Length; i++) 
			{
				progress+=resDatas [i].progress;
			}
			return progress / resDatas.Length;
		}
		/** 是否完成 */
		public static bool IsDone(this ResData[] resDatas)
		{
			for (int i = 0; i < resDatas.Length; i++) 
			{
				if(!resDatas [i].isDone) return false;
			}
			return true;
		}
		/** 添加前缀 */
		public static string[] AddPrefix(this string[] paths,string prefix)
		{
			for (int i = 0; i < paths.Length; i++) 
			{
				paths [i] = prefix + paths[i];
			}
			return paths;
		}
		/** 移出相同的路径 */
		public static string[] RemoveSamePath(this string[] paths)
		{
			List<string> list = new List<string> ();
			string path = null;
			for (int i = 0; i < paths.Length; i++) 
			{
				path = paths [i].ToLower ();
				if (list.Contains (path)) continue;
				list.Add (path);
			}
			return list.ToArray ();
		}
	}
}
