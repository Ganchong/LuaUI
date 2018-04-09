using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using LuaFramework.Core;

public static class LuaHelper {

	/// <summary>
	/// 资源更新管理器
	/// </summary>
	public static ResourceUpdateManager GetResourceUpdateManager()
	{
		return ResourceUpdateManager.Instance;
	}
}


