using UnityEngine;
using System.Collections;

namespace ABSystem{
	/// <summary>
	/// 回收基础
	/// </summary>
	public class RecoveryBase : MonoBehaviour
	{
		/* fields */
		/** 资源池 */
		public IRecoveryPool recovery;
		/** 是否回收 */
		public bool recoveryed=true;
		/** 路径 */
		public string path;
		/** 是否可用 */
		public bool active;

		/* methods */
		/** 可见 */
		public virtual void OnEnable()
		{
			active = true;
		}
		/** 不可见 */
		public virtual void OnDisable()
		{
			active = false;
		}
		/** 销毁 */
		public virtual void OnDestroy()
		{
			active = false;
		}
	}
}
