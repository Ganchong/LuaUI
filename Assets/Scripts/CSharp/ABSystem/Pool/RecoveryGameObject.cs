using System;
using UnityEngine;

namespace ABSystem{
	/// <summary>
	/// 回收游戏物体
	/// </summary>
	public class RecoveryGameObject : RecoveryBase
	{
		/* fields */
		/** 游戏物体 */
		public GameObject obj;
		/** 回调 */
		public Action action;

		/* methods */
		public override void OnDisable ()
		{
			base.OnDisable ();
			#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				return;
			#endif
			this.DelayRun(1,()=>{
				if (this==null||recoveryed||active||string.IsNullOrEmpty(path)) return;
				recoveryed = true;
				recovery.Release (path, obj);
			});
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
			recovery.Release (path, obj);
			path=null;
		}
	}
}
