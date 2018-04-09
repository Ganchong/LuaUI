using UnityEngine;

namespace ABSystem{
	/// <summary>
	/// 资源回收
	/// </summary>
	public interface IRecoveryPool
	{
		/* methods */
		/** 回收 */
		void Release(string path,Object obj);
	}
}
