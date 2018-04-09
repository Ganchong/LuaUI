using System;
using UnityEngine;
using System.Collections;

namespace ABSystem{
	/// <summary>
	/// 释放资源
	/// </summary>
	public class RecoveryAsset : RecoveryBase
	{
		/** 重新加载回调 */
		public Action action;

		/* methods */
		public override void OnDisable ()
		{
			base.OnDisable ();
			if (recoveryed||string.IsNullOrEmpty(path)) return;
			recovery.Release (path, null);
			recoveryed = true;
		}
		public override void OnEnable ()
		{
			base.OnEnable ();
			if (string.IsNullOrEmpty (path)||!recoveryed) return;
			if(action!=null) action ();
		}
		public override void OnDestroy ()
		{
			base.OnDestroy ();
			#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;
			#endif
			if (recoveryed||string.IsNullOrEmpty(path)) 
			{
				path=null;
				return;
			}
			recoveryed = true;
			recovery.Release (path, null);
			path = null;
		}
	}
}
