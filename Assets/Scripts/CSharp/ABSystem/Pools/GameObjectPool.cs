using System;
using UnityEngine;

namespace ABSystem{
	/// <summary>
	/// 对象池 用完后立即销毁了
	/// </summary>
	public class GameObjectPool : ResourcePool<GameObjectPool,GameObject>
	{
		/* constructor */
		/** 构造方法 */
		public GameObjectPool()
		{
			Init(10,8,ResourceHelper.Instance);
		}

		public override void Load (string path,int length, Action<GameObject> callBack)
		{
			base.Load (path,length, (gameObj)=>{
				#if UNITY_EDITOR
				ResourcesManager.ChangeShader(gameObj.transform);
				#endif
				callBack(gameObj);
			});
		}

		/** 加载并装载 */
		public void LoadGameObject(Action<GameObject> action, string path, Transform parent,int maxCount)
		{
			RecoveryGameObject recovery=parent.gameObject.GetOrAddComponent<RecoveryGameObject>();
			if (recovery.path == path) return;
			if (!string.IsNullOrEmpty (recovery.path) && !recovery.recoveryed) 
			{
				recovery.recoveryed=true;
				GameObjectPool.Instance.Release(recovery.path,recovery.obj);
			}
			recovery.path = path;
			recovery.recovery=GameObjectPool.Instance;
			recovery.action=()=>{
				string _path=recovery.path;
				Load (action, _path, parent, recovery,maxCount);
			};

			if (string.IsNullOrEmpty(path)||!recovery.active) return;
			Load (action, path, parent, recovery,maxCount);
		}

		/** 加载资源 */
		private void Load(Action<GameObject> action, string path, Transform parent, RecoveryGameObject recovery,int length)
		{
			GameObjectPool.Instance.Load (path,length,(gameObj) => {
				if(gameObj==null) return;
				if(recovery!=null&&path==recovery.path&&recovery.active&&recovery.recoveryed)
				{
					recovery.recoveryed=false;
					recovery.obj=gameObj;
					Transform trans=gameObj.transform;
					trans.parent = parent;
					trans.localPosition = Vector3.zero;
					trans.localScale = Vector3.one;
					trans.localRotation = Quaternion.identity;
					if(action!=null) action(gameObj);
				}
				else
				{
					GameObjectPool.Instance.Release(path,gameObj);
				}
			});
		}
	}
}
