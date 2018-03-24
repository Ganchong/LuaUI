using System;
using UnityEngine;
namespace ABSystem{
	/// <summary>
	/// 模型池
	/// </summary>
	public class ModelPool : ResourcePool<ModelPool,GameObject>
	{
		/* constructor */
		/** 构造方法 */
		public ModelPool()
		{
			Init(13,1,ResourceHelper.Instance);
		}
		public override void Load (string path,int length, Action<GameObject> callBack)
		{
			base.Load (path,length, (gameObj)=>{
				#if UNITY_EDITOR
				ResourcesManager.ChangeShader(gameObj.transform);
				#endif
				if(gameObj!=null)
				{
					//TODO 初始化
				}
				callBack(gameObj);
			});
		}

		/** 加载并装载 */
		public void LoadModel(Action<GameObject> action, string path, Transform parent)
		{
			path = string.IsNullOrEmpty(path) ? null : ResourceHelper.MODELPATH + path;
			RecoveryGameObject recovery=parent.gameObject.GetOrAddComponent<RecoveryGameObject>();
			if (recovery.path == path) return;
			if (!string.IsNullOrEmpty (recovery.path) && !recovery.recoveryed) 
			{
				recovery.recoveryed=true;
				ModelPool.Instance.Release(recovery.path,recovery.obj);
			}
			recovery.path = path;
			recovery.recovery=ModelPool.Instance;
			recovery.action=()=>{
				string _path=recovery.path;
				Load (action, _path, parent, recovery);
			};

			if (string.IsNullOrEmpty(path)||!recovery.active) return;
			Load (action, path, parent, recovery);
		}

		/** 加载资源 */
		private void Load(Action<GameObject> action, string path, Transform parent, RecoveryGameObject recovery)
		{
			ModelPool.Instance.Load (path,1, (gameObj) => {
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
					ModelPool.Instance.Release(path,gameObj);
				}
			});
		}
	}
}
